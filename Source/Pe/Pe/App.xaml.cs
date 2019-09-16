using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Prism;

namespace ContentTypeTextNet.Pe.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif
            var initializer = new ApplicationInitializer();
            initializer.Initialize(this, e);
            /*
            if(!ApplicationManager.Startup(this, e)) {
                Shutdown();
                return;
            }

            var viewModel = ApplicationManager.CreateViewModel();
            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
            notifyIcon.DataContext = viewModel;

            ApplicationManager.Execute();
            */
            Shutdown();
        }
    }
}
