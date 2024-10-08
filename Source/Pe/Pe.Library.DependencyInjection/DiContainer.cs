//#define ENABLED_PRISM7

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;
#if ENABLED_PRISM7
using Prism.Ioc;
#endif

// 勉強がてら作ってみる。

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// DI コンテナ。
    /// </summary>
    /// <remarks>
    /// <para>コンストラクタ・メンバインジェクションに対応。</para>
    /// TODO: 関数注入なかったっけか・・・？
    /// </remarks>
    public class DiContainer: DisposerBase, IDiRegisterContainer
    {
        #region define

        /// <summary>
        /// コンストラクタ取得フラグ。
        /// </summary>
        private const BindingFlags ConstructorBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// メンバ取得フラグ。
        /// </summary>
        private const BindingFlags MemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        #endregion

        /// <summary>
        /// プールしているオブジェクトはコンテナに任せる。
        /// </summary>
        public DiContainer()
            : this(true)
        { }

        /// <summary>
        /// プールしているオブジェクトをコンテナに任せるか選択。
        /// </summary>
        /// <param name="managingResource">解放処理をコンテナに任せるか</param>
        public DiContainer(bool managingResource)
        {
            ManagingResource = managingResource;
        }

        #region property

        /// <summary>
        /// 解放処理をコンテナに任せるか。
        /// </summary>
        protected bool ManagingResource { get; }

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
        protected IList<DiInjectionMember> InjectionMembers { get; } = new List<DiInjectionMember>();

        private static IReadOnlyDictionary<ParameterInfo, DiInjectionAttribute> EmptyInjections = new Dictionary<ParameterInfo, DiInjectionAttribute>();

        #endregion

        #region function

        protected virtual void RegisterFactoryCore(Type interfaceType, Type objectType, string name, DiLifecycle lifecycle, DiCreator creator)
        {
            Mapping[name].TryAdd(interfaceType, objectType);
            Factory[name].TryAdd(interfaceType, new DiFactoryWorker(lifecycle, creator, this));
        }

        private void RegisterFactorySingleton(Type interfaceType, Type objectType, string name, DiCreator creator)
        {
            var lazy = new Lazy<object>(() => creator());
            RegisterFactoryCore(interfaceType, objectType, name, DiLifecycle.Singleton, () => lazy.Value);
        }

        protected virtual void SimpleRegister(Type interfaceType, Type objectType, string name, object value)
        {
            Mapping[name].TryAdd(interfaceType, objectType);
            ObjectPool[name].TryAdd(interfaceType, value);
        }

        private void Register(Type interfaceType, Type objectType, string name, DiLifecycle lifecycle, DiCreator creator)
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

        private Type GetMappingType(Type type, string name)
        {
            return Mapping[name].TryGetValue(type, out var objectType) ? objectType : type;
        }

        protected object GetCore(Type interfaceType, string name)
        {
            if(ObjectPool[name].TryGetValue(interfaceType, out var value)) {
                return value;
            }

            var targetFactory = Factory[name];
            if(targetFactory.TryGetValue(interfaceType, out var targetFactoryWorker)) {
                return targetFactoryWorker.Create();
            }

            // 対象の名前で存在しなければ存在するところから引っ張る
            if(name != string.Empty) {
                // まずは空の名前から検索
                var namelessPool = ObjectPool[string.Empty];
                if(namelessPool.TryGetValue(interfaceType, out var poolValue)) {
                    return poolValue;
                }

                var namelessFactory = Factory[string.Empty];
                if(namelessFactory.TryGetValue(interfaceType, out var factoryValue)) {
                    return factoryValue;
                }
            }

            // 順序なく総なめ
            var namedPools = GetIgnoreNamedPools(ObjectPool, name);
            foreach(var namedPool in namedPools) {
                if(namedPool.Value.TryGetValue(interfaceType, out var namedValue)) {
                    return namedValue;
                }
            }

            var namedFactories = GetIgnoreNamedPools(Factory, name);
            foreach(var namedFactory in namedFactories) {
                if(namedFactory.Value.TryGetValue(interfaceType, out var namedValue)) {
                    return namedValue;
                }
            }

            throw new DiException($"get error: {interfaceType} [{name}]");
        }

        private static List<KeyValuePair<Type, object?>> BuildManualParameters(IReadOnlyCollection<object> manualParameters)
        {
            using var arrayPoolDisposer = new ArrayPoolObject<KeyValuePair<Type, object?>>(manualParameters.Count);
            int resultIndex = 0;
            foreach(var manualParameter in manualParameters) {
                if(manualParameter is null) {
                    continue;
                }

                if(manualParameter is DiDefaultParameter diDefaultParameter) {
                    arrayPoolDisposer.Items[resultIndex++] = diDefaultParameter.GetPair();
                } else {
                    arrayPoolDisposer.Items[resultIndex++] = new KeyValuePair<Type, object?>(manualParameter.GetType(), manualParameter);
                }
            }

            var result = new List<KeyValuePair<Type, object?>>(resultIndex);
            foreach(var item in arrayPoolDisposer.Items.AsSpan(0, resultIndex)) {
                result.Add(item);
            }
            return result;
        }

        private static KeyValuePair<Type, object?> GetParameter(ParameterInfo parameterInfo, IReadOnlyCollection<KeyValuePair<Type, object?>> manualParameterItems)
        {
            foreach(var manualParameterItem in manualParameterItems) {
                if(manualParameterItem.Key == parameterInfo.ParameterType) {
                    return manualParameterItem;
                }
                if(parameterInfo.ParameterType.IsAssignableFrom(manualParameterItem.Key)) {
                    return manualParameterItem;
                }
            }

            return default;
        }

        private static IReadOnlyList<KeyValuePair<string, ConcurrentDictionary<Type, T>>> GetIgnoreNamedPools<T>(DiNamedContainer<ConcurrentDictionary<Type, T>> pool, string ignoreName)
        {
            var result = new List<KeyValuePair<string, ConcurrentDictionary<Type, T>>>(pool.Container.Count);

            foreach(var pair in pool.Container) {
                if(pair.Key == ignoreName) {
                    continue;
                }

                result.Add(pair);
            }

            return result;
        }

        private object?[] CreateParameters(string name, IReadOnlyList<ParameterInfo> parameterInfoItems, IReadOnlyDictionary<ParameterInfo, DiInjectionAttribute> parameterInjections, IReadOnlyCollection<object> manualParameters)
        {
            var manualParameterItems = BuildManualParameters(manualParameters);

            var arguments = new object?[parameterInfoItems.Count];
            for(var i = 0; i < parameterInfoItems.Count; i++) {
                var parameterInfo = parameterInfoItems[i];
                // 入力パラメータを優先して設定
                if(manualParameterItems.Count != 0) {
                    //var item = manualParameterItems.FirstOrDefault(p => p.Key == parameterInfo.ParameterType || parameterInfo.ParameterType.IsAssignableFrom(p.Key));
                    var item = GetParameter(parameterInfo, manualParameterItems);
                    if(item.Key != default) {
                        arguments[i] = item.Value;
                        manualParameterItems.Remove(item);
                        continue;
                    }
                }

                if(parameterInjections.TryGetValue(parameterInfo, out var injectAttribute) && injectAttribute.Name != string.Empty) {
                    var injectName = injectAttribute.Name;

                    if(ObjectPool[injectName].TryGetValue(parameterInfo.ParameterType, out var injectNamePoolValue)) {
                        arguments[i] = injectNamePoolValue;
                        continue;
                    }
                    if(Factory[injectName].TryGetValue(parameterInfo.ParameterType, out var injectFactory)) {
                        arguments[i] = injectFactory.Create();
                        continue;
                    }
                }

                if(ObjectPool[name].TryGetValue(parameterInfo.ParameterType, out var poolValue)) {
                    arguments[i] = poolValue;
                } else if(Factory[name].TryGetValue(parameterInfo.ParameterType, out var factoryWorker)) {
                    arguments[i] = factoryWorker.Create();
                } else {
                    if(name != string.Empty) {
                        if(ObjectPool[string.Empty].TryGetValue(parameterInfo.ParameterType, out var namelessPoolValue)) {
                            arguments[i] = namelessPoolValue;
                            continue;
                        }
                        if(Factory[string.Empty].TryGetValue(parameterInfo.ParameterType, out var namelessFactory)) {
                            arguments[i] = namelessFactory.Create();
                            continue;
                        }
                    }

                    var namedPools = GetIgnoreNamedPools(ObjectPool, name);
                    foreach(var namedPool in namedPools) {
                        if(namedPool.Value.TryGetValue(parameterInfo.ParameterType, out var namedValue)) {
                            arguments[i] = namedValue;
                            break;
                        }
                    }
                    if(arguments[i] is not null) {
                        continue;
                    }

                    var namedFactories = GetIgnoreNamedPools(Factory, name);
                    foreach(var namedFactory in namedFactories) {
                        if(namedFactory.Value.TryGetValue(parameterInfo.ParameterType, out var factory)) {
                            arguments[i] = factory.Create();
                            break;
                        }
                    }
                    if(arguments[i] is not null) {
                        continue;
                    }

                    // どうしようもねぇ
                    return Array.Empty<object>();
                }
            }

            return arguments;

        }

        private bool TryNewObjectCore(Type objectType, string name, bool isCached, DiConstructorCache constructorCache, IReadOnlyCollection<object> manualParameters, [NotNullWhen(true)] out object? createdObject)
        {
            var parameters = constructorCache.ParameterInfoItems;
            var parameterInjections = constructorCache.ParameterInjections;

            if(parameters.Count == 0) {
                if(!isCached) {
                    Constructors[name].TryAdd(objectType, constructorCache);
                }
                createdObject = constructorCache.Create(Array.Empty<object>());
                return true;
            }

            var arguments = CreateParameters(name, parameters, parameterInjections, manualParameters);
            if(arguments.Length == 0) {
                createdObject = default;
                return false;
            }
            if(arguments.Length != parameters.Count) {
                createdObject = default;
                return false;
            }

            if(!isCached) {
                Constructors[name].TryAdd(objectType, constructorCache);
            }
            createdObject = constructorCache.Create(arguments);
            return true;
        }

        private bool TryNewObject(Type objectType, string name, IReadOnlyCollection<object> manualParameters, bool useFactoryCache, [NotNullWhen(true)] out object? createdObject)
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
                if(TryNewObjectCore(objectType, name, true, constructorCache, manualParameters, out createdObject)) {
                    return true;
                }
                // 生成できなきゃ下の処理に流してキャッシュも多分変わる
            }

            // 属性付きで引数が多いものを優先
            var constructorItems = objectType.GetConstructors(ConstructorBindingFlags)
                .Select(c => (
                    constructor: c,
                    parameters: c.GetParameters(),
                    attribute: c.GetCustomAttribute<DiInjectionAttribute>()
                ))
                .Where(i => i.attribute != null || i.constructor.IsPublic)
                .OrderBy(i => i.attribute != null ? 0 : 1)
                .ThenByDescending(i => i.parameters.Length)
                .Select(i => new DiConstructorCache(i.constructor, i.parameters))
                .ToList()
            ;

#if ENABLED_STRUCT
            if(!constructorItems.Any() && type.IsValueType) {
                //TODO: 構造体っぽければそのまま作る
            }
#endif

            foreach(var constructorItem in constructorItems) {
                if(TryNewObjectCore(objectType, name, false, constructorItem, manualParameters, out createdObject)) {
                    return true;
                }
            }

            createdObject = default;
            return false;
        }

        private object NewCore(Type type, string name, IReadOnlyCollection<object> manualParameters, bool useFactoryCache)
        {
            if(ObjectPool[name].TryGetValue(type, out var poolValue)) {
                return poolValue;
            }

            if(TryNewObject(GetMappingType(type, name), name, manualParameters, useFactoryCache, out var raw)) {
                return raw;
            }

            // 失敗したのでコンストラクタ全部出して死ぬ
            var exceptionConstructorLines = GetMappingType(type, name).GetConstructors(ConstructorBindingFlags)
                .Select(a => a.ToString())
                .JoinString(Environment.NewLine)
            ;
            throw new DiException($"{type}: create error{Environment.NewLine}{exceptionConstructorLines}");
        }

        private bool TryGetInstance(Type interfaceType, string name, IReadOnlyCollection<object> manualParameters, [MaybeNullWhen(false)] out object value)
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

        private static Type GetMemberType(MemberInfo memberInfo)
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

        private void SetMemberValue<TObject>(ref TObject target, MemberInfo memberInfo, Type valueType, string name)
        {
            switch(memberInfo.MemberType) {
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberInfo;
                    if(TryGetInstance(valueType, name, Array.Empty<object>(), out var fieldValue)) {
                        fieldInfo.SetValue(target, fieldValue);
                    } else {
                        throw new DiException($"{fieldInfo}: failed to create {valueType}");
                    }
                    break;

                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    if(TryGetInstance(valueType, name, Array.Empty<object>(), out var propertyValue)) {
                        propertyInfo.SetValue(target, propertyValue);
                    } else {
                        throw new DiException($"{propertyInfo}: failed to create {valueType}");
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void InjectCore<TObject>(ref TObject target, string name)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var targetType = GetMappingType(typeof(TObject), name);
            var memberItems = targetType.GetMembers(MemberBindingFlags)
                .Select(m => (memberInfo: m, inject: m.GetCustomAttribute<DiInjectionAttribute>()))
                .ToList()
            ;
            foreach(var memberItem in memberItems.Where(a => a.inject is not null)) {
                SetMemberValue(ref target, memberItem.memberInfo, GetMemberType(memberItem.memberInfo), memberItem.inject!.Name);
            }

            // 強制付け替え処理の実施
            var dirtyPairs = memberItems
                .Join(
                    InjectionMembers.Where(d => d.BaseType.IsAssignableFrom(targetType)),
                    m => m.memberInfo.Name,
                    d => d.MemberInfo.Name,
                    (m, d) => (item: m, dirty: d )
                )
            ;
            foreach(var pair in dirtyPairs) {
                SetMemberValue(ref target, pair.item.memberInfo, pair.dirty.ObjectType, pair.item.inject?.Name ?? string.Empty);
            }

        }

        private static string ToInjectionName(string? name)
        {
            if(name == null) {
                return string.Empty;
            }

            return name.Trim();
        }

        #endregion

        #region IDiContainer

        /// <inheritdoc cref="IDiContainer.Get(Type)"/>
        public object Get(Type interfaceType)
        {
            return GetCore(interfaceType, string.Empty);
        }

        /// <inheritdoc cref="IDiContainer.Get(Type, string)"/>
        public object Get(Type interfaceType, string name)
        {
            return GetCore(interfaceType, ToInjectionName(name));
        }

        /// <inheritdoc cref="IDiContainer.Get{TInterface}()"/>
        public TInterface Get<TInterface>()
        {
            return (TInterface)Get(typeof(TInterface));
        }

        /// <inheritdoc cref="IDiContainer.Get{TInterface}(string)"/>
        public TInterface Get<TInterface>(string name)
        {
            return (TInterface)Get(typeof(TInterface), ToInjectionName(name));
        }

        /// <inheritdoc cref="IDiContainer.New(Type, IReadOnlyCollection{object})"/>
        public object New(Type type, IReadOnlyCollection<object> manualParameters)
        {
            return NewCore(type, string.Empty, manualParameters, true);
        }
        /// <inheritdoc cref="IDiContainer.New(Type, string, IReadOnlyCollection{object})"/>
        public object New(Type type, string name, IReadOnlyCollection<object> manualParameters)
        {
            return NewCore(type, ToInjectionName(name), manualParameters, true);
        }

        /// <inheritdoc cref="IDiContainer.New(Type)"/>
        public object New(Type type)
        {
            return New(type, Array.Empty<object>());
        }
        /// <inheritdoc cref="IDiContainer.New(Type, string)"/>
        public object New(Type type, string name)
        {
            return New(type, name, Array.Empty<object>());
        }


        /// <inheritdoc cref="IDiContainer.New{TObject}(IReadOnlyCollection{object})"/>
        public TObject New<TObject>(IReadOnlyCollection<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), manualParameters);
        }
        /// <inheritdoc cref="IDiContainer.New{TObject}(string, IReadOnlyCollection{object})"/>
        public TObject New<TObject>(string name, IReadOnlyCollection<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), name, manualParameters);
        }


        /// <inheritdoc cref="IDiContainer.New{TObject}"/>
        public TObject New<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), Array.Empty<object>());
        }
        /// <inheritdoc cref="IDiContainer.New{TObject}(string)"/>
        public TObject New<TObject>(string name)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return (TObject)New(typeof(TObject), name, Array.Empty<object>());
        }

        [return: MaybeNull]
        public object? CallMethod(string name, object instance, MethodInfo methodInfo, IReadOnlyCollection<object> manualParameters)
        {
            var parameters = methodInfo.GetParameters();
            var arguments = CreateParameters(name, parameters, EmptyInjections, manualParameters);

            var result = methodInfo.Invoke(instance, arguments);

            return result;
        }

        public void Inject<TObject>(TObject target)
            where TObject : class
        {
            InjectCore(ref target, string.Empty);
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
            var cloneContainer = new ScopeDiContainer(ManagingResource);
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
            foreach(var item in InjectionMembers) {
                cloneContainer.InjectionMembers.Add(item);
            }

            return cloneContainer;
        }

        #endregion

        #region IDiRegisterContainer

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(DiLifecycle)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            return Register<TInterface, TObject>(string.Empty, lifecycle);
        }

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(string, DiLifecycle)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(string name, DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            var interfaceType = typeof(TInterface);
            var objectType = typeof(TObject);
            if(interfaceType == objectType) {
                Register(typeof(TInterface), typeof(TObject), ToInjectionName(name), lifecycle, () => NewCore(typeof(TObject), string.Empty, Array.Empty<object>(), false));
            } else {
                Register(typeof(TInterface), typeof(TObject), ToInjectionName(name), lifecycle, () => NewCore(typeof(TObject), string.Empty, Array.Empty<object>(), true));
            }

            return this;
        }

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(DiLifecycle, DiCreator)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            return Register<TInterface, TObject>(string.Empty, lifecycle, creator);
        }

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(string, DiLifecycle, DiCreator)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(string name, DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            Register(typeof(TInterface), typeof(TObject), name, lifecycle, creator);

            return this;
        }

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(TObject)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            return Register<TInterface, TObject>(string.Empty, value);
        }

        /// <inheritdoc cref="IDiRegisterContainer.Register{TInterface, TObject}(string, TObject)"/>
        public IDiRegisterContainer Register<TInterface, TObject>(string name, TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            SimpleRegister(typeof(TInterface), typeof(TObject), name, value);

            return this;
        }

        /// <inheritdoc cref="IDiRegisterContainer.RegisterMember(Type, string, Type)"/>
        public IDiRegisterContainer RegisterMember(Type baseType, string memberName, Type objectType)
        {
            return RegisterMember(baseType, memberName, objectType, string.Empty);
        }

        static bool CanAdd(IList<DiInjectionMember> injectionMembers, DiInjectionMember member)
        {
            foreach(var injectionMember in injectionMembers) {
                if(injectionMember.BaseType == member.BaseType && injectionMember.MemberInfo.Name == member.MemberInfo.Name) {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc cref="IDiRegisterContainer.RegisterMember(Type, string, Type, string)"/>
        public IDiRegisterContainer RegisterMember(Type baseType, string memberName, Type objectType, string name)
        {
            var memberInfo = baseType.GetMember(memberName, MemberBindingFlags);
            if(memberInfo == null || memberInfo.Length != 1) {
                throw new NullReferenceException(memberName);
            }

            var member = new DiInjectionMember(baseType, memberInfo[0], objectType, ToInjectionName(name));
            if(CanAdd(InjectionMembers, member)) {
                throw new ArgumentException(null, $"{baseType}.{memberInfo}");
            }
            InjectionMembers.Add(member);

            return this;
        }
        /// <inheritdoc cref="IDiRegisterContainer.RegisterMember{TBase, TObject}(string)"/>
        public IDiRegisterContainer RegisterMember<TBase, TObject>(string memberName)
        {
            return RegisterMember(typeof(TBase), memberName, typeof(TObject), string.Empty);
        }
        /// <inheritdoc cref="IDiRegisterContainer.RegisterMember{TBase, TObject}(string, string)"/>
        public IDiRegisterContainer RegisterMember<TBase, TObject>(string memberName, string name)
        {
            return RegisterMember(typeof(TBase), memberName, typeof(TObject), name);
        }

        /// <inheritdoc cref="IDiRegisterContainer.Unregister(Type)"/>
        public bool Unregister(Type interfaceType)
        {
            return Unregister(interfaceType, string.Empty);
        }
        /// <inheritdoc cref="IDiRegisterContainer.Unregister(Type, string)"/>
        public bool Unregister(Type interfaceType, string name)
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

        /// <inheritdoc cref="IDiRegisterContainer.Unregister{TInterface}"/>
        public bool Unregister<TInterface>()
        {
            return Unregister(typeof(TInterface), string.Empty);
        }

        /// <inheritdoc cref="IDiRegisterContainer.Unregister{TInterface}(string)"/>
        public bool Unregister<TInterface>(string name)
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
                    if(ManagingResource) {
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
