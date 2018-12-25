using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    public class ApplicationDiContainer : DiContainer
    {
        #region DiContainer
        #endregion
    }

    public static class IDiRegisterContainerExtensions
    {
        #region function

        public static IScopeDiContainer CreateChildContainer(this IDiScopeContainerFactory @this)
        {
            var scopeDiContainer = @this.Scope();
            scopeDiContainer.Register<IDiContainer, IScopeDiContainer>(scopeDiContainer);
            return scopeDiContainer;
        }

        public static IDiRegisterContainer RegisterLogger(this IDiRegisterContainer @this, ILogger parentLogger, [CallerMemberName] string callerMemberName = default(string))
        {
            var logger = parentLogger.Factory.CreateLogger(callerMemberName);

            return @this
                .Register<ILogger, ILogger>(logger)
                .Register<ILoggerFactory, ILoggerFactory>(logger.Factory)
            ;
        }

        public static IDiRegisterContainer RegisterMvvm<TModel, TViewModel, TView>(this IDiRegisterContainer @this)
            where TModel : BindModelBase
            where TViewModel : ViewModelBase
            where TView : FrameworkElement
        {
            return @this
                .Register<TModel, TModel>(DiLifecycle.Singleton)
                .Register<TViewModel, TViewModel>(DiLifecycle.Transient)
                .DirtyRegister<TView, TViewModel>(nameof(FrameworkElement.DataContext))
            ;
        }

        #endregion
    }
}
