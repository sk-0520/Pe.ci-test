using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public interface IReadOnlyFontData
    {
        #region property

        /// <summary>
        /// フォント名。
        /// </summary>
        string FamilyName { get; }
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
        /// <summary>
        /// フォントに下線を設定するか。
        /// </summary>
        bool IsUnderline { get; }
        /// <summary>
        /// フォントに取り消し線を設定するか。
        /// </summary>
        bool IsStrikeThrough { get; }

        #endregion
    }
}
