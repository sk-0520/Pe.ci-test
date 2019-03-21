using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic.Theme
{
    public class PainterBase
    {
        public PainterBase(IDispatcherWapper dispatcherWapper, ILogger logger)
        {
            Logger = logger;
        }
        public PainterBase(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected IDispatcherWapper DispatcherWapper { get; }
        protected ILogger Logger { get; }

        #endregion
    }
}
