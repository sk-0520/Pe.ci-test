using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// 限定的なDIコンテナ。
    /// </summary>
    public interface IScopeDiContainer : IDiRegisterContainer, IDisposable
    {
        /// <summary>
        /// ただ単純な登録。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lifecycle"></param>
        /// <returns></returns>
        IScopeDiContainer Register<TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
    }

    internal class ScopeDiContainer : DiContainer, IScopeDiContainer
    {
        public ScopeDiContainer(bool isDisposeObjectPool)
            : base(isDisposeObjectPool)
        { }

        #region property

        HashSet<Type> RegisteredTypeSet { get; } = new HashSet<Type>();

        #endregion

        #region IScopeDiContainer

        public IScopeDiContainer Register<TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            base.Register<TObject, TObject>(lifecycle);

            return this;
        }


        #endregion

        #region DiContainer

        protected override void RegisterFactoryCore(Type interfaceType, Type objectType, string? name, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!RegisteredTypeSet.Contains(interfaceType)) {
                Mapping.Remove(interfaceType);
                Factory.Remove(interfaceType);

                base.RegisterFactoryCore(interfaceType, objectType, name, lifecycle, creator);
                RegisteredTypeSet.Add(interfaceType);
            } else {
                throw new ArgumentException(nameof(interfaceType));
            }
        }

        protected override void SimpleRegister(Type interfaceType, Type objectType, object value)
        {
            if(!RegisteredTypeSet.Contains(interfaceType)) {
                Mapping.Remove(interfaceType);
                ObjectPool.Remove(interfaceType);

                base.SimpleRegister(interfaceType, objectType, value);
                RegisteredTypeSet.Add(interfaceType);
            } else {
                throw new ArgumentException(nameof(interfaceType));
            }
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var type in RegisteredTypeSet) {
                        if(Factory.TryGetValue(type, out var value)) {
                            value.Dispose();
                        }

                        if(IsDisposeObjectPool) {
                            if(ObjectPool.TryGetValue(type, out var poolObject)) {
                                if(poolObject != this && poolObject is IDisposable disposer) {
                                    disposer.Dispose();
                                }
                            }
                        }

                    }

                }
            }

            if(IsDisposed) {
                return;
            }

            OnDisposing();

            if(disposing) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        #endregion
    }
}
