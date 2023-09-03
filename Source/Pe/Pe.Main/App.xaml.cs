using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application
    {
        #region property

        private ApplicationManager? ApplicationManager { get; set; }
        private ILogger? Logger { get; set; }
        private RunMode RunMode { get; set; }
        private bool CachedUnhandledException { get; set; }

        private bool VisibleErrorDialog => RunMode != RunMode.InterProcessCommunication;

        #endregion

        #region function

        private (bool runSpecialMode, int exitCode) ExecuteIfExistsSpecialModeEnvironment(string[] arguments)
        {
            //var ce = new ApplicationConsoleExecutor();
            //ce.Run("DRY-RUN", arguments);
            //var a = true; if(a) return true;

            var appSpecialMode = Environment.GetEnvironmentVariable("PE_SPECIAL_MODE");
            if(string.IsNullOrWhiteSpace(appSpecialMode)) {
                return (false, 0);
            }

            var specialExecutor = new ApplicationSpecialExecutor();
            return (true, specialExecutor.Run(appSpecialMode, arguments));
        }

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();

            var special = ExecuteIfExistsSpecialModeEnvironment(e.Args);
            if(special.runSpecialMode) {
                Shutdown(special.exitCode);
                return;
            }

            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif

            var initializer = new ApplicationInitializer();
            var accepted = initializer.Initialize(this, e);
            if(!accepted) {
                Shutdown();
                return;
            }
            Debug.Assert(initializer.Logging != null);

            Logger = initializer.Logging.Factory.CreateLogger(GetType());
            RunMode = initializer.RunMode;

            switch(initializer.RunMode) {
                case Models.Data.RunMode.Normal: {
                        ApplicationManager = new ApplicationManager(initializer);

                        if(!ApplicationManager.Startup(this, e)) {
                            Shutdown();
                            return;
                        }

                        var viewModel = ApplicationManager.CreateViewModel();
                        ApplicationManager.Execute();

                        Dispatcher.BeginInvoke(new Action(() => {
                            Logger.LogInformation("つかえるよ！ 所要時間: {0}", stopwatch.Elapsed);

                            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
                            notifyIcon.DataContext = viewModel;

                            ApplicationManager.CompleteStartup();
                        }), System.Windows.Threading.DispatcherPriority.SystemIdle);
                    }
                    break;

                case Models.Data.RunMode.CrashReport: {
                        ShutdownMode = ShutdownMode.OnMainWindowClose;
                        var options = new Standard.Base.CommandLineSimpleConverter<CrashReport.Models.Data.CrashReportOptions>(new Standard.Base.CommandLine(e.Args, false)).GetMappingData();
                        if(options == null) {
                            Logger.LogError("クラッシュレポート起動できず: {0}", string.Join(" ", e.Args));
                            Shutdown(-1);
                            return;
                        }
                        var model = new CrashReport.Models.Element.CrashReportElement(options, initializer.Logging.Factory);
                        model.Initialize();
                        var viewModel = new CrashReport.ViewModels.CrashReportViewModel(model, new Models.Telemetry.UserTracker(initializer.Logging.Factory), new ApplicationDispatcherWrapper(Timeout.InfiniteTimeSpan), initializer.Logging.Factory);
                        MainWindow = new CrashReport.Views.CrashReportWindow() {
                            DataContext = viewModel,
                        };
                        MainWindow.Show();
                    }
                    break;

                case RunMode.InterProcessCommunication: {
                        var ipcManager = new InterProcessCommunicationManager(initializer, e);
                        ipcManager.Execute();
                        Shutdown();
                        return;
                    }

                default:
                    throw new NotImplementedException();
            }

        }

        #endregion

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(!CachedUnhandledException) {
                CachedUnhandledException = true;
            } else {
                // すでになんかしてるならもう何もしない
                Shutdown(2);
                return;
            }

            if(Logger != null) {
                Logger.LogError(e.Exception, "{0}, {1}", e.Dispatcher.Thread.ManagedThreadId, e.Exception.Message);
                Logger.LogInformation("RunMode: {0}", RunMode);
                if(ApplicationManager is not null) {
                    Logger.LogInformation("CanSendCrashReport: {0}", ApplicationManager.CanSendCrashReport);
                    if(ApplicationManager.CanSendCrashReport) {
                        // ふりしぼれ最後の輝き
                        Logger.LogInformation("生クラッシュレポートファイルを吐き出し");
                        var outputFile = ApplicationManager.OutputRawCrashReport(e.Exception);
                        Logger.LogInformation("生クラッシュレポートファイル: {0}", outputFile);
                        Logger.LogInformation("クラッシュレポート送信処理立ち上げ...");
                        ApplicationManager.ExecuteCrashReport(outputFile);

                        e.Handled = ApplicationManager.UnhandledExceptionHandled;
                    }
                } else {
                    if(VisibleErrorDialog) {
                        MessageBox.Show(e.Exception.ToString(), $"[{RunMode}] {e.Exception.GetType().FullName}");
                    }
                }
            } else {
                if(VisibleErrorDialog) {
                    MessageBox.Show(e.Exception.ToString(), $"[{RunMode}] {e.Exception.GetType().FullName}");
                }
            }

            Shutdown(1);
        }
    }
}
