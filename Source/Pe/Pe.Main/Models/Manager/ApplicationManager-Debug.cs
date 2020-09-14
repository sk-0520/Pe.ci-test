#pragma warning disable S3261 // Namespaces should not be empty

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.ViewModels._Debug_;
using ContentTypeTextNet.Pe.Main.Views._Debug_;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region property

        bool IsDevDebug { get; } = !true;

        #endregion


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
            //DebugKeyAction();
            //DebugSetting();
            //ShowCommandView();
            //ShowAboutView();
            //DebugEnvironmentExecuteFile();
            //ShowFeedbackView();
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
            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache.Instance, OrderManager, NotifyManager, new ApplicationDispatcherWrapper(Timeout.InfiniteTimeSpan), LoggerFactory);
            var data = new LauncherFileData() {
                //Path = batchPath,
                Path = "cmd",
                Option = "/c " + batchPath,
                IsEnabledStandardInputOutput = true,
            };
            var env = new List<LauncherEnvironmentVariableData>();
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, LauncherRedoData.GetDisable(), Screen.PrimaryScreen);
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

        KeyActionChecker? dbgKeyActionChecker { get; set; }
        KeyActionAssistant? dbgKeyActionAssistant { get; set; }
        void DebugKeyAction()
        {
            dbgKeyActionChecker = new KeyActionChecker(LoggerFactory);
            dbgKeyActionAssistant = new KeyActionAssistant(LoggerFactory);
            dbgKeyActionAssistant.SelfJobInputId = dbgKeyActionChecker.SelfJobInputId;

            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.X,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.L,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData(Guid.NewGuid(), false),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.E,
                    Super = ModifierKey.Any,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(Guid.NewGuid(), Key.LeftShift),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.LeftCtrl,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData(Guid.NewGuid(), Key.X),
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.RightShift,
                }
            ));

            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, e.modifierKeyStatus, e.kbdll);
                if(jobs.Any()) {
                    e.Handled = true;
                    Task.Run(() => {
                        foreach(var job in jobs) {
                            if(job.CommonData.KeyActionKind == KeyActionKind.Replace) {
                                var replaceJob = (KeyActionReplaceJob)job;
                                dbgKeyActionAssistant.ExecuteReplaceJob(replaceJob, e.modifierKeyStatus);
                            }
                        }
                    });
                }
            };
            dbgKeyboradHooker.KeyUp += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, new ModifierKeyStatus(), e.kbdll);
                Task.Run(() => {
                    dbgKeyActionAssistant.CleanupReplaceJob(e.Key, e.modifierKeyStatus);

                    foreach(var job in jobs) {
                        // 何もやることはないハズ
                    }
                });
            };
            dbgKeyboradHooker.Register();
        }

        void DebugSetting()
        {
            ShowSettingView();
        }
        void DebugColorPicker()
        {
            using(var di = ApplicationDiContainer.CreateChildContainer()) {
                di.RegisterMvvm<DebugColorPickerElement, DebugColorPickerViewModel, DebugColorPickerWindow>();
                var model = di.Build<DebugColorPickerElement>();
                var view = di.Build<DebugColorPickerWindow>();
                var windowItem = new WindowItem(WindowKind.Debug, model,view);
                WindowManager.Register(windowItem);
                view.ShowDialog();
            }
        }

        void DebugEnvironmentExecuteFile()
        {
            var eef = ApplicationDiContainer.Build<Platform.EnvironmentExecuteFile>();
            var pef = eef.GetPathExecuteFiles();

            var cmd = eef.Get("cmd", pef);
            var powershell = eef.Get("powershell", pef);
            var pwsh = eef.Get("pwsh", pef);
        }

        public void DebugStartupEnd()
        {
            //DebugSetting();
        }

        #endregion
    }
#endif
}
