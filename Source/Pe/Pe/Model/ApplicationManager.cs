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

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationManager
    {
        public ApplicationManager()
        { }

        #region property



        #endregion

        #region function

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(initializer.Initialize(e.Args)) {
                return false;
            }

            return true;
        }

        #endregion
    }
}
