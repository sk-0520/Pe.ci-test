#define ENABLED_PRISM7

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
#if ENABLED_PRISM7
using Prism.Ioc;
#endif

// 勉強がてら作ってみる。

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// 生成タイミング。
    /// </summary>
    public enum DiLifecycle
    {
        /// <summary>
        /// 毎回作る。
        /// </summary>
        Transient,
        /// <summary>
        /// シングルトン。
        /// </summary>
        Singleton,
    }

    /// <summary>
    /// 注入マーク。
    /// <para><see cref="IDiContainer.New{T}(IEnumerable{object})"/> する際の対象コンストラクタを限定。</para>
    /// <para><see cref="IDiContainer.Inject{T}(T)"/> を使用する際の対象を指定。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
            Name = string.Empty;
        }

        public InjectAttribute(string name)
        {
            Name = name;
        }

        #region property

        public string Name { get; }

        #endregion
    }

    /// <summary>
    /// DI処理でわっけ分からんことになったら投げられる例外。
    /// <para><see cref="ArgumentException"/>等の分かっているのはその例外を投げるのでこの例外だけ受ければ良いという話ではない。</para>
    /// </summary>
    public class DiException : ApplicationException
    {
        public DiException()
        { }

        public DiException(string? message)
            : base(message)
        { }

        public DiException(string? message, Exception? innerException)
            : base(message, innerException)
        { }

        protected DiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }

    /// <summary>
    /// パラメータに型判別できない(default(T)とか)を無理やり認識させるしゃあなし対応。
    /// </summary>
    public struct DiDefaultParameter
    {
        public DiDefaultParameter(Type type)
        {
            Type = type;
        }

        #region property

        public Type Type { get; }

        #endregion

        #region function

        public KeyValuePair<Type, object> GetPair()
        {
            if(Type.IsValueType) {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                return new KeyValuePair<Type, object>(Type, Activator.CreateInstance(Type));
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
            }

#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
            return new KeyValuePair<Type, object>(Type, null);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
        }

        public static DiDefaultParameter Create<T>()
        {
            return new DiDefaultParameter(typeof(T));
        }

        #endregion
    }

    public interface IDiScopeContainerFactory
    {
        /// <summary>
        /// 限定的なDIコンテナを作成。
        /// </summary>
        /// <returns>現在マッピングを複製したDIコンテナ。</returns>
        IScopeDiContainer Scope();
    }

    /// <summary>
    /// 取得可能コンテナ。
    /// </summary>
    public interface IDiContainer : IDiScopeContainerFactory
#if ENABLED_PRISM7
        , IContainerProvider
#endif
    {
        /// <summary>
        /// マッピングから実体を取得。
        /// <para>必ずしも依存が解決されるわけではない。</para>
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns>実体そのまま</returns>
        TInterface Get<TInterface>();

        /// <summary>
        /// コンストラクタインジェクション。
        /// <para>依存を解決するとともにパラメータを指定。</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="manualParameters">依存関係以外のパラメータ。前方から型に一致するものが使用される。</param>
        /// <returns></returns>
        object New(Type type, IEnumerable<object> manualParameters);

        /// <summary>
        /// コンストラクタインジェクション。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object New(Type type);

        /// <summary>
        /// コンストラクタインジェクション。
        /// <para>依存を解決するとともにパラメータを指定。</para>
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="manualParameters">依存関係以外のパラメータ。前方から型に一致するものが使用される。</param>
        /// <returns></returns>
        TObject New<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <summary>
        /// コンストラクタインジェクション。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        TObject New<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <summary>
        /// <see cref="InjectAttribute"/> を補完する。
        /// </summary>
        /// <typeparam name="TObject">生成済みオブジェクト</typeparam>
        /// <param name="target"></param>
        void Inject<TObject>(TObject target)
            where TObject : class
        ;
#if ENABLED_STRUCT
        void Inject<TObject>(ref TObject target)
            where TObject : struct
        ;
#endif

        /// <summary>
        /// <see cref="New"/>して<see cref="Inject{TObject}(TObject)"/>するイメージ。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="manualParameters">依存関係以外のパラメータ。前方から型に一致するものが使用される。</param>
        /// <returns></returns>
        TObject Make<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        TObject Make<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
    }

    /// <summary>
    ///登録可能コンテナ。
    /// </summary>
    public interface IDiRegisterContainer : IDiContainer
#if ENABLED_PRISM7
        , IContainerRegistry
#endif
    {
        /// <summary>
        /// シンプルなマッピングを追加。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <summary>
        /// 自分で作る版のマッピング。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lifecycle"></param>
        /// <param name="creator"></param>
        IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <summary>
        /// シングルトンとしてオブジェクトを単純登録。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        IDiRegisterContainer Register<TInterface, TObject>(TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <summary>
        /// <see cref="IDiContainer.Inject{TObject}(TObject)"/> を行う際に <see cref="InjectAttribute"/> を設定できないプロパティに無理やり設定する。
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="memberName"></param>
        /// <param name="objectType"></param>
        IDiRegisterContainer DirtyRegister(Type baseType, string memberName, Type objectType);
        IDiRegisterContainer DirtyRegister<TBase, TObject>(string memberName);

        /// <summary>
        /// 登録解除。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        bool Unregister<TInterface>();
    }

    /// <summary>
    /// DI コンテナ。
    /// </summary>
    public class DiContainer : DisposerBase, IDiRegisterContainer
    {
        const string DummyName = "";
        /// <summary>
        /// プールしているオブジェクトはコンテナに任せる。
        /// </summary>
        public DiContainer()
            : this(true)
        { }

        /// <summary>
        /// プールしているオブジェクトをコンテナに任せるか選択。
        /// </summary>
        /// <param name="isDisposeObjectPool">解放処理をコンテナに任せるか</param>
        public DiContainer(bool isDisposeObjectPool)
        {
            IsDisposeObjectPool = isDisposeObjectPool;
        }

        #region property

        /// <summary>
        /// 解放処理をコンテナに任せるか。
        /// </summary>
        protected bool IsDisposeObjectPool { get; }

        /// <summary>
        /// IF → 実体 のマッピング。
        /// </summary>
        protected DiNamedContainer<ConcurrentDictionary<Type, Type>> Mapping { get; } = new DiNamedContainer<ConcurrentDictionary<Type, Type>>();
        /// <summary>
        /// 生成処理キャッシュ。
        /// </summary>
        protected DiNamedContainer<ConcurrentDictionary<Type, DiFactoryWorker>> Factory { get; } = new DiNamedContainer<ConcurrentDictionary<Type, DiFactoryWorker>>();
        /// <summary>
        /// コンストラクタキャッシュ。
        /// </summary>
        protected DiNamedContainer<ConcurrentDictionary<Type, DiConstructorCache>> Constructors { get; } = new DiNamedContainer<ConcurrentDictionary<Type, DiConstructorCache>>();
        protected DiNamedContainer<ConcurrentDictionary<Type, object>> ObjectPool { get; } = new DiNamedContainer<ConcurrentDictionary<Type, object>>();
        protected IList<DiDirtyMember> DirtyMembers { get; } = new List<DiDirtyMember>();

        #endregion

        #region function

        protected virtual void RegisterFactoryCore(Type interfaceType, Type objectType, string name, DiLifecycle lifecycle, DiCreator creator)
        {
            Mapping[name].TryAdd(interfaceType, objectType);
            Factory[name].TryAdd(interfaceType, new DiFactoryWorker(lifecycle, creator, this));
        }

        void RegisterFactorySingleton(Type interfaceType, Type objectType, string name, DiCreator creator)
        {
            var lazy = new Lazy<object>(() => creator());
            RegisterFactoryCore(interfaceType, objectType, name, DiLifecycle.Singleton, () => lazy.Value);
        }

        protected virtual void SimpleRegister(Type interfaceType, Type objectType, string name, object value)
        {
            Mapping[name].TryAdd(interfaceType, objectType);
            ObjectPool[name].TryAdd(interfaceType, value);
        }

        void Register(Type interfaceType, Type objectType, string name, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!interfaceType.IsAssignableFrom(objectType)) {
                throw new ArgumentException($"error: {interfaceType} <- {objectType}");
            }

            switch(lifecycle) {
                case DiLifecycle.Transient:
                    RegisterFactoryCore(interfaceType, objectType, name, DiLifecycle.Transient, creator);
                    break;

                case DiLifecycle.Singleton:
                    RegisterFactorySingleton(interfaceType, objectType, name, creator);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        bool Unregister(Type interfaceType, string name)
        {
            if(Factory[name].TryGetValue(interfaceType, out var factory)) {
                Mapping[name].TryRemove(interfaceType, out _);
                Factory[name].TryRemove(interfaceType, out _);
                Constructors[name].TryRemove(interfaceType, out _);
                if(factory.Lifecycle == DiLifecycle.Singleton) {
                    ObjectPool[name].TryRemove(interfaceType, out _);
                    factory.Dispose();
                }
                return true;
            }

            return false;
        }

        Type GetMappingType(Type type, string name)
        {
            return Mapping[name].TryGetValue(type, out var objectType) ? objectType : type;
        }

        object[] CreateParameters(IReadOnlyList<ParameterInfo> parameterInfos, IEnumerable<object> manualParameters)
        {
            var manualParameterItems = manualParameters
                .Where(o => o != null)
                .Select(o => o.GetType() == typeof(DiDefaultParameter) ? ((DiDefaultParameter)o).GetPair() : new KeyValuePair<Type, object>(o.GetType(), o))
                .ToList()
            ;

            var arguments = new object[parameterInfos.Count];
            for(var i = 0; i < parameterInfos.Count; i++) {
                var parameterInfo = parameterInfos[i];
                // 入力パラメータを優先して設定
                if(manualParameterItems.Count != 0) {
                    var item = manualParameterItems.FirstOrDefault(p => p.Key == parameterInfo.ParameterType || parameterInfo.ParameterType.IsAssignableFrom(p.Key));
                    if(item.Key != default(Type)) {
                        arguments[i] = item.Value;
                        manualParameterItems.Remove(item);
                        continue;
                    }
                }

                if(ObjectPool[DummyName].TryGetValue(parameterInfo.ParameterType, out var poolValue)) {
                    arguments[i] = poolValue;
                } else if(Factory[DummyName].TryGetValue(parameterInfo.ParameterType, out var factoryWorker)) {
                    arguments[i] = factoryWorker.Create();
                } else {
                    // どうしようもねぇ
#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
                    return null;
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
                }
            }

            return arguments;

        }

        bool TryNewObjectCore(Type objectType, string name, bool isCached, DiConstructorCache constructorCache, IEnumerable<object> manualParameters, out object? createdObject)
        {
            var parameters = constructorCache.ParameterInfos;

            if(parameters.Count == 0) {
                if(!isCached) {
                    Constructors[name].TryAdd(objectType, constructorCache);
                }
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                createdObject = constructorCache.Create(null);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                return true;
            }

            var arguments = CreateParameters(parameters, manualParameters);
            if(arguments == null) {
                createdObject = default(object);
                return false;
            }
            if(arguments.Length != parameters.Count) {
                createdObject = default(object);
                return false;
            }

            if(!isCached) {
                Constructors[name].TryAdd(objectType, constructorCache);
            }
            createdObject = constructorCache.Create(arguments);
            return true;
        }

        bool TryNewObject(Type objectType, string name, IEnumerable<object> manualParameters, bool useFactoryCache, out object createdObject)
        {
            if(ObjectPool[name].TryGetValue(objectType, out var poolValue)) {
                createdObject = poolValue;
                return true;
            }

            if(useFactoryCache) {
                // 生成可能なものはこの段階で生成
                if(Factory[name].TryGetValue(objectType, out var factoryWorker)) {
                    createdObject = factoryWorker.Create();
                    return true;
                }
            }

            // コンストラクタのキャッシュを使用
            if(Constructors[name].TryGetValue(objectType, out var constructorCache)) {
                //NOTE: これ生成できなければ下の処理に流した方がいいと思う
#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
                return TryNewObjectCore(objectType, name, true, constructorCache, manualParameters, out createdObject);
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
            }

            // 属性付きで引数が多いものを優先
            var constructorItems = objectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Select(c => new {
                    Constructor = c,
                    Parameters = c.GetParameters(),
                    Attribute = c.GetCustomAttribute<InjectAttribute>()
                })
                .Where(i => i.Attribute != null ? true : i.Constructor.IsPublic)
                .OrderBy(i => i.Attribute != null ? 0 : 1)
                .ThenByDescending(i => i.Parameters.Length)
                .Select(i => new DiConstructorCache(i.Constructor, i.Parameters))
                .ToList()
            ;

#if ENABLED_STRUCT
            if(!constructorItems.Any() && type.IsValueType) {
                //TODO: 構造体っぽければそのまま作る
            }
#endif

            foreach(var constructorItem in constructorItems) {
#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
                if(TryNewObjectCore(objectType, name, false, constructorItem, manualParameters, out createdObject)) {
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
                    return true;
                }
            }

#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
            createdObject = default(object);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
            return false;
        }

        object NewCore(Type type, string name, IEnumerable<object> manualParameters, bool useFactoryCache)
        {
            if(ObjectPool[name].TryGetValue(type, out var poolValue)) {
                return poolValue;
            }

            if(TryNewObject(GetMappingType(type, name), name, manualParameters, useFactoryCache, out var raw)) {
                return raw;
            }

            throw new DiException($"{type}: create rror");
        }

        bool TryGetInstance(Type interfaceType, string name, IEnumerable<object> manualParameters, out object value)
        {
            if(ObjectPool[name].TryGetValue(interfaceType, out var poolValue)) {
                value = poolValue;
                return true;
            }

            // 生成可能なものはこの段階で生成
            if(Factory[name].TryGetValue(interfaceType, out var factoryWorker)) {
                value = factoryWorker.Create();
                return true;
            }

            return TryNewObject(GetMappingType(interfaceType, name), name, manualParameters, true, out value);
        }

        Type GetMemberType(MemberInfo memberInfo)
        {
            switch(memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;

                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;

                default:
                    throw new NotImplementedException();
            }
        }

        void SetMemberValue<TObject>(ref TObject target, MemberInfo memberInfo, Type valueType, string name)
        {
            switch(memberInfo.MemberType) {
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberInfo;
                    if(TryGetInstance(valueType, name, Enumerable.Empty<object>(), out var fieldValue)) {
                        fieldInfo.SetValue(target, fieldValue);
                    } else {
                        throw new DiException($"{fieldInfo}: create fail");
                    }
                    break;

                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    if(TryGetInstance(valueType, name, Enumerable.Empty<object>(), out var propertyValue)) {
                        propertyInfo.SetValue(target, propertyValue);
                    } else {
                        throw new DiException($"{propertyInfo}: create fail");
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void InjectCore<TObject>(ref TObject target, string name)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var targetType = GetMappingType(typeof(TObject), name);
            var memberItems = targetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Select(m => new { MemberInfo = m, Inject = m.GetCustomAttribute<InjectAttribute>() })
                .ToList()
            ;
            foreach(var memberItem in memberItems.Where(i => i.Inject != null)) {
                SetMemberValue(ref target, memberItem.MemberInfo, GetMemberType(memberItem.MemberInfo), memberItem.Inject!.Name);
            }

            // 強制付け替え処理の実施
            var dirtyPairs = memberItems
                .Join(
                    DirtyMembers.Where(d => d.BaseType.IsAssignableFrom(targetType)),
                    m => m.MemberInfo.Name,
                    d => d.MemberInfo.Name,
                    (m, d) => new { Item = m, Dirty = d }
                )
            ;
            foreach(var pair in dirtyPairs) {
                SetMemberValue(ref target, pair.Item.MemberInfo, pair.Dirty.ObjectType, pair.Item.Inject?.Name ?? string.Empty);
            }

        }

        #endregion

        #region IDiContainer

        public object Get(Type interfaceType)
        {
            if(ObjectPool[DummyName].TryGetValue(interfaceType, out var value)) {
                return value;
            }

            return Factory[DummyName][interfaceType].Create();
        }

        public TInterface Get<TInterface>()
        {
            return (TInterface)Get(typeof(TInterface));
        }

        public object New(Type type, IEnumerable<object> manualParameters)
        {
            return NewCore(type, string.Empty, manualParameters, true);
        }

        public object New(Type type)
        {
            return New(type, Enumerable.Empty<object>());
        }

        public TObject New<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), manualParameters);
        }

        public TObject New<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), Enumerable.Empty<object>());
        }

        public void Inject<TObject>(TObject target)
            where TObject : class
        {
            InjectCore(ref target, string.Empty);
        }

        public TObject Make<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var obj = New<TObject>(manualParameters);
            InjectCore(ref obj, string.Empty);
            return obj;
        }

        public TObject Make<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return Make<TObject>(Enumerable.Empty<object>());
        }


#if ENABLED_STRUCT
        public void Inject<TObject>(ref TObject target)
            where TObject : struct
        {
            InjectCore(ref target);
        }
#endif

#if ENABLED_PRISM7
        public object Resolve(Type type) => New(type);
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters) => New(type, parameters.Select(p => p.Instance));
        public object Resolve(Type type, string name) => throw new NotSupportedException();
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters) => throw new NotSupportedException();
#endif

        #endregion

        #region IDiScopeContainerCreator

        public virtual IScopeDiContainer Scope()
        {
            var cloneContainer = new ScopeDiContainer(IsDisposeObjectPool);
            foreach(var pair in Mapping.ToArray()) {
                var map = cloneContainer.Mapping[pair.Key];
                foreach(var sub in pair.Value) {
                    map.TryAdd(sub.Key, sub.Value);
                }
            }
            foreach(var pair in Factory.ToArray()) {
                var factory = cloneContainer.Factory[pair.Key];
                foreach(var sub in pair.Value) {
                    factory.TryAdd(sub.Key, sub.Value);
                }
            }
            foreach(var pair in ObjectPool.ToArray()) {
                var objectPool = cloneContainer.ObjectPool[pair.Key];
                foreach(var sub in pair.Value) {
                    objectPool.TryAdd(sub.Key, sub.Value);
                }
            }
            foreach(var pair in Constructors.ToArray()) {
                var constructor = cloneContainer.Constructors[pair.Key];
                foreach(var sub in pair.Value) {
                    constructor.TryAdd(sub.Key, sub.Value);
                }
            }
            foreach(var item in DirtyMembers) {
                cloneContainer.DirtyMembers.Add(item);
            }

            return cloneContainer;
        }

        #endregion

        #region IDiRegisterContainer

        public IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            var interfaceType = typeof(TInterface);
            var objectType = typeof(TObject);
            if(interfaceType == objectType) {
                Register(typeof(TInterface), typeof(TObject), string.Empty, lifecycle, () => NewCore(typeof(TObject), string.Empty, Enumerable.Empty<object>(), false));
            } else {
                Register(typeof(TInterface), typeof(TObject), string.Empty, lifecycle, () => NewCore(typeof(TObject), string.Empty, Enumerable.Empty<object>(), true));
            }

            return this;
        }

        public IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            Register(typeof(TInterface), typeof(TObject), string.Empty, lifecycle, creator);

            return this;
        }

        public IDiRegisterContainer Register<TInterface, TObject>(TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            SimpleRegister(typeof(TInterface), typeof(TObject), string.Empty, value);

            return this;
        }

        public IDiRegisterContainer DirtyRegister(Type baseType, string memberName, Type objectType)
        {
            var memberInfo = baseType.GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if(memberInfo == null || memberInfo.Length != 1) {
                throw new NullReferenceException(memberName);
            }
            var member = new DiDirtyMember(baseType, memberInfo[0], objectType);
            if(DirtyMembers.Any(m => m.BaseType == member.BaseType && m.MemberInfo.Name == member.MemberInfo.Name)) {
                throw new ArgumentException($"{baseType}.{memberInfo}");
            }
            DirtyMembers.Add(member);

            return this;
        }

        public IDiRegisterContainer DirtyRegister<TBase, TObject>(string propertyName)
        {
            DirtyRegister(typeof(TBase), propertyName, typeof(TObject));

            return this;
        }

        public bool Unregister<TInterface>()
        {
            return Unregister(typeof(TInterface), string.Empty);
        }

#if ENABLED_PRISM7
        public bool IsRegistered(Type type) => Mapping[string.Empty].ContainsKey(type);
        public bool IsRegistered(Type type, string name) => throw new NotSupportedException();
        public IContainerRegistry Register(Type from, Type to)
        {
            Register(from, to, string.Empty, DiLifecycle.Transient, () => NewCore(to, string.Empty, Enumerable.Empty<object>(), false));
            return this;
        }
        public IContainerRegistry Register(Type from, Type to, string name) => throw new NotSupportedException();
        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            SimpleRegister(type, instance.GetType(), string.Empty, instance);
            return this;
        }
        public IContainerRegistry RegisterInstance(Type type, object instance, string name) => throw new NotSupportedException();
        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Register(from, to, string.Empty, DiLifecycle.Singleton, () => NewCore(to, string.Empty, Enumerable.Empty<object>(), false));
            return this;
        }
        public IContainerRegistry RegisterSingleton(Type from, Type to, string name) => throw new NotSupportedException();
#endif

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var factory in Factory.Values.SelectMany(i => i.Values)) {
                        factory.Dispose();
                    }
                    if(IsDisposeObjectPool) {
                        foreach(var pair in ObjectPool.ToArray()) {
                            foreach(var sub in pair.Value) {
                                // 自分自身が処理中なので無視
                                if(sub.Value == this) {
                                    continue;
                                }
                                if(sub.Value is IDisposable disposer) {
                                    disposer.Dispose();
                                }
                            }
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
