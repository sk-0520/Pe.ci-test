using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    /// <summary>
    /// 何かしらをずっと管理してる長老。
    /// </summary>
    public abstract class ManagerBase: DisposerBase
    {
        protected ManagerBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
        {
            DiContainer = diContainer;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDiContainer DiContainer { get; }
        protected ILogger Logger { get; }

        #endregion
    }
}
