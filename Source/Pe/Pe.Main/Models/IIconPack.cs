using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models
{
    /// <summary>
    /// アイコンのまとまり。
    /// </summary>
    /// <typeparam name="TIcon"></typeparam>
    public interface IIconPack<TIcon>
    {
        #region property

        /// <inheritdoc cref="IconBox.Small"/>
        TIcon Small { get; }
        /// <inheritdoc cref="IconBox.Normal"/>
        TIcon Normal { get; }
        /// <inheritdoc cref="IconBox.Big"/>
        TIcon Big { get; }
        /// <inheritdoc cref="IconBox.Large"/>
        TIcon Large { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="IconBox"/> のマッピング。
        /// </summary>
        IReadOnlyDictionary<IconBox, TIcon> IconItems { get; }

        #endregion
    }
}
