using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlCharacters
    {
        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "<保留中>")]
        public string Encode(string html)
        {
            var result = new StringBuilder();

            foreach(char c in html) {
                switch(c) {
                    case '<':
                        result.Append("&lt;");
                        break;

                    case '>':
                        result.Append("&gt;");
                        break;

                    case '&':
                        result.Append("&amp;");
                        break;

                    case '\'':
                        result.Append("&apos;");
                        break;

                    case '"':
                        result.Append("&quot;");
                        break;

                    case ' ':
                        result.Append("&nbsp;");
                        break;

                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
