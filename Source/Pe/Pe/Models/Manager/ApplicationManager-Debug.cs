using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.ViewModels._Debug_;
using ContentTypeTextNet.Pe.Main.Views._Debug_;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region function

        void DebugExecuteBefore()
        {
            Logger.LogDebug("デバッグ用前処理");

            DebugColorPicker();
            Exit();
        }

        void DebugExecuteAfter()
        {
            Logger.LogDebug("デバッグ用後処理");

            //DebugCustomize();
        }

        void DebugCustomize()
        {
            // LauncherGroups.Sequence を調整すること
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen);
            }
        }

        void DebugColorPicker()
        {
            using(var di = ApplicationDiContainer.CreateChildContainer()) {
                di.RegisterMvvm<DebugColorPickerElement, DebugColorPickerViewModel, DebugColorPickerWindow>();
                var model = di.Build<DebugColorPickerElement>();
                var view = di.Build<DebugColorPickerWindow>();
                var windowItem = new WindowItem(WindowKind.Debug, view);
                WindowManager.Register(windowItem);
                view.ShowDialog();
            }
        }
        #endregion
    }
#endif
}
