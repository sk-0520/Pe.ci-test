using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    /// <summary>
    /// HTML に対して${ から } までの文字列を置き換える。
    /// </summary>
    /// <para>&lt;!--${}--&gt;, /*${}*/ で記述されている場合、コメント部分は破棄される。ただしコメント自体が正しいものかどうかまでは把握していない(script内にHTMLコメントとかあっても気にしない)</para>
    /// <para>${ } の中に : が存在する場合、以降はオプション扱いとなる(${ABC:OPTION} の場合、 ABC がキー、 OPTION がオプション値)。</para>
    public abstract class WebViewTemplateBase
    {
        #region function

        public abstract string Build(string option);

        #endregion
    }

    public sealed class CultureWebViewTemplate : WebViewTemplateBase
    {
        public CultureWebViewTemplate(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public CultureWebViewTemplate()
            : this(CultureInfo.CurrentUICulture)
        { }


        #region property

        CultureInfo CultureInfo { get; }

        #endregion

        #region RawTextWebViewTemplate

        public override string Build(string option) => CultureInfo.TwoLetterISOLanguageName;

        #endregion
    }

    public class RawTextWebViewTemplate : WebViewTemplateBase
    {
        public RawTextWebViewTemplate(string text)
        {
            Text = text;
        }

        #region property

        protected string Text { get; }

        #endregion

        #region WebViewTemplateBase

        public override string Build(string option) => Text;

        #endregion
    }

    public class HtmlTextWebViewTemplate : RawTextWebViewTemplate
    {
        public HtmlTextWebViewTemplate(string text)
            : base(text)
        { }

        #region RawTextWebViewTemplate

        public override string Build(string option) => HttpUtility.HtmlEncode(Text);

        #endregion
    }

    public class CustomWebViewTemplate : WebViewTemplateBase
    {
        public CustomWebViewTemplate(Func<string, string> buildFunction)
        {
            BuildFunction = buildFunction;
        }

        #region property

        Func<string, string> BuildFunction { get; }

        #endregion

        #region WebViewTemplateBase

        public override string Build(string option) => BuildFunction(option);

        #endregion
    }
}
