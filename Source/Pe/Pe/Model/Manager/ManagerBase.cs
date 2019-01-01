using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    /// <summary>
    /// 何かしらをずっと管理してる長老。
    /// </summary>
    public class ManagerBase : ContextElementBase
    {
        public ManagerBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
            :base(diContainer, loggerFactory)
        { }

        #region property

        #endregion
    }
}
