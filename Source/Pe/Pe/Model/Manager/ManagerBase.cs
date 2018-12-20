using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    /// <summary>
    /// 何かしらをずっと管理してる長老。
    /// </summary>
    public class ManagerBase : DisposerBase
    {
        public ManagerBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
        {
            DiContainer = diContainer;
            Logger = loggerFactory.CreateLogger(GetType().Name);
        }

        #region property

        protected IDiContainer DiContainer { get; }

        protected ILogger Logger { get; }

        #endregion
    }
}
