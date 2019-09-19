using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region function

        void DebugExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen);
            }
        }

        #endregion
    }
#endif
}
