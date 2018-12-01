using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Model;

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
            if(!ApplicationManager.Startup(this, e)) {
                Shutdown();
            }
        }

        #endregion
    }
}
