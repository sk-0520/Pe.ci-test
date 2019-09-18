using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    /// <summary>
    /// アプリケーションからの通知を発行する。
    /// </summary>
    public interface INotifyManager
    {
        #region function
        #endregion
    }

    partial class ApplicationManager
    {
        class NotifyManagerImpl : ManagerBase, INotifyManager
        {
            public NotifyManagerImpl(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region function
            #endregion

            #region INotifyManager
            #endregion
        }
    }
}
