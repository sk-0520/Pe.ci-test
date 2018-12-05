using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.ViewElement.Accept
{
    public class AcceptViewElement : ViewElementBase
    {
        public AcceptViewElement(ILogger logger)
            : base(logger)
        { }
        [Injection]
        public AcceptViewElement(ILogFactory logFactory)
            : base(logFactory.CreateCurrentClass())
        { }

        #region property

        public bool Accepted { get; set; }

        #endregion

        #region function
        #endregion
    }
}
