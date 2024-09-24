using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public interface IHtmlNode
    {
        #region function

        IReadOnlyList<HtmlNode> Children { get; }

        TNode AppendChild<TNode>(TNode node)
            where TNode : HtmlNode
        ;

        void Write(TextWriter writer, HtmlNodeOutputOptions options);

        #endregion
    }

    public static class IHtmlNodeExtensions
    {
        #region function

        public static string Output(this IHtmlNode node) => Output(node, new HtmlNodeOutputOptions() {
            Indent = "    ",
            Optimization = false,
        });

        public static string Output(this IHtmlNode node, HtmlNodeOutputOptions options) {
            var writer = new StringWriter();
            node.Write(writer, options);
            return writer.ToString();
        }

        #endregion
    }
}
