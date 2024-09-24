using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Html.TagElement;
using ContentTypeTextNet.Pe.Standard.Base;

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
        };

        #endregion

        #region function

        public virtual HtmlTagElement Create(string tagName)
        {
            if(TagElementMap.TryGetValue(tagName, out var tagElement)) {
                var result = HtmlHeadingElement.HeadingTags.Contains(tagName)
                    ? Activator.CreateInstance(tagElement, Document, tagName)
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


        #endregion
    }
}
