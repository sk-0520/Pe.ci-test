using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationDiContainer: DiContainer
    {
        #region DiContainer
        #endregion

        #region function

        /// <summary>
        /// 指定の方に関するデータを破棄。
        /// <para>本来なら<see cref="ScopeDiContainer"/>で何とかすべきかもしれないが<see cref="DiContainer.Constructors"/>周りのキャッシュ構成に手を加えるのがしんどかったのでここで拡張。</para>
        /// </summary>
        /// <param name="type"></param>
        [Obsolete]
        private void ClearTypeCore(Type type)
        {
            foreach(var item in ObjectPool.Values) {
                item.TryRemove(type, out _);
            }
            foreach(var item in Factory.Values) {
                item.TryRemove(type, out _);
            }
            foreach(var item in Mapping.Values) {
                item.TryRemove(type, out _);

                // マッピング先の削除
                var targetHasKeys = item
                    .Where(i => i.Value == type)
                    .Select(i => i.Key)
                    .ToList()
                ;
                foreach(var key in targetHasKeys) {
                    item.TryRemove(key, out _);
                }
            }
            foreach(var item in Constructors.Values) {
                item.TryRemove(type, out _);

                // 引数の削除
                var targetHasKeys = item
                    .Where(i => i.Value.ParameterInfos.Any(i => i.ParameterType ==type))
                    .Select(i => i.Key)
                    .ToList()
                ;
                foreach(var key in targetHasKeys) {
                    item.TryRemove(key, out _);
                }
            }
        }

        [Obsolete]
        public void ClearType<T>() => ClearTypeCore(typeof(T));
        [Obsolete]
        public void ClearType(Type type) => ClearTypeCore(type);

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
                .Register<ILogger, ILogger>(@this.Build<ILoggerFactory>().CreateLogger(typeof(TView)))
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
                c.Register<ILogger, ILogger>(c.Build<ILoggerFactory>().CreateLogger(typeof(TView)));
                return c.Build<TView>();
            });
        }


        public static TContainer RegisterDatabase<TContainer>(this TContainer @this, ApplicationDatabaseFactoryPack factoryPack, LazyWriterWaitTimePack lazyWriterWaitTimePack, ILoggerFactory loggerFactory)
            where TContainer : IDiRegisterContainer
        {
            var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            var readerWriterLockerPack = new ApplicationReaderWriterLockerPack(
                new ApplicationMainReaderWriterLocker(),
                new ApplicationFileReaderWriterLocker(),
                new ApplicationTemporaryReaderWriterLocker()
            );
            var barrierPack = new ApplicationDatabaseBarrierPack(
                new ApplicationDatabaseBarrier(accessorPack.Main, readerWriterLockerPack.Main),
                new ApplicationDatabaseBarrier(accessorPack.File, readerWriterLockerPack.File),
                new ApplicationDatabaseBarrier(accessorPack.Temporary, readerWriterLockerPack.Temporary)
            );

            var lazyWriterPack = new ApplicationDatabaseLazyWriterPack(
                new ApplicationDatabaseLazyWriter(barrierPack.Main, lazyWriterWaitTimePack.Main, loggerFactory),
                new ApplicationDatabaseLazyWriter(barrierPack.File, lazyWriterWaitTimePack.File, loggerFactory),
                new ApplicationDatabaseLazyWriter(barrierPack.Temporary, lazyWriterWaitTimePack.Temporary, loggerFactory)
            );

            @this
                .Register<IDatabaseFactoryPack, ApplicationDatabaseFactoryPack>(factoryPack)
                .Register<IDatabaseAccessorPack, ApplicationDatabaseAccessorPack>(accessorPack)
                .Register<IDatabaseBarrierPack, ApplicationDatabaseBarrierPack>(barrierPack)
                .Register<IReaderWriterLockerPack, ApplicationReaderWriterLockerPack>(readerWriterLockerPack)
                .Register<IDatabaseLazyWriterPack, ApplicationDatabaseLazyWriterPack>(lazyWriterPack)

                .Register<IMainDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Main)
                .Register<IFileDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.File)
                .Register<ITemporaryDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Temporary)

                .Register<IMainDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.Main)
                .Register<IFileDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.File)
                .Register<ITemporaryDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.Temporary)
            ;

            return @this;
        }

        public static void UnregisterDatabase(this IDiRegisterContainer @this)
        {
            var unregisters = new Action[] {
                () => @this.Unregister<IDatabaseFactoryPack>(),
                () => @this.Unregister<IDatabaseAccessorPack>(),
                () => @this.Unregister<IDatabaseBarrierPack>(),
                () => @this.Unregister<IReaderWriterLockerPack>(),
                () => @this.Unregister<IDatabaseLazyWriterPack>(),

                () => @this.Unregister<IMainDatabaseBarrier>(),
                () => @this.Unregister<IFileDatabaseBarrier>(),
                () => @this.Unregister<ITemporaryDatabaseBarrier>(),

                () => @this.Unregister<IMainDatabaseLazyWriter>(),
                () => @this.Unregister<IFileDatabaseLazyWriter>(),
                () => @this.Unregister<ITemporaryDatabaseLazyWriter>(),
            };

            foreach(var unreg in unregisters.Reverse()) {
                unreg();
            }
        }

        #endregion
    }
}
