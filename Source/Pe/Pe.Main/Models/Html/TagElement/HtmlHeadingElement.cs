using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html.TagElement
{
    public class HtmlHeadingElement: HtmlTagElement
    {
        public HtmlHeadingElement(string headlineTagName, HtmlDocument document)
            : base(headlineTagName, document)
        {
            if(!HeadingTags.Contains(headlineTagName)) {
                throw new ArgumentException(nameof(headlineTagName));
            }
        }

        #region property

        public static IReadOnlySet<string> HeadingTags { get; } = new HashSet<string>(["h1", "h2", "h3", "h4", "h5", "h6"]);

        #endregion

        #region function

        #endregion

        #region HtmlTagElement

        public override bool IsInline => false;
        public override bool IsVoid => false;

        #endregion
    }
}
