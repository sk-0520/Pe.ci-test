using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public class KeyActionAssistant
    {
        public KeyActionAssistant(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        /// <summary>
        /// <see cref="KeyActionChecker.SelfJobInputId"/>
        /// </summary>
        public uint SelfJobInputId { get; set; }

        KeyActionReplaceJob? LastExecuteReplaceJob { get; set; }
        ModifierKeyStatus ReplacedModifierKeyStatus { get; set; }
        #endregion

        #region function

        INPUT CreateKeyboardInputData(Key key, bool isDown, bool isExtended = true)
        {
            var input = new INPUT() {
                type = INPUT_type.INPUT_KEYBOARD,
            };
            input.data.ki.wVk = (ushort)KeyInterop.VirtualKeyFromKey(key);
            input.data.ki.wScan = (ushort)NativeMethods.MapVirtualKey(input.data.ki.wVk, MAPVK.MAPVK_VK_TO_VSC);
            input.data.ki.dwFlags = (isExtended ? KEYEVENTF.KEYEVENTF_EXTENDEDKEY : 0) | (isDown ? KEYEVENTF.KEYEVENTF_KEYDOWN : KEYEVENTF.KEYEVENTF_KEYUP);
            input.data.ki.dwExtraInfo = new UIntPtr(SelfJobInputId);
            input.data.ki.time = 0;

            return input;
        }

        void SendInput(IReadOnlyList<INPUT> inputs)
        {
            Debug.Assert(0 < inputs.Count);

            var inputArray = inputs.ToArray();

            NativeMethods.SendInput((uint)inputArray.Length, inputArray, Marshal.SizeOf(inputs[0]));
        }

        /// <summary>
        /// キー置き換え処理を実行。
        /// </summary>
        /// <param name="job"></param>
        public void ExecuteReplaceJob(KeyActionReplaceJob job, in ModifierKeyStatus modifierKeyStatus)
        {
            // 修飾キーの置き換え処理であれば戻し処理用に確保しておく
            if(job.ActionData.ReplaceKey.IsModifierKey()) {
                LastExecuteReplaceJob = job;
                ReplacedModifierKeyStatus = modifierKeyStatus;
            }

            var inputs = new List<INPUT>();

            var input = CreateKeyboardInputData(job.ActionData.ReplaceKey, true);

            inputs.Add(input);

            SendInput(inputs);
        }

        /// <summary>
        /// キー置き換え処理後の補正。
        /// </summary>
        public void CleanupReplaceJob(Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(LastExecuteReplaceJob == null) {
                Logger.LogTrace("clean up job: none");
                return;
            }

            if(LastExecuteReplaceJob.Mapping.Key != key) {
                Logger.LogTrace("unhit job");
                return;
            }

            Logger.LogDebug("cleanup: {0} -> {1}", key, LastExecuteReplaceJob.ActionData.ReplaceKey);

            var inputs = new List<INPUT>();

            var input = CreateKeyboardInputData(LastExecuteReplaceJob.ActionData.ReplaceKey, false);
            inputs.Add(input);

            SendInput(inputs);

            LastExecuteReplaceJob = null;
        }

        #endregion
    }
}
