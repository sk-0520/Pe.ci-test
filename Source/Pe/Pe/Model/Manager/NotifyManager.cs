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
