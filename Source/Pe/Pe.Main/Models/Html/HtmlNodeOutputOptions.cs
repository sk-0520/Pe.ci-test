using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html
{
    public class HtmlNodeOutputOptions
    {
        #region property

        public bool Optimization { get; init; } = false;
        public string Indent { get; init; } = "\t";

        #endregion
    }
}
