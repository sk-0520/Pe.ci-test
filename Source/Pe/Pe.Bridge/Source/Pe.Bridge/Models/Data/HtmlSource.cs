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

    public interface IHtmlSource
    {
        #region property

        /// <summary>
        /// 自身のHTMLソースの種別。
        /// </summary>
        HtmlSourceKind HtmlSourceKind { get; }

        #endregion
    }

    /// <inheritdoc cref="IHtmlSource"/>
    public abstract class HtmlSourceBase: IHtmlSource
    {
        #region IHtmlSource

        /// <inheritdoc cref="IHtmlSource.HtmlSourceKind"/>
        public abstract HtmlSourceKind HtmlSourceKind { get; }

        #endregion
    }

    public interface IHtmlAddress: IHtmlSource
    {
        #region proeprty

        Uri Address { get; }

        #endregion
    }

    /// <inheritdoc cref="IHtmlAddress"/>
    public class HtmlAddress: HtmlSourceBase, IHtmlAddress
    {
        public HtmlAddress(Uri address)
        {
            Address = address;
        }

        #region IHtmlAddress

        /// <inheritdoc cref="IHtmlAddress.Address"/>
        public Uri Address { get; }

        #endregion

        #region HtmlSourceBase

        /// <inheritdoc cref="HtmlSourceBase.HtmlSourceKind"/>
        public override HtmlSourceKind HtmlSourceKind => HtmlSourceKind.Address;

        #endregion
    }

    public interface IHtmlSourceCode: IHtmlSource
    {
        #region property

        string SourceCode { get; }
        Uri? BaseAddress { get; }

        #endregion
    }

    /// <inheritdoc cref="IHtmlSourceCode"/>
    public class HtmlSourceCode: HtmlSourceBase, IHtmlSourceCode
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

        #region IHtmlSourceCode

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
