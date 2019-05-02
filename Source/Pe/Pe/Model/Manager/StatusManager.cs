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
    /// アプリケーションの状態管理。
    /// </summary>
    public interface IStatusManager
    {
        #region property
        #endregion

        #region function
        #endregion
    }

    public class StatusManager : ManagerBase, IStatusManager
    {
        public StatusManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region function
        #endregion

        #region IStatusManager
        #endregion

    }
}
