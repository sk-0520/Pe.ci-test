using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Html.TagElement;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlTagFactory
    {
        public HtmlTagFactory(HtmlDocument document)
        {
            Document = document;
        }

        #region property

        protected HtmlDocument Document { get; }

        protected IDictionary<string, Type> TagElementMap { get; } = new Dictionary<string, Type> {
            ["html"] = typeof(HtmlHtmlTagElement),

            ["head"] = typeof(HtmlHeadTagElement),

            ["title"] = typeof(HtmlTitleTagElement),

            ["body"] = typeof(HtmlBodyTagElement),

            ["span"] = typeof(HtmlSpanTagElement),
            ["div"] = typeof(HtmlDivTagElement),

            ["h1"] = typeof(HtmlHeadingElement),
            ["h2"] = typeof(HtmlHeadingElement),
            ["h3"] = typeof(HtmlHeadingElement),
            ["h4"] = typeof(HtmlHeadingElement),
            ["h5"] = typeof(HtmlHeadingElement),
            ["h6"] = typeof(HtmlHeadingElement),

            ["img"] = typeof(HtmlImageTagElement),
            ["hr"] = typeof(HtmlHrTagElement),
            ["br"] = typeof(HtmlBrTagElement),
        };

        #endregion

        #region function

        public virtual HtmlTagElement Create(string tagName)
        {
            if(TagElementMap.TryGetValue(tagName, out var tagElement)) {
                var result = HtmlHeadingElement.HeadingTags.Contains(tagName)
                    ? Activator.CreateInstance(tagElement, tagName, Document)
                    : Activator.CreateInstance(tagElement, Document)
                ;
                if(result is null) {
                    throw new ArgumentException("element is null", nameof(tagName));
                }

                return (HtmlTagElement)result;
            }

            return new HtmlTagElement(tagName, Document);
        }

        public THtmlTagElement New<THtmlTagElement>()
        {
            var result = Activator.CreateInstance(typeof(THtmlTagElement), Document);
            if(result is null) {
                throw new ArgumentException(nameof(THtmlTagElement));
            }

            return (THtmlTagElement)result;
        }

        private HtmlElement CreateTreeCore(string tagName, IReadOnlyDictionary<string, string>? attributes, IEnumerable<HtmlNode> childElements)
        {
            var element = Create(tagName);

            if(attributes is not null) {
                foreach(var pair in attributes) {
                    element.Attributes[pair.Key] = pair.Value;
                }
            }

            foreach(var childElement in childElements) {
                element.AppendChild(childElement);
            }

            return element;
        }

        public HtmlElement CreateTree(string tagName, IReadOnlyDictionary<string, string> attributes, HtmlNode childElement)
        {
            return CreateTreeCore(tagName, attributes, [childElement]);
        }

        public HtmlElement CreateTree(string tagName, IReadOnlyDictionary<string, string> attributes, IEnumerable<HtmlNode> childElements)
        {
            return CreateTreeCore(tagName, attributes, childElements);
        }

        public HtmlElement CreateTree(string tagName, IReadOnlyDictionary<string, string> attributes)
        {
            return CreateTreeCore(tagName, attributes, []);
        }

        public HtmlElement CreateTree(string tagName, HtmlNode childElement)
        {
            return CreateTreeCore(tagName, null, [childElement]);
        }

        public HtmlElement CreateTree(string tagName, IEnumerable<HtmlNode> childElements)
        {
            return CreateTreeCore(tagName, null, childElements);
        }

        public HtmlElement CreateTree(string tagName)
        {
            return CreateTreeCore(tagName, null, []);
        }

        #endregion
    }
}
