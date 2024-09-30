using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlNode: IHtmlNode
    {
        protected HtmlNode(HtmlDocument document)
        {
            Document = document;
        }

        #region property

        public HtmlDocument Document { get; }

        public HtmlNode? Parent { get; internal set; }

        private List<HtmlNode> Nodes { get; } = [];

        #endregion

        #region function

        protected virtual void WriteCore(TextWriter writer, HtmlNodeOutputOptions options, int parentNodeLevel)
        {
            foreach(var child in Children) {
                child.WriteCore(writer, options, parentNodeLevel + 1);
            }
        }

        #endregion

        #region object

        public override string ToString()
        {
            return this.Output();
        }

        #endregion

        #region IHtmlNode

        public IReadOnlyList<HtmlNode> Children => Nodes;

        public TNode AppendChild<TNode>(TNode node)
            where TNode : HtmlNode
        {
            Nodes.Add(node);
            node.Parent = this;
            return node;
        }

        public void Write(TextWriter writer, HtmlNodeOutputOptions options)
        {
            WriteCore(writer, options, 0);
        }

        #endregion
    }
}
