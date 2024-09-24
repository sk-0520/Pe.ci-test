using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html.TagElement
{
    public sealed class HtmlHrTagElement: HtmlTagElement
    {
        public HtmlHrTagElement(HtmlDocument document)
            : base("hr", document)
        { }

        #region property

        #endregion

        #region function

        #endregion

        #region HtmlTagElement

        public override bool IsInline => false;
        public override bool IsVoid => true;

        #endregion
    }
}
