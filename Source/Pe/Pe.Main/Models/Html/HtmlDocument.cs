using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Main.Models.Html.TagElement;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlDocument: IHtmlNode
    {
        public HtmlDocument(Func<HtmlDocument, HtmlTagFactory>? htmlTagFactoryCreator = null)
        {
            Factory = htmlTagFactoryCreator is not null ? htmlTagFactoryCreator(this) : new HtmlTagFactory(this);
            Characters = new HtmlCharacters();

            Head = Factory.New<HtmlHeadTagElement>();
            Body = Factory.New<HtmlBodyTagElement>();
            Root = Factory.New<HtmlHtmlTagElement>();

            Root.AppendChild(Head);
            Root.AppendChild(Body);
        }

        #region property

        public HtmlVersion Version { get; set; } = HtmlVersion.Html5;

        public HtmlTagFactory Factory { get; }
        public HtmlCharacters Characters { get; }


        public HtmlHtmlTagElement Root { get; }
        public HtmlHeadTagElement Head { get; }
        public HtmlBodyTagElement Body { get; }

        #endregion

        #region function

        public HtmlTextNode CreateTextNode(string text)
        {
            var node = new HtmlTextNode(this) {
                TextContent = text,
            };

            return node;
        }

        public HtmlElement CreateElement(string tagName)
        {
            return Factory.Create(tagName);
        }

        public THtmlTagElement CreateElement<THtmlTagElement>()
        {
            return Factory.New<THtmlTagElement>();
        }

        #endregion

        #region object

        public override string ToString()
        {
            return this.Output();
        }

        #endregion

        #region IHtmlNode

        public IReadOnlyList<HtmlNode> Children => Root.Children;

        public TNode AppendChild<TNode>(TNode node)
            where TNode : HtmlNode
        {
            return Root.AppendChild(node);
        }

        public void Write(TextWriter writer, HtmlNodeOutputOptions options)
        {
            switch(Version) {
                case HtmlVersion.Html5:
                    writer.Write("<!doctype html>");
                    if(!options.Optimization) {
                        writer.WriteLine();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            Root.Write(writer, options);
        }

        #endregion
    }
}
