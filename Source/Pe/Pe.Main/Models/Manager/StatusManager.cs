using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
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
