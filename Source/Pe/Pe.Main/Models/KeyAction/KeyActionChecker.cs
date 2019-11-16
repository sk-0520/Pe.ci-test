using System;
using System.Collections.Generic;
using System.Text;

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
        public KeyActionChecker()
        {

        }

        #region property
        #endregion

        #region function


        #endregion
    }
}
