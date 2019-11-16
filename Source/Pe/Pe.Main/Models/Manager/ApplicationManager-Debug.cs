using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
            //DebugStdIoExecute();
            //DebugHook();
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
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
test
echo end
            ", Encoding.GetEncoding("shift_jis"));
            var launcherExecutor = new LauncherExecutor(OrderManager, new ApplicationDispatcherWapper(), LoggerFactory);
            var data = new LauncherFileData() {
                //Path = batchPath,
                Path = "cmd",
                Option = "/c " + batchPath,
                IsEnabledStandardInputOutput = true,
            };
            var env = new List<LauncherEnvironmentVariableData>();
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, Screen.PrimaryScreen);
        }

        KeyboradHooker? dbgKeyboradHooker { get; set; }
        MouseHooker? dbgMouseHooker { get; set; }
        void DebugHook()
        {
            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
                Logger.LogTrace("UP: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.KeyUp += (sender, e) => {
                Logger.LogTrace("DW: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.Register();

            dbgMouseHooker = new MouseHooker(LoggerFactory);
            //dbgMouseHooker.Register();
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
