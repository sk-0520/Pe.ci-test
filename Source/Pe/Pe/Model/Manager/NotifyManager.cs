using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public interface INotifyManager
    {

    }

    public class NotifyManager : ManagerBase, INotifyManager
    {
        public NotifyManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region function
        #endregion

        #region INotifyManager
        #endregion
    }
}
