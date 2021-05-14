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
        #region IReadOnlyFontData

        /// <summary>
        /// フォント名。
        /// </summary>
        [DataMember]
        public string FamilyName { get; set; } = string.Empty;
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        [DataMember]
        public double Size { get; set; }
        /// <summary>
        /// フォントを太字にするか。
        /// </summary>
        [DataMember]
        public bool IsBold { get; set; }
        /// <summary>
        /// フォントを斜体にするか。
        /// </summary>
        [DataMember]
        public bool IsItalic { get; set; }

        /// <summary>
        /// フォントに下線を設定するか。
        /// </summary>
        [DataMember]
        public bool IsUnderline { get; set; }
        /// <summary>
        /// フォントに取り消し線を設定するか。
        /// </summary>
        [DataMember]
        public bool IsStrikeThrough { get; set; }

        #endregion
    }

}
