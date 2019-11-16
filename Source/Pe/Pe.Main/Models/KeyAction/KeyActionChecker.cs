using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
    ///             <para>置き換えに該当した場合は後続の該当チェックは行わない</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             入力無効化
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
    /// 入力無効化は独立して存在してるが、何がどうあれ該当があった場合は無効化される。
    /// </remarks>
    public class KeyActionChecker
    {
        public KeyActionChecker(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        /// <summary>
        /// キー無効化処理にてこの時間内の入力に限り有効化する。
        /// </summary>
        public TimeSpan KeyDisableToEnableTime { get; set; } = SystemInformation.DoubleClickTime;
        /// <summary>
        /// 自身の入力を弾くかの設定。
        /// <para>呼び出し側で制御。。。</para>
        /// </summary>
        public bool IgnoreSelfJobInput { get; set; } = false;
        /// <summary>
        /// 置き換えに使用する入力に埋め込むIDの設定。
        /// <para>0以外の場合 <see cref="KBDLLHOOKSTRUCT.dwExtraInfo"/> を確認して同じなら弾くようにする。</para>
        /// <para>呼び出し側で制御。。。</para>
        /// </summary>
        public int SelfJobInputId { get; set; } = 1234;

        public IList<KeyActionDisableJob> DisableJobs { get; } = new List<KeyActionDisableJob>();
        public IList<KeyActionReplaceJob> ReplaceJobs { get; } = new List<KeyActionReplaceJob>();
        public IList<KeyActionPressedJobBase<IReadOnlyKeyActionPressedData>> PressedJobs { get; } = new List<KeyActionPressedJobBase<IReadOnlyKeyActionPressedData>>();
        #endregion

        #region function

        public IReadOnlyCollection<KeyActionJobBase> Find(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            // 置き換え
            foreach(var job in ReplaceJobs) {
                if(job.Check(isDown, key, modifierKeyStatus)) {
                    return new[] { job };
                }
            }

            var result = new List<KeyActionJobBase>();
            var now = DateTime.UtcNow;

            // 無効化
            foreach(var job in DisableJobs) {
                if(KeyDisableToEnableTime < now - job.LastCheckTimestamp) {
                    if(job.Check(isDown, key, modifierKeyStatus)) {
                        // 一つでも無効化になれば後は不要(効果が一緒のため)
                        result.Add(job);
                        break;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
