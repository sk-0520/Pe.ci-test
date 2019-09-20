using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region function

        void DebugExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Library.Shared.Library.Compatibility.Forms.Screen.PrimaryScreen);
            }
        }

        #endregion
    }
#endif
}
