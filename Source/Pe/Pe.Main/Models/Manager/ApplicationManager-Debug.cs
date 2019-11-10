using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
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

            //DebugColorPicker();
            //Exit();
        }

        void DebugExecuteAfter()
        {
            Logger.LogDebug("デバッグ用後処理");

            //DebugCustomize();
            //DebugExtendsExecute();
            DebugStdIoExecute();
        }

        void DebugCustomize()
        {
            // LauncherGroups.Sequence を調整すること
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen);
            }
        }

        void DebugExtendsExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenExtendsExecuteView(Screen.PrimaryScreen);
            }
        }

        void DebugStdIoExecute()
        {
            var batchPath = @".\temp.bat";
            File.WriteAllText(batchPath, @"
echo test1
echo test2
ping localhost
ping 127.0.0.1
ping localhost
ping 127.0.0.1
echo end
            ", Encoding.GetEncoding("shift_jis"));
            var launcherExecutor = new LauncherExecutor(OrderManager, LoggerFactory);
            var data = new LauncherFileData() {
                Path = batchPath,
                //Path = "cmd",
                //Option = "/c " + batchPath,
                IsEnabledStandardInputOutput = true,
            };
            var env = new List<LauncherEnvironmentVariableData>();
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, Screen.PrimaryScreen);
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
