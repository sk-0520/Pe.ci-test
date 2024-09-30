using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlTextNode: HtmlNode, ITextContent
    {
        public HtmlTextNode(HtmlDocument document)
            : base(document)
        {
        }

        #region property

        #endregion

        #region HtmlNode

        protected override void WriteCore(TextWriter writer, HtmlNodeOutputOptions options, int parentNodeLevel)
        {
            var text = Document.Characters.Encode(TextContent);
            writer.Write(text);
        }

        #endregion


        #region ITextContent

        public string TextContent { get; set; } = string.Empty;

        #endregion
    }
}
