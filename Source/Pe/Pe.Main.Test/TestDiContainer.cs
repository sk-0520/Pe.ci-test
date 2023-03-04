using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Test
{
    class TestDiContainer
    {
        #region function

        public DiContainer CreateDiContainer(ILoggerFactory loggerFactory)
        {
            var diContainer = new ApplicationDiContainer();
            diContainer
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
            ;

            return diContainer;
        }

        #endregion
    }
}
