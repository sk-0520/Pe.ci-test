using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum DefaultFontKind
    {
        Note,
        LauncherToolbar,
        Command,
    }


    [Serializable, DataContract]
    public class FontData: IFont
    {
        #region IFont

        /// <inheritdoc cref="IFont.FamilyName"/>
        [DataMember]
        public string FamilyName { get; set; } = string.Empty;
        /// <inheritdoc cref="IFont.Size"/>
        [DataMember]
        public double Size { get; set; }
        /// <inheritdoc cref="IFont.IsBold"/>
        [DataMember]
        public bool IsBold { get; set; }
        /// <inheritdoc cref="IFont.IsItalic"/>
        [DataMember]
        public bool IsItalic { get; set; }
        /// <inheritdoc cref="IFont.IsUnderline"/>
        [DataMember]
        public bool IsUnderline { get; set; }
        /// <inheritdoc cref="IFont.IsStrikeThrough"/>
        [DataMember]
        public bool IsStrikeThrough { get; set; }

        #endregion
    }
}
