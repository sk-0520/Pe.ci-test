using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main;
using ContentTypeTextNet.Pe.Main.Models.Applications;
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
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif
            var initializer = new ApplicationInitializer();
            initializer.Initialize(this, e);

            Logger = initializer.LoggerFactory.CreateLogger(GetType());

            ApplicationManager = new ApplicationManager(initializer);

            if(!ApplicationManager.Startup(this, e)) {
                Shutdown();
                return;
            }

            var viewModel = ApplicationManager.CreateViewModel();
            //var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
            //notifyIcon.DataContext = viewModel;

            ApplicationManager.Execute();

            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
            notifyIcon.DataContext = viewModel;
            //Shutdown();

            Dispatcher.BeginInvoke(new Action(() => {
                Logger.LogInformation("つかえるよ！");
            }), System.Windows.Threading.DispatcherPriority.SystemIdle);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(Logger != null) {
                Logger.LogError(e.Exception, "{0}, {1}", e.Dispatcher.Thread.ManagedThreadId, e.Exception.Message);
            } else {
                MessageBox.Show(e.Exception.ToString());
            }
        }
    }
}
