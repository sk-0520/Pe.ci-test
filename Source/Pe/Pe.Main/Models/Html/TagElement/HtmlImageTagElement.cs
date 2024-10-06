using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html.TagElement
{
    public sealed class HtmlImageTagElement: HtmlTagElement
    {
        public HtmlImageTagElement(HtmlDocument document)
            : base("img", document)
        { }

        #region property

        #endregion

        #region function

        #endregion

        #region HtmlTagElement

        public override bool IsInline => true;
        public override bool IsVoid => true;

        #endregion
    }
}
