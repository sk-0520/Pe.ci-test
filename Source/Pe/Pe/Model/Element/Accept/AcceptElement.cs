using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Accept
{
    public class AcceptElement : ElementBase
    {
        public AcceptElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        public bool Accepted { get; set; }

        #endregion

        #region function
        #endregion
    }
}
