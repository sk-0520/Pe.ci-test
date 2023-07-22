using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

        private ILogger Logger { get; }
        /// <summary>
        /// <see cref="KeyActionChecker.SelfJobInputId"/>
        /// </summary>
        public uint SelfJobInputId { get; set; }

        private KeyActionReplaceJob? LastExecuteReplaceJob { get; set; }
        private ModifierKeyStatus ReplacedModifierKeyStatus { get; set; }

        #endregion

        #region function

        private INPUT CreateKeyboardInputData(Key key, bool isDown, bool isExtended = true)
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

        private void SendInput(IReadOnlyList<INPUT> inputs)
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
#if DEBUG
                Logger.LogTrace("補正対象のジョブ: {0}, {1}", job.ActionData.ReplaceKey, job.ActionData.KeyActionId);
#endif
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
#if DEBUG
                Logger.LogTrace("補正すべきジョブなし");
#endif
                return;
            }

            if(LastExecuteReplaceJob.Mapping.Key != key) {
#if DEBUG
                Logger.LogTrace("補正すべきジョブの対象ではない: 入力 = {0}, 補正対象 = {1}, {2}", key, LastExecuteReplaceJob.Mapping.Key, LastExecuteReplaceJob.ActionData.KeyActionId);
#endif
                return;
            }

#if DEBUG
            Logger.LogDebug("補正処理: 入力 = {0} -> 補正対象 = {1}, {2}", key, LastExecuteReplaceJob.ActionData.ReplaceKey, LastExecuteReplaceJob.ActionData.KeyActionId);
#endif

            var inputs = new List<INPUT>();

            var input = CreateKeyboardInputData(LastExecuteReplaceJob.ActionData.ReplaceKey, false);
            inputs.Add(input);

            SendInput(inputs);

#if DEBUG
            Logger.LogDebug("補正処理完了: {0}", LastExecuteReplaceJob.ActionData.KeyActionId);
#endif
            LastExecuteReplaceJob = null;
        }

        #endregion
    }
}
