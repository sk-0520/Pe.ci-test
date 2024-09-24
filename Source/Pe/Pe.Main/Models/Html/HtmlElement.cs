using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlElement: HtmlNode
    {
        public HtmlElement(string tagName, HtmlDocument document)
            : base(document)
        {
            TagName = tagName;
        }

        #region property

        public string TagName { get; }

        /// <summary>
        /// インライン要素か。
        /// </summary>
        public virtual bool IsInline { get; set; } = false;
        /// <summary>
        /// 空要素か。
        /// </summary>
        public virtual bool IsVoid { get; set; } = false;

        public IDictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        #endregion

        #region function

        public void Append<TNode>(TNode node1, params TNode[] nextNodes)
            where TNode : HtmlNode
        {
            var nodes = new List<TNode>(1 + nextNodes.Length) {
                node1
            };
            nodes.AddRange(nextNodes);

            foreach(var node in nodes) {
                AppendChild(node);
            }
        }

        #endregion

        #region HtmlNode

        protected override void WriteCore(TextWriter writer, HtmlNodeOutputOptions options, int parentNodeLevel)
        {
            writer.Write('<');
            writer.Write(TagName);

            if(0 < Attributes.Count) {
                foreach(var pair in Attributes) {
                    writer.Write(' ');
                    writer.Write(pair.Key);
                    if(!string.IsNullOrEmpty(pair.Value)) {
                        var value = Document.HtmlCharacters.Encode(pair.Value);
                        writer.Write("=\"");
                        writer.Write(value);
                        writer.Write("\"");
                    }
                }
            }

            if(IsVoid) {
                writer.Write('>');
            } else {
                writer.Write('>');
                if(!IsInline && !options.Optimization) {
                    writer.WriteLine();
                }

                base.WriteCore(writer, options, parentNodeLevel);

                writer.Write("</");
                writer.Write(TagName);
                writer.Write('>');
            }

            if(!IsInline && !options.Optimization) {
                writer.WriteLine();
            }
        }

        #endregion



    }
}
