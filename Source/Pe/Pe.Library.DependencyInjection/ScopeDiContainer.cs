using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    internal class ConcurrentHashSet<T>: ConcurrentDictionary<T, byte>
        where T : notnull
    {
        #region function

        public bool Contains(T key) => ContainsKey(key);
        public bool Add(T key) => TryAdd(key, 1);

        public IReadOnlyList<T> GetValues() => Keys.ToList();

        #endregion
    }

    internal class ScopeDiContainer: DiContainer, IScopeDiContainer
    {
        public ScopeDiContainer(bool isDisposeObjectPool)
            : base(isDisposeObjectPool)
        { }

        #region property

        private DiNamedContainer<ConcurrentHashSet<Type>> RegisteredTypeSet { get; } = new DiNamedContainer<ConcurrentHashSet<Type>>();

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

        protected override void RegisterFactoryCore(Type interfaceType, Type objectType, string name, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!RegisteredTypeSet[name].Contains(interfaceType)) {
                Mapping[name].TryRemove(interfaceType, out _);
                Factory[name].TryRemove(interfaceType, out _);

                base.RegisterFactoryCore(interfaceType, objectType, name, lifecycle, creator);
                RegisteredTypeSet[name].Add(interfaceType);
            } else {
                throw new ArgumentException(null, nameof(interfaceType));
            }
        }

        protected override void SimpleRegister(Type interfaceType, Type objectType, string name, object value)
        {
            if(!RegisteredTypeSet[name].Contains(interfaceType)) {
                Mapping[name].TryRemove(interfaceType, out _);
                ObjectPool[name].TryRemove(interfaceType, out _);

                base.SimpleRegister(interfaceType, objectType, name, value);
                RegisteredTypeSet[name].Add(interfaceType);
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
                    foreach(var pair in RegisteredTypeSet.ToArray()) {
                        var name = pair.Key;
                        foreach(var type in pair.Value.GetValues()) {
                            if(Factory[name].TryGetValue(type, out var value)) {
                                value.Dispose();
                            }

                            if(ManagingResource) {
                                if(ObjectPool[name].TryGetValue(type, out var poolObject)) {
                                    if(poolObject != this && poolObject is IDisposable disposer) {
                                        disposer.Dispose();
                                    }
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
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
                GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
            }

            IsDisposed = true;
        }

        #endregion
    }
}
