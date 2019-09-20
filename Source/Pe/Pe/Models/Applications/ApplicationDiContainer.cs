using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
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

        //public static IDiRegisterContainer RegisterLogger(this IDiRegisterContainer @this, ILoggerFactory logerFactory, [CallerMemberName] string callerMemberName = "")
        //{
        //    var logger = logerFactory.CreateLogger(callerMemberName);

        //    return @this
        //        .Register<ILogger, ILogger>(logger)
        //        .Register<ILoggerFactory, ILoggerFactory>(logerFactory)
        //    ;
        //}

        public static IDiRegisterContainer RegisterMvvm<TModel, TViewModel, TView>(this IDiRegisterContainer @this)
            where TModel : BindModelBase
            where TViewModel : ViewModelBase
            where TView : FrameworkElement
        {
            return @this
                .Register<TModel, TModel>(DiLifecycle.Singleton)
                .Register<TViewModel, TViewModel>(DiLifecycle.Transient)
                .Register<ILogger, ILogger>(@this.Make<ILoggerFactory>().CreateLogger(typeof(TView)))
                .DirtyRegister<TView, TViewModel>(nameof(FrameworkElement.DataContext))
            ;
        }

        public static TResult UsingTemporaryContainer<TResult>(this IDiScopeContainerFactory @this, Func<IDiRegisterContainer, TResult> func)
        {
            using(var container = CreateChildContainer(@this)) {
                return func(container);
            }
        }

        public static TView BuildView<TView>(this IDiScopeContainerFactory @this)
#if !ENABLED_STRUCT
            where TView : class
#endif
        {
            return @this.UsingTemporaryContainer(c => {
                c.Register<ILogger, ILogger>(c.Make<ILoggerFactory>().CreateLogger(typeof(TView)));
                return c.Build<TView>();
            });
        }

        #endregion
    }
}
