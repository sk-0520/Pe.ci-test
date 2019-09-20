using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Model;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.ViewModel.Manager;

namespace ContentTypeTextNet.Pe.Main
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region property

        ApplicationManager ApplicationManager { get; } = new ApplicationManager();

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if DEBUG
            DebugStartup();
#endif
            if(!ApplicationManager.Startup(this, e)) {
                Shutdown();
                return;
            }

            var viewModel = ApplicationManager.CreateViewModel();
            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("root");
            notifyIcon.DataContext = viewModel;

            ApplicationManager.Execute();
        }

        #endregion
    }
}
