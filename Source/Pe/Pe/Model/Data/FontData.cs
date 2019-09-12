using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Model.Data
{
    public interface IReadOnlyFontData
    {
        #region property

        /// <summary>
        /// フォント名。
        /// </summary>
        string? FamilyName { get; }
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        double Size { get; }
        /// <summary>
        /// フォントを太字にするか。
        /// </summary>
        bool IsBold { get; }
        /// <summary>
        /// フォントを斜体にするか。
        /// </summary>
        bool IsItalic { get; }
        bool IsUnderline { get; }
        bool IsStrikeThrough { get; }

        #endregion
    }

    [Serializable, DataContract]
    public class FontData : IReadOnlyFontData
    {
        #region IReadOnlyFontData

        /// <summary>
        /// フォント名。
        /// </summary>
        [DataMember]
        public string? FamilyName { get; set; }
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

        [DataMember]
        public bool IsUnderline { get; set; }
        [DataMember]
        public bool IsStrikeThrough { get; set; }

        #endregion
    }
}
