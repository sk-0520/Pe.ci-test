using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    /// <summary>
    /// キー入力から実行可能な処理を判定する。
    /// </summary>
    /// <remarks>
    /// 優先順位は以下とする。
    /// <list type="number">
    ///     <item>
    ///         <description>
    ///             入力入れ替え
    ///             <para>後続の該当チェックは行わない</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             入力無効化
    ///             <para>後続の該当チェックは行わない</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             コマンド実行
    ///             <para>修飾キー単体での実行は不可</para>
    ///             <para>後続チェック処理はリセット</para>
    ///         </description>
    ///     </item>
    /// </list>
    /// 入力無効化は独立して存在してるが、コマンド実行がシステム通知しないかぎり該当があった場合は無効化される。
    /// </remarks>
    public class KeyActionChecker
    {
        #region variable

        private uint _selfJobInputId = 1234;

        #endregion
        public KeyActionChecker(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        /// <summary>
        /// キー無効化処理にてこの時間内の入力に限り有効化する。
        /// </summary>
        public TimeSpan KeyDisableToEnableTime { get; set; } = SystemInformation.DoubleClickTime;
        /// <summary>
        /// 自身の入力を弾くかの設定。
        /// </summary>
        /// <remarks>
        /// <para>呼び出し側で制御。。。</para>
        /// </remarks>
        public bool IgnoreSelfJobInput { get; set; } = false;
        /// <summary>
        /// 置き換えに使用する入力に埋め込むIDの設定。
        /// </summary>
        /// <remarks>
        /// <para>0以外の場合 <see cref="KBDLLHOOKSTRUCT.dwExtraInfo"/> を確認して同じなら弾くようにする。</para>
        /// <para>呼び出し側で制御。。。</para>
        /// </remarks>
        public uint SelfJobInputId
        {
            get => this._selfJobInputId;
            set
            {
                if(value == 0) {
                    throw new ArgumentException("error: 0");
                }
                this._selfJobInputId = value;

            }
        }

        public bool HasJob => DisableJobs.Any() || ReplaceJobs.Any() || PressedJobs.Any();

        public IList<KeyActionDisableJob> DisableJobs { get; } = new List<KeyActionDisableJob>();
        public IList<KeyActionReplaceJob> ReplaceJobs { get; } = new List<KeyActionReplaceJob>();
        public IList<KeyActionPressedJobBase> PressedJobs { get; } = new List<KeyActionPressedJobBase>();

        #endregion

        #region function

        public IReadOnlyCollection<KeyActionJobBase> Find(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus, in KBDLLHOOKSTRUCT kbdll)
        {
            if(IgnoreSelfJobInput && kbdll.dwExtraInfo != UIntPtr.Zero) {
                var extraInfo = kbdll.dwExtraInfo.ToUInt32();
                if(extraInfo == SelfJobInputId) {
                    Logger.LogTrace("ignore input");
                    return Array.Empty<KeyActionJobBase>();
                }
            }
            Logger.LogTrace("{0}, {1}", key, modifierKeyStatus);

            // 置き換え
            foreach(var job in ReplaceJobs) {
                if(job.Check(isDown, key, modifierKeyStatus)) {
                    return new[] { job };
                }
            }

            var now = DateTime.UtcNow;

            // 無効化
            foreach(var job in DisableJobs) {
                if(job.ActionData.Forever) {
                    if(job.Check(isDown, key, modifierKeyStatus)) {
                        // 完全無視
                        return new[] { job };
                    }
                }

                if(KeyDisableToEnableTime < now - job.LastCheckTimestamp) {
                    if(job.Check(isDown, key, modifierKeyStatus)) {
                        // 一つでも無効化になれば後は不要(効果が一緒のため)
                        return new[] { job };
                    }
                }
            }

            var result = new List<KeyActionPressedJobBase>();

            // キー入力処理
            foreach(var job in PressedJobs) {
                if(job.Check(isDown, key, modifierKeyStatus)) {
                    // なんであれ後続の入力止めるためジョブを返す(IsAllHit で実行を制御)
                    result.Add(job);
                }
            }

            return result;
        }

        public void Reset()
        {
            foreach(var job in PressedJobs) {
                job.Reset();
            }
        }

        #endregion
    }
}
