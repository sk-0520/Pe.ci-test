using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationManager
    {
        public ApplicationManager()
        { }

        #region property

        ILogger Logger { get; set; }

        #endregion

        #region function

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(!initializer.Initialize(e.Args)) {
                return false;
            }

            Logger = DiContainer.Instance.Get<ILoggerFactory>().CreateCurrentClass();
            Logger.Debug("初期化完了");

            return true;
        }

        #endregion
    }
}
