using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region function

        private void MakeMessageWindow()
        {
            /*
            var thread = new Thread(() => {
                MessageWindowDispatcherWapper = new CurrentDispatcherWapper();
                MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                    Width = 0,
                    Height = 0,
                    WindowStyle = (int)WindowStyle.None,
                    //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                    HwndSourceHook = MessageWindowProc,
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            */
            MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                Width = 0,
                Height = 0,
                WindowStyle = (int)WindowStyle.None,
                //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                HwndSourceHook = MessageWindowProc,
            });
        }

        private IntPtr MessageWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Logger.LogTrace("[MSG WND] hwnd = {0}, msg = {1}({2}), wParam = {3}, lParam = {4}", hwnd, msg, (WM)msg, wParam, lParam);
            return IntPtr.Zero;
        }

        private void InitializeHook()
        {
            KeyActionAssistant.SelfJobInputId = KeyActionChecker.SelfJobInputId;

            var builder = ApplicationDiContainer.Build<KeyActionFactory>();

            KeyboradHooker.KeyDown += KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp += KeyboradHooker_KeyUp;

            RebuildHook();
        }

        private void RebuildHook()
        {
            var keyActionFactory = ApplicationDiContainer.Build<KeyActionFactory>();
            KeyActionChecker.ReplaceJobs.SetRange(keyActionFactory.CreateReplaceJobs());
            KeyActionChecker.DisableJobs.SetRange(keyActionFactory.CreateDisableJobs());
            KeyActionChecker.PressedJobs.SetRange(keyActionFactory.CreatePressedJobs());


        }

        private void DisposeHook()
        {
            KeyboradHooker.KeyDown -= KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp -= KeyboradHooker_KeyUp;

            KeyboradHooker.Dispose();
            MouseHooker.Dispose();
        }

        void StartHook() {
            if(KeyActionChecker.HasJob) {
                KeyboradHooker.Register();
            }
            //MouseHooker.Register();
        }

        void StopHook() {
            KeyboradHooker.Unregister();
            MouseHooker.Unregister();
        }

        Task ExecuteKeyDownJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, in ModifierKeyStatus modifierKeyStatus)
        {
            var localModifierKeyStatus = modifierKeyStatus;
            return Task.Run(() => {
                foreach(var job in jobs) {
                    switch(job.CommonData.KeyActionKind) {
                        case KeyActionKind.Replace: {
                                var replaceJob = (KeyActionReplaceJob)job;
                                KeyActionAssistant.ExecuteReplaceJob(replaceJob, localModifierKeyStatus);
                            }
                            break;

                        case KeyActionKind.Disable:
                            break;


                        case KeyActionKind.LauncherItem: {
                                var launcherItemJob = (KeyActionLauncherItemJob)job;
                                if(!launcherItemJob.IsAllHit) {
                                    Logger.LogTrace("待機中: {0}", job.CommonData.KeyActionId);
                                    break;
                                }
                                Logger.LogInformation("キーからの起動: アイテム = {0}, キー = {1}", launcherItemJob.PressedData.LauncherItemId, launcherItemJob.CommonData.KeyActionId);

                                NativeMethods.GetCursorPos(out var podPoint);
                                var deviceCursorLocation = PodStructUtility.Convert(podPoint);
                                var screen = Screen.FromDevicePoint(deviceCursorLocation);
                                var element = GetOrCreateLauncherItemElement(launcherItemJob.PressedData.LauncherItemId);
                                switch(launcherItemJob.PressedData.LauncherItemKind) {
                                    case KeyActionContentLauncherItem.Execute:
                                        element.Execute(screen);
                                        break;
                                    case KeyActionContentLauncherItem.ExtendsExecute:
                                        element.OpenExtendsExecuteView(screen);
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            });
        }

        Task ExecuteKeyUpJobsAsync(IReadOnlyCollection<KeyActionJobBase> jobs, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            var localModifierKeyStatus = modifierKeyStatus;
            return Task.Run(() => {
                KeyActionAssistant.CleanupReplaceJob(key, localModifierKeyStatus);

                foreach(var job in jobs) {
                    // 何もやることはないハズ
                }
            });
        }

        #endregion

        private void KeyboradHooker_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, e.modifierKeyStatus, e.kbdll);
            if(0 < jobs.Count) {
                e.Handled = true;
                ExecuteKeyDownJobsAsync(jobs, e.modifierKeyStatus).ConfigureAwait(false);
            }
        }

        private void KeyboradHooker_KeyUp(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(e.IsDown, e.Key, new ModifierKeyStatus(), e.kbdll);
            ExecuteKeyUpJobsAsync(jobs, e.Key, e.modifierKeyStatus).ConfigureAwait(false);
        }


    }
}
