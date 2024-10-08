using System;
using System.Linq;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal class ApplicationDiContainer: DiContainer
    {
        #region function
        #endregion
    }

    internal static class IDiRegisterContainerExtensions
    {
        #region function

        public static IScopeDiContainer CreateChildContainer(this IDiScopeContainerFactory factory)
        {
            var scopeDiContainer = factory.Scope();
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

        public static IDiRegisterContainer RegisterMvvm<TModel, TViewModel, TView>(this IDiRegisterContainer container)
            where TModel : BindModelBase
            where TViewModel : ViewModelBase
            where TView : FrameworkElement
        {
            return container
                .Register<TModel, TModel>(DiLifecycle.Singleton)
                .Register<TViewModel, TViewModel>(DiLifecycle.Transient)
                .Register<ILogger, ILogger>(container.Build<ILoggerFactory>().CreateLogger(typeof(TView)))
                .RegisterMember<TView, TViewModel>(nameof(FrameworkElement.DataContext))
            ;
        }

        public static TResult UsingTemporaryContainer<TResult>(this IDiScopeContainerFactory factory, Func<IDiRegisterContainer, TResult> func)
        {
            using(var container = CreateChildContainer(factory)) {
                return func(container);
            }
        }

        public static TView BuildView<TView>(this IDiScopeContainerFactory factory)
#if !ENABLED_STRUCT
            where TView : class
#endif
        {
            return factory.UsingTemporaryContainer(c => {
                c.Register<ILogger, ILogger>(c.Build<ILoggerFactory>().CreateLogger(typeof(TView)));
                return c.Build<TView>();
            });
        }

        public static TContainer RegisterDatabase<TContainer>(this TContainer container, ApplicationDatabaseFactoryPack factoryPack, DelayWriterWaitTimePack delayWriterWaitTimePack, ILoggerFactory loggerFactory)
            where TContainer : IDiRegisterContainer
        {
            var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            var readerWriterLockerPack = new ApplicationReadWriteLockHelperPack(
                new ApplicationMainReadWriteLockHelper(),
                new ApplicationLargeReadWriteLockHelper(),
                new ApplicationTemporaryReadWriteLockHelper()
            );
            var barrierPack = new ApplicationDatabaseBarrierPack(
                new ApplicationDatabaseBarrier(accessorPack.Main, readerWriterLockerPack.Main),
                new ApplicationDatabaseBarrier(accessorPack.Large, readerWriterLockerPack.Large),
                new ApplicationDatabaseBarrier(accessorPack.Temporary, readerWriterLockerPack.Temporary)
            );

            var delayWriterPack = new ApplicationDatabaseDelayWriterPack(
                new ApplicationDatabaseDelayWriter(barrierPack.Main, delayWriterWaitTimePack.Main, loggerFactory),
                new ApplicationDatabaseDelayWriter(barrierPack.Large, delayWriterWaitTimePack.Large, loggerFactory),
                new ApplicationDatabaseDelayWriter(barrierPack.Temporary, delayWriterWaitTimePack.Temporary, loggerFactory)
            );

            container
                .Register<IDatabaseFactoryPack, ApplicationDatabaseFactoryPack>(factoryPack)
                .Register<IDatabaseAccessorPack, ApplicationDatabaseAccessorPack>(accessorPack)

                .Register<IMainDatabaseAccessor, ApplicationDatabaseAccessor>(accessorPack.Main)
                .Register<ILargeDatabaseAccessor, ApplicationDatabaseAccessor>(accessorPack.Large)
                .Register<ITemporaryDatabaseAccessor, ApplicationDatabaseAccessor>(accessorPack.Temporary)

                .Register<IDatabaseBarrierPack, ApplicationDatabaseBarrierPack>(barrierPack)
                .Register<IReadWriteLockHelperPack, ApplicationReadWriteLockHelperPack>(readerWriterLockerPack)
                .Register<IDatabaseDelayWriterPack, ApplicationDatabaseDelayWriterPack>(delayWriterPack)

                .Register<IMainDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Main)
                .Register<ILargeDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Large)
                .Register<ITemporaryDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Temporary)

                .Register<IMainDatabaseDelayWriter, ApplicationDatabaseDelayWriter>(delayWriterPack.Main)
                .Register<ILargeDatabaseDelayWriter, ApplicationDatabaseDelayWriter>(delayWriterPack.Large)
                .Register<ITemporaryDatabaseDelayWriter, ApplicationDatabaseDelayWriter>(delayWriterPack.Temporary)
            ;

            return container;
        }

        public static void UnregisterDatabase(this IDiRegisterContainer container)
        {
            var unregisters = new Action[] {
                () => container.Unregister<IDatabaseFactoryPack>(),
                () => container.Unregister<IDatabaseAccessorPack>(),
                () => container.Unregister<IDatabaseBarrierPack>(),
                () => container.Unregister<IReadWriteLockHelperPack>(),
                () => container.Unregister<IDatabaseDelayWriterPack>(),

                () => container.Unregister<IMainDatabaseBarrier>(),
                () => container.Unregister<ILargeDatabaseBarrier>(),
                () => container.Unregister<ITemporaryDatabaseBarrier>(),

                () => container.Unregister<IMainDatabaseDelayWriter>(),
                () => container.Unregister<ILargeDatabaseDelayWriter>(),
                () => container.Unregister<ITemporaryDatabaseDelayWriter>(),
            };

            foreach(var unreg in unregisters.Reverse()) {
                unreg();
            }
        }

        #endregion
    }
}
