using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism;

namespace ContentTypeTextNet.Pe.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region property

        ApplicationManager? ApplicationManager { get; set; }
        ILogger? Logger { get; set; }
        RunMode RunMode { get; set; }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();

            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif
            var initializer = new ApplicationInitializer();
            var accepted = initializer.Initialize(this, e);
            Debug.Assert(initializer.Logging != null);
            if(!accepted) {
                Shutdown();
                return;
            }

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

                        var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
                        notifyIcon.DataContext = viewModel;

                        Dispatcher.BeginInvoke(new Action<Stopwatch>(sw => {
                            Logger.LogInformation("つかえるよ！ 所要時間: {0}", sw.Elapsed);
                            ApplicationManager.DelayCheckUpdateAsync().ConfigureAwait(false);

                            throw new Exception("#503");

                        }), System.Windows.Threading.DispatcherPriority.SystemIdle, stopwatch);
                    }
                    break;

                case Models.Data.RunMode.CrashReport: {
                        ShutdownMode = ShutdownMode.OnMainWindowClose;
                        var options = new Core.Models.CommandLineSimpleConverter<CrashReport.Models.Data.CrashReportOptions>(new Core.Models.CommandLine(e.Args, false)).GetMappingData();
                        if(options == null) {
                            Logger.LogError("クラッシュレポート起動できず: {0}", string.Join(" ", e.Args));
                            Shutdown(-1);
                            return;
                        }
                        var model = new CrashReport.Models.Element.CrashReportElement(options, initializer.Logging.Factory);
                        var viewModel = new CrashReport.ViewModels.CrashReportViewModel(model, new Models.Telemetry.UserTracker(initializer.Logging.Factory), new ApplicationDispatcherWrapper(), initializer.Logging.Factory);
                        MainWindow = new CrashReport.Views.CrashReportWindow() {
                            DataContext = viewModel,
                        };
                        MainWindow.Show();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(Logger != null) {
                Logger.LogCritical(e.Exception, "{0}, {1}", e.Dispatcher.Thread.ManagedThreadId, e.Exception.Message);
                Debug.Assert(ApplicationManager != null);
                if(RunMode == RunMode.Normal && ApplicationManager.EnvironmentParameters.Configuration.General.SendCrashReport) {
                    // ふりしぼれ最後の輝き
                    e.Handled = ApplicationManager.EnvironmentParameters.Configuration.General.UnhandledExceptionHandled;
                }
            } else {
                MessageBox.Show(e.Exception.ToString());
            }

            Shutdown(1);
        }
    }
}
