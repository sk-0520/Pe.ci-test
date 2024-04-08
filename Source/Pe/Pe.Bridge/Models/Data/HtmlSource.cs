using System;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// HTMLソース種別。
    /// </summary>
    public enum HtmlSourceKind
    {
        /// <summary>
        /// URI を HTML ソースとして扱う。
        /// </summary>
        Address,
        /// <summary>
        /// ソースコードを HTML ソースとして扱う。
        /// </summary>
        SourceCode,
    }

    /// <summary>
    /// HTML 出生地。
    /// </summary>
    public abstract class HtmlSourceBase
    {
        #region IHtmlSource

        /// <summary>
        /// HTMLソース種別。
        /// </summary>
        public abstract HtmlSourceKind HtmlSourceKind { get; }

        #endregion
    }

    /// <summary>
    /// HTML は URI から生まれる。
    /// </summary>
    public class HtmlAddress: HtmlSourceBase
    {
        public HtmlAddress(Uri address)
        {
            Address = address;
        }

        #region property

        /// <summary>
        /// URI。
        /// </summary>
        public Uri Address { get; }

        #endregion

        #region HtmlSourceBase

        /// <inheritdoc cref="HtmlSourceBase.HtmlSourceKind"/>
        public sealed override HtmlSourceKind HtmlSourceKind => HtmlSourceKind.Address;

        #endregion
    }

    /// <summary>
    /// HTML はコードから生まれる。
    /// </summary>
    public class HtmlSourceCode: HtmlSourceBase
    {
        public HtmlSourceCode(string sourceCode)
        {
            SourceCode = sourceCode;
        }

        public HtmlSourceCode(string sourceCode, Uri baseAddress)
        {
            SourceCode = sourceCode;
            BaseAddress = baseAddress;
        }

        #region property

        /// <summary>
        /// HTMLソース。
        /// </summary>
        public string SourceCode { get; }
        /// <summary>
        /// HTMLの元URI。
        /// </summary>
        /// <remarks>
        /// <para>厳密に<see cref="SourceCode"/>と紐付くわけではない(HTML内のbaseとは別物)。たぶんまぁ元ファイルパスとかそんな感じ。</para>
        /// </remarks>
        public Uri? BaseAddress { get; }

        #endregion

        #region HtmlSourceBase

        /// <inheritdoc cref="HtmlSourceBase.HtmlSourceKind"/>
        public sealed override HtmlSourceKind HtmlSourceKind => HtmlSourceKind.SourceCode;

        #endregion
    }
}
