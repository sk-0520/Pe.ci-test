using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.Extensions.Logging;

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
