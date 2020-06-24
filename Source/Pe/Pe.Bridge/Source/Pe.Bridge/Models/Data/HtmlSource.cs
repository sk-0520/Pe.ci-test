using System;
using System.Collections.Generic;
using System.Text;

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

        public Uri Address { get; }

        #endregion

        #region HtmlSourceBase

        /// <inheritdoc cref="HtmlSourceBase.HtmlSourceKind"/>
        public override HtmlSourceKind HtmlSourceKind => HtmlSourceKind.Address;

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

        /// <inheritdoc cref="IHtmlSourceCode.SourceCode"/>
        public string SourceCode { get; }
        /// <inheritdoc cref="IHtmlSourceCode.BaseAddress"/>
        public Uri? BaseAddress { get; }

        #endregion

        #region HtmlSourceBase

        /// <inheritdoc cref="HtmlSourceBase.HtmlSourceKind"/>
        public override HtmlSourceKind HtmlSourceKind => HtmlSourceKind.SourceCode;

        #endregion
    }
}
