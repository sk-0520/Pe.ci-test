using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
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
    /// </remarks>
    public class KeyActionChecker
    {
        public KeyActionChecker(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        public IList<KeyActionDisableJob> DisableJobs { get; } = new List<KeyActionDisableJob>();
        public IList<KeyActionReplaceJob> ReplaceJobs { get; } = new List<KeyActionReplaceJob>();

        #endregion

        #region function

        public IReadOnlyCollection<KeyActionJobBase> Find(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(!isDown) {
                return new List<KeyActionJobBase>();
            }

            var result = new List<KeyActionJobBase>();
            foreach(var job in DisableJobs) {
                if(job.Check(isDown, key, modifierKeyStatus)) {
                    result.Add(job);
                }
            }

            return result;
        }

        #endregion
    }
}
