using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

// 勉強がてら作ってみる。

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
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
    public class InjectionAttribute : Attribute
    { }

    /// <summary>
    /// パラメータに型判別できない(default(Object)とか)を無理やり認識させるしゃあなし対応。
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
                return new KeyValuePair<Type, object>(Type, Activator.CreateInstance(Type));
            }

            return new KeyValuePair<Type, object>(Type, null);
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

    public interface IDiContainer : IDiScopeContainerFactory
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
        /// <see cref="InjectionAttribute"/> を補完する。
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

    public interface IDiRegisterContainer : IDiContainer
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
        /// <see cref="IDiContainer.Inject{TObject}(TObject)"/> を行う際に <see cref="InjectionAttribute"/> を設定できないプロパティに無理やり設定する。
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="memberName"></param>
        /// <param name="objectType"></param>
        IDiRegisterContainer DirtyRegister(Type baseType, string memberName, Type objectType);
        IDiRegisterContainer DirtyRegister<TBase, TObject>(string memberName);
    }

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

    /// <summary>
    /// マッピング生成処理。
    /// </summary>
    /// <returns></returns>
    public delegate object DiCreator();

    /// <summary>
    /// マッピング生成キャッシュ。
    /// </summary>
    public sealed class DiFactoryWorker : DisposerBase
    {
        public DiFactoryWorker(DiLifecycle lifecycle, DiCreator creator, object bind)
        {
            Lifecycle = lifecycle;
            Creator = creator;
        }

        #region property

        public DiLifecycle Lifecycle { get; }
        DiCreator Creator { get; }

        bool CreatedSingleton { get; set; }

        #endregion

        #region function

        public object Create()
        {
            var result = Creator();

            if(Lifecycle == DiLifecycle.Singleton) {
                CreatedSingleton = true;
            }

            return result;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(CreatedSingleton) {
                        var createdObject = Create();
                        if(createdObject is IDisposable disposer) {
                            disposer.Dispose();
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// コンストラクタ情報キャッシュ。
    /// </summary>
    public sealed class DiConstructorCache
    {
        public DiConstructorCache(ConstructorInfo constructorInfo, IReadOnlyList<ParameterInfo> parameterInfos)
        {
            ConstructorInfo = constructorInfo;
            ParameterInfos = parameterInfos;
        }

        #region proeprty

        public ConstructorInfo ConstructorInfo { get; }
        public IReadOnlyList<ParameterInfo> ParameterInfos { get; }
        Func<object[], object> Creator { get; set; }

        #endregion

        #region function

        IEnumerable<ParameterExpression> CreateParameterExpressions()
        {
            return ParameterInfos
                .Select((p, i) => Expression.Parameter(typeof(object), "wrapperArg_" + i.ToString()))
            ;
        }

        IEnumerable<UnaryExpression> CreateConvertExpressions(IEnumerable<ParameterExpression> parameterExpressions)
        {
            return ParameterInfos
                .Zip(parameterExpressions, (pi, pe) => Expression.Convert(pe, pi.ParameterType))
            ;
        }

        FuncN CreateFunction<FuncN>()
        {
            var parameterExpressions = CreateParameterExpressions().ToArray();
            var convertExpressions = CreateConvertExpressions(parameterExpressions).ToArray();
            Debug.Assert(parameterExpressions.Length == convertExpressions.Length);

            var constructorNewParams = Expression.Lambda<FuncN>(
                Expression.Convert(
                    Expression.New(
                        ConstructorInfo,
                        convertExpressions
                    ),
                    typeof(object)
                ),
                "constructorNewParams_" + ParameterInfos.Count.ToString(),
                parameterExpressions
            );
            var creator = constructorNewParams.Compile();

            return creator;
        }

        public object Create(object[] parameters)
        {
            if(Creator == null) {
                if(ParameterInfos.Count == 0) {
                    var newExp = Expression.New(ConstructorInfo);
                    var lambda = Expression.Lambda<Func<object>>(newExp);
                    var creator = lambda.Compile();
                    Creator = p => creator();
                } else if(ParameterInfos.Count < 10) {

                    switch(ParameterInfos.Count) {
                        case 1: {
                                var creator = CreateFunction<Func<object, object>>();
                                Creator = p => creator(p[0]);
                            }
                            break;

                        case 2: {
                                var creator = CreateFunction<Func<object, object, object>>();
                                Creator = p => creator(p[0], p[1]);
                            }
                            break;

                        case 3: {
                                var creator = CreateFunction<Func<object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2]);
                            }
                            break;

                        case 4: {
                                var creator = CreateFunction<Func<object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3]);
                            }
                            break;

                        case 5: {
                                var creator = CreateFunction<Func<object, object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3], p[4]);
                            }
                            break;

                        case 6: {
                                var creator = CreateFunction<Func<object, object, object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3], p[4], p[5]);
                            }
                            break;

                        case 7: {
                                var creator = CreateFunction<Func<object, object, object, object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3], p[4], p[5], p[6]);
                            }
                            break;

                        case 8: {
                                var creator = CreateFunction<Func<object, object, object, object, object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7]);
                            }
                            break;

                        case 9: {
                                var creator = CreateFunction<Func<object, object, object, object, object, object, object, object, object, object>>();
                                Creator = p => creator(p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7], p[8]);
                            }
                            break;
                    }
                }

                if(Creator == null) {
                    Creator = ConstructorInfo.Invoke;
                }
            }

            return Creator(parameters);
        }

        #endregion
    }

    public sealed class DiDirtyMember
    {
        public DiDirtyMember(Type baseType, MemberInfo memberInfo, Type objectType)
        {
            BaseType = baseType;
            MemberInfo = memberInfo;
            ObjectType = objectType;
        }

        #region property

        public Type BaseType { get; }
        public MemberInfo MemberInfo { get; }
        public Type ObjectType { get; }

        #endregion
    }

    /// <summary>
    /// DI コンテナ。
    /// </summary>
    public class DiContainer : DisposerBase, IDiRegisterContainer
    {
        #region property

        /// <summary>
        /// シングルトンなDIコンテナ。
        /// <para><see cref="Initialize"/>にて初期化が必要。</para>
        /// </summary>
        public static IDiContainer Instance { get; private set; }

        /// <summary>
        /// IF → 実体 のマッピング。
        /// </summary>
        protected IDictionary<Type, Type> Mapping { get; } = new Dictionary<Type, Type>();
        /// <summary>
        /// 生成処理キャッシュ。
        /// </summary>
        protected IDictionary<Type, DiFactoryWorker> Factory { get; } = new Dictionary<Type, DiFactoryWorker>();
        /// <summary>
        /// コンストラクタキャッシュ。
        /// </summary>
        protected IDictionary<Type, DiConstructorCache> Constructors { get; } = new Dictionary<Type, DiConstructorCache>();
        protected IDictionary<Type, object> ObjectPool { get; } = new Dictionary<Type, object>();
        protected IList<DiDirtyMember> DirtyMembers { get; } = new List<DiDirtyMember>();

        #endregion

        #region function

        /// <summary>
        /// <see cref="Instance"/> を使用するための準備処理。
        /// </summary>
        /// <param name="creator"></param>
        public static void Initialize(Func<IDiContainer> creator)
        {
            if(Instance != null) {
                throw new InvalidOperationException();
            }

            Instance = creator();
        }

        protected virtual void RegisterFactoryCore(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            Mapping.Add(interfaceType, objectType);
            Factory.Add(interfaceType, new DiFactoryWorker(lifecycle, creator, this));
        }

        void RegisterFactorySingleton(Type interfaceType, Type objectType, DiCreator creator)
        {
            var lazy = new Lazy<object>(() => creator());
            RegisterFactoryCore(interfaceType, objectType, DiLifecycle.Singleton, () => lazy.Value);
        }

        protected virtual void SimpleRegister(Type interfaceType, Type objectType, object value)
        {
            Mapping.Add(interfaceType, objectType);
            ObjectPool.Add(interfaceType, value);
        }

        void Register(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!interfaceType.IsAssignableFrom(objectType)) {
                throw new ArgumentException($"error: {interfaceType} <- {objectType}");
            }

            switch(lifecycle) {
                case DiLifecycle.Transient:
                    RegisterFactoryCore(interfaceType, objectType, DiLifecycle.Transient, creator);
                    break;

                case DiLifecycle.Singleton:
                    RegisterFactorySingleton(interfaceType, objectType, creator);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        Type GetMappingType(Type type)
        {
            return Mapping.TryGetValue(type, out var objectType) ? objectType : type;
        }

        object[] CreateParameters(IReadOnlyList<ParameterInfo> parameterInfos, IEnumerable<object> manualParameters)
        {
            var manualParameterItems = manualParameters
                .Where(o => o != null)
                .Select(o => o.GetType() == typeof(DiDefaultParameter) ? ((DiDefaultParameter)o).GetPair() :  new KeyValuePair<Type, object>(o.GetType(), o))
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

                if(ObjectPool.TryGetValue(parameterInfo.ParameterType, out var poolValue)) {
                    arguments[i] = poolValue;
                } else if(Factory.TryGetValue(parameterInfo.ParameterType, out var factoryWorker)) {
                    arguments[i] = factoryWorker.Create();
                } else {
                    // どうしようもねぇ
                    return null;
                }
            }

            return arguments;

        }

        bool TryNewObjectCore(Type objectType, bool isCached, DiConstructorCache constructorCache, IEnumerable<object> manualParameters, out object createdObject)
        {
            var parameters = constructorCache.ParameterInfos;

            if(parameters.Count == 0) {
                if(!isCached) {
                    Constructors.Add(objectType, constructorCache);
                }
                createdObject = constructorCache.Create(null);
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
                Constructors.Add(objectType, constructorCache);
            }
            createdObject = constructorCache.Create(arguments);
            return true;
        }

        bool TryNewObject(Type objectType, IEnumerable<object> manualParameters, bool useFactoryCache, out object createdObject)
        {
            if(ObjectPool.TryGetValue(objectType, out var poolValue)) {
                createdObject = poolValue;
                return true;
            }

            if(useFactoryCache) {
                // 生成可能なものはこの段階で生成
                if(Factory.TryGetValue(objectType, out var factoryWorker)) {
                    createdObject = factoryWorker.Create();
                    return true;
                }
            }

            // コンストラクタのキャッシュを使用
            if(Constructors.TryGetValue(objectType, out var constructorCache)) {
                //NOTE: これ生成できなければ下の処理に流した方がいいと思う
                return TryNewObjectCore(objectType, true, constructorCache, manualParameters, out createdObject);
            }

            // 属性付きで引数が多いものを優先
            var constructorItems = objectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Select(c => new {
                    Constructor = c,
                    Parameters = c.GetParameters(),
                    Attribute = c.GetCustomAttribute<InjectionAttribute>()
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
                if(TryNewObjectCore(objectType, false, constructorItem, manualParameters, out createdObject)) {
                    return true;
                }
            }

            createdObject = default(object);
            return false;
        }

        object NewCore(Type type, IEnumerable<object> manualParameters, bool useFactoryCache)
        {
            if(ObjectPool.TryGetValue(type, out var poolValue)) {
                return poolValue;
            }

            if(TryNewObject(GetMappingType(type), manualParameters, useFactoryCache, out var raw)) {
                return raw;
            }

            throw new Exception($"{type}: create fail");
        }

        bool TryGetInstance(Type interfaceType, IEnumerable<object> manualParameters, out object value)
        {
            if(ObjectPool.TryGetValue(interfaceType, out var poolValue)) {
                value = poolValue;
                return true;
            }

            // 生成可能なものはこの段階で生成
            if(Factory.TryGetValue(interfaceType, out var factoryWorker)) {
                value = factoryWorker.Create();
                return true;
            }

            return TryNewObject(GetMappingType(interfaceType), manualParameters, true, out value);
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

        void SetMemberValue<TObject>(ref TObject target, MemberInfo memberInfo, Type valueType)
        {
            switch(memberInfo.MemberType) {
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberInfo;
                    if(TryGetInstance(valueType, Enumerable.Empty<object>(), out var fieldValue)) {
                        fieldInfo.SetValue(target, fieldValue);
                    } else {
                        throw new Exception($"{fieldInfo}: create fail");
                    }
                    break;

                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    if(TryGetInstance(valueType, Enumerable.Empty<object>(), out var propertyValue)) {
                        propertyInfo.SetValue(target, propertyValue);
                    } else {
                        throw new Exception($"{propertyInfo}: create fail");
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void InjectCore<TObject>(ref TObject target)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var targetType = GetMappingType(typeof(TObject));
            var memberItems = targetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Select(m => new { MemberInfo = m, IsInjectionTarget = m.GetCustomAttribute<InjectionAttribute>() != null })
                .ToList()
            ;
            foreach(var memberItem in memberItems.Where(i => i.IsInjectionTarget)) {
                SetMemberValue(ref target, memberItem.MemberInfo, GetMemberType(memberItem.MemberInfo));
            }

            // 強制付け替え処理の実施
            var dirtyPairs = memberItems
                .Where(i => !i.IsInjectionTarget)
                .Join(
                    DirtyMembers.Where(d => d.BaseType.IsAssignableFrom(targetType)),
                    m => m.MemberInfo.Name,
                    d => d.MemberInfo.Name,
                    (m, d) => new { Item = m, Dirty = d }
                )
            ;
            foreach(var pair in dirtyPairs) {
                SetMemberValue(ref target, pair.Item.MemberInfo, pair.Dirty.ObjectType);
            }

        }

        #endregion

        #region IDiContainer

        public object Get(Type interfaceType)
        {
            if(ObjectPool.TryGetValue(interfaceType, out var value)) {
                return value;
            }

            return Factory[interfaceType].Create();
        }

        public TInterface Get<TInterface>()
        {
            return (TInterface)Get(typeof(TInterface));
        }

        public object New(Type type, IEnumerable<object> manualParameters)
        {
            return NewCore(type, manualParameters, true);
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
            InjectCore(ref target);
        }

        public TObject Make<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var obj = New<TObject>(manualParameters);
            InjectCore(ref obj);
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

        #endregion

        #region IDiScopeContainerCreator

        public virtual IScopeDiContainer Scope()
        {
            var cloneContainer = new ScopeDiContainer();
            foreach(var pair in Mapping) {
                cloneContainer.Mapping.Add(pair.Key, pair.Value);
            }
            foreach(var pair in Factory) {
                cloneContainer.Factory.Add(pair.Key, pair.Value);
            }
            foreach(var pair in ObjectPool) {
                cloneContainer.ObjectPool.Add(pair.Key, pair.Value);
            }
            foreach(var pair in Constructors) {
                cloneContainer.Constructors.Add(pair.Key, pair.Value);
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
                Register(typeof(TInterface), typeof(TObject), lifecycle, () => NewCore(typeof(TObject), Enumerable.Empty<object>(), false));
            } else {
                Register(typeof(TInterface), typeof(TObject), lifecycle, () => NewCore(typeof(TObject), Enumerable.Empty<object>(), true));
            }

            return this;
        }

        public IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            Register(typeof(TInterface), typeof(TObject), lifecycle, creator);

            return this;
        }

        public IDiRegisterContainer Register<TInterface, TObject>(TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        {
            SimpleRegister(typeof(TInterface), typeof(TObject), value);

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


        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var factory in Factory.Values) {
                        factory.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    class ScopeDiContainer : DiContainer, IScopeDiContainer
    {
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

        protected override void RegisterFactoryCore(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!RegisteredTypeSet.Contains(interfaceType)) {
                Mapping.Remove(interfaceType);
                Factory.Remove(interfaceType);

                base.RegisterFactoryCore(interfaceType, objectType, lifecycle, creator);
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

        // 自動生成にしても日本語がすごい
        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if(IsDisposed) {
                if(disposing) {
                    foreach(var type in RegisteredTypeSet) {
                        Factory[type].Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public static class IDiContainerExtensions
    {
        #region function

        public static TObject Build<TObject>(this IDiContainer @this, params object[] parameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return @this.Make<TObject>(parameters);
        }

        #endregion
    }
}
