using System;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    /// <summary>
    /// HTML に対して${ から } までの文字列を置き換える。
    /// </summary>
    /// <remarks>
    /// <para>&lt;!--${}--&gt;, /*${}*/ で記述されている場合、コメント部分は破棄される。ただしコメント自体が正しいものかどうかまでは把握していない(script内にHTMLコメントとかあっても気にしない)</para>
    /// <para>${ } の中に : が存在する場合、以降はオプション扱いとなる(${ABC:OPTION} の場合、 ABC がキー、 OPTION がオプション値)。</para>
    /// </remarks>
    public abstract class WebViewTemplateBase
    {
        #region function

        public abstract string Build(string option);

        #endregion
    }

    public sealed class CultureWebViewTemplate: WebViewTemplateBase
    {
        public CultureWebViewTemplate(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public CultureWebViewTemplate()
            : this(CultureInfo.CurrentUICulture)
        { }

        #region property

        private CultureInfo CultureInfo { get; }

        #endregion

        #region RawTextWebViewTemplate

        public override string Build(string option) => CultureInfo.TwoLetterISOLanguageName;

        #endregion
    }

    public class RawTextWebViewTemplate: WebViewTemplateBase
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

    public class HtmlTextWebViewTemplate: RawTextWebViewTemplate
    {
        public HtmlTextWebViewTemplate(string text)
            : base(text)
        { }

        #region RawTextWebViewTemplate

        public override string Build(string option) => HttpUtility.HtmlEncode(Text);

        #endregion
    }

    public class JavaScriptTextViewTemplate: RawTextWebViewTemplate
    {
        public JavaScriptTextViewTemplate(string text)
            : base(text)
        { }

        #region RawTextWebViewTemplate

        /// <summary>
        ///
        /// </summary>
        /// <param name="option">CSV形式, S = 文字列のくくりに ' が使用されている</param>
        /// <returns></returns>
        public override string Build(string option)
        {
            // あまあま

            var options = option.Split(',');
            if(options.Any(i => i == "S")) {
                return Text
                    .Replace("'", @"\'")
                    .Replace("\r", @"\r")
                    .Replace("\n", @"\n")
                    .Replace("\\", @"\\")
                ;
            }

            return Text
                .Replace("\"", "\\\"")
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\\", @"\\")
            ;
        }

        #endregion
    }

    public class CustomWebViewTemplate: WebViewTemplateBase
    {
        public CustomWebViewTemplate(Func<string, string> buildFunction)
        {
            BuildFunction = buildFunction;
        }

        #region property

        private Func<string, string> BuildFunction { get; }

        #endregion

        #region WebViewTemplateBase

        public override string Build(string option) => BuildFunction(option);

        #endregion
    }
}
