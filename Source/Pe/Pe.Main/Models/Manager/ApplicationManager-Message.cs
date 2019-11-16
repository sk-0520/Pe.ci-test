using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
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

            KeyboradHooker.KeyDown += KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp += KeyboradHooker_KeyUp;
        }

        private void DisposeHook()
        {
            KeyboradHooker.KeyDown -= KeyboradHooker_KeyDown;
            KeyboradHooker.KeyUp -= KeyboradHooker_KeyUp;

            KeyboradHooker.Dispose();
            MouseHooker.Dispose();
        }

        void StartHook() {
            //KeyboradHooker.Register();
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
                    if(job.CommonData.KeyActionKind == KeyActionKind.Replace) {
                        var replaceJob = (KeyActionReplaceJob)job;
                        KeyActionAssistant.ExecuteReplaceJob(replaceJob, localModifierKeyStatus);
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
            var jobs = KeyActionChecker.Find(true, e.Key, e.modifierKeyStatus, e.kbdll);
            if(0 < jobs.Count) {
                e.Handled = true;
                ExecuteKeyDownJobsAsync(jobs, e.modifierKeyStatus).ConfigureAwait(false);
            }
        }

        private void KeyboradHooker_KeyUp(object? sender, KeyboardHookEventArgs e)
        {
            var jobs = KeyActionChecker.Find(true, e.Key, new ModifierKeyStatus(), e.kbdll);
            ExecuteKeyUpJobsAsync(jobs, e.Key, e.modifierKeyStatus).ConfigureAwait(false);
        }


    }
}
