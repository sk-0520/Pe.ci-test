using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 修飾キー。
    /// </summary>
    public enum ModifierKey
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// いずれか。
        /// </summary>
        Any,
        /// <summary>
        /// 左。
        /// </summary>
        Left,
        /// <summary>
        /// 右。
        /// </summary>
        Right,
        /// <summary>
        /// 両方。
        /// </summary>
        All,
    }

    public interface IReadOnlyKeyMappingItemData
    {
        #region property

        Key Key { get; }
        ModifierKey Shift { get; }
        ModifierKey Control { get; }
        ModifierKey Alt { get; }
        ModifierKey Super { get; }

        #endregion
    }

    public class KeyMappingItemData : IReadOnlyKeyMappingItemData
    {
        #region IReadOnlyKeyMappingData

        public Key Key { get; set; } = Key.None;
        public ModifierKey Shift { get; set; } = ModifierKey.None;
        public ModifierKey Control { get; set; } = ModifierKey.None;
        public ModifierKey Alt { get; set; } = ModifierKey.None;
        public ModifierKey Super { get; set; } = ModifierKey.None;

        #endregion
    }


}
