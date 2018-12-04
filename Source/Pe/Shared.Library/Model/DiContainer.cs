using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

    public interface IDiContainer
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
        /// 限定的なDIコンテナを作成。
        /// </summary>
        /// <returns>現在マッピングを複製したDIコンテナ。</returns>
        IScopeDiContainer Scope();
    }

    public interface IDiRegisterContainer : IDiContainer
    {
        /// <summary>
        /// シンプルなマッピングを追加。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        void Register<TInterface, TObject>(DiLifecycle lifecycle = DiLifecycle.Transient)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <summary>
        /// 自分で作る版のマッピング。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="creator"></param>
        /// <param name="lifecycle"></param>
        void Register<TInterface, TObject>(DiCreator creator, DiLifecycle lifecycle = DiLifecycle.Transient)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
    }

    /// <summary>
    /// 限定的なDIコンテナ。
    /// </summary>
    public interface IScopeDiContainer : IDiRegisterContainer, IDisposable
    { }

    /// <summary>
    /// マッピング生成処理。
    /// </summary>
    /// <returns></returns>
    public delegate object DiCreator();

    /// <summary>
    /// マッピング生成キャッシュ。
    /// </summary>
    public sealed class DiFactoryWorker
    {
        public DiFactoryWorker(DiLifecycle lifecycle, DiCreator creator, object bind)
        {
            Lifecycle = lifecycle;
            Creator = creator;
        }

        #region property

        public DiLifecycle Lifecycle { get; }
        DiCreator Creator { get; }

        #endregion

        #region function

        public object Create() => Creator();

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

    /// <summary>
    /// DI コンテナ。
    /// </summary>
    public class DiContainer : IDiRegisterContainer
    {
        #region property

        /// <summary>
        /// シングルトンなDIコンテナ。
        /// <para><see cref="Initialize"/>にて初期化が必要。</para>
        /// </summary>
        public static IDiContainer Current { get; private set; }

        /// <summary>
        /// 具象化 → 実体 のマッピング。
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

        #endregion

        #region function

        public static void Initialize(Func<IDiContainer> creator)
        {
            if(Current != null) {
                throw new InvalidOperationException();
            }

            Current = creator();
        }

        protected virtual void RegisterCore(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            Mapping.Add(interfaceType, objectType);
            Factory.Add(interfaceType, new DiFactoryWorker(lifecycle, creator, this));
        }

        void RegisterSingleton(Type interfaceType, Type objectType, DiCreator creator)
        {
            var lazy = new Lazy<object>(() => creator());
            RegisterCore(interfaceType, objectType, DiLifecycle.Singleton, () => lazy.Value);
        }

        void Register(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            switch(lifecycle) {
                case DiLifecycle.Transient:
                    RegisterCore(interfaceType, objectType, DiLifecycle.Transient, creator);
                    break;

                case DiLifecycle.Singleton:
                    RegisterSingleton(interfaceType, objectType, creator);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        Type GetMappingType(Type type)
        {
            return Mapping.TryGetValue(type, out var objectType) ? objectType : type;
        }

        IList<object> CreateParameters(IReadOnlyCollection<ParameterInfo> parameterInfos, IEnumerable<object> manualParameters)
        {
            var manualParameterItems = manualParameters
                .Where(o => o != null)
                .Select(o => new KeyValuePair<Type, object>(o.GetType(), o))
                .ToList()
            ;

            var arguments = new List<object>(parameterInfos.Count);
            foreach(var parameterInfo in parameterInfos) {
                // 入力パラメータを優先して設定
                if(manualParameterItems.Count != 0) {
                    var item = manualParameterItems.FirstOrDefault(i => i.Key == parameterInfo.ParameterType);
                    if(item.Key != default(Type)) {
                        arguments.Add(item.Value);
                        manualParameterItems.Remove(item);
                        continue;
                    }
                }

                if(Factory.TryGetValue(parameterInfo.ParameterType, out var factoryWorker)) {
                    arguments.Add(factoryWorker.Create());
                }  else {
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
            if(arguments.Count != parameters.Count) {
                createdObject = default(object);
                return false;
            }

            if(!isCached) {
                Constructors.Add(objectType, constructorCache);
            }
            createdObject = constructorCache.Create(arguments.ToArray());
            return true;
        }

        bool TryNewObject(Type objectType, IEnumerable<object> manualParameters, out object createdObject)
        {
            // 生成可能なものはこの段階で生成
            if(Factory.TryGetValue(objectType, out var factoryWorker)) {
                createdObject = factoryWorker.Create();
                return true;
            }

            // コンストラクタのキャッシュを使用
            if(Constructors.TryGetValue(objectType, out var constructorCache)) {
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

        bool TryGetInstance(Type interfaceType, IEnumerable<object> manualParameters, out object value)
        {
            // 生成可能なものはこの段階で生成
            if(Factory.TryGetValue(interfaceType, out var factoryWorker)) {
                value = factoryWorker.Create();
                return true;
            }

            return TryNewObject(GetMappingType(interfaceType), manualParameters, out value);
        }

        void InjectCore<TObject>(ref TObject target)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var members = typeof(TObject).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            foreach(var member in members.Where(m => m.GetCustomAttribute<InjectionAttribute>() != null)) {
                switch(member.MemberType) {
                    case MemberTypes.Field:
                        var fieldInfo = (FieldInfo)member;
                        if(TryGetInstance(fieldInfo.FieldType, Enumerable.Empty<object>(), out var fieldValue)) {
                            fieldInfo.SetValue(target, fieldValue);
                        } else {
                            throw new Exception($"{fieldInfo}: create fail");
                        }
                        break;

                    case MemberTypes.Property:
                        var propertyInfo = (PropertyInfo)member;
                        if(TryGetInstance(propertyInfo.PropertyType, Enumerable.Empty<object>(), out var propertyValue)) {
                            propertyInfo.SetValue(target, propertyValue);
                        } else {
                            throw new Exception($"{propertyInfo}: create fail");
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region IDependencyInjectionContainer

        public TInterface Get<TInterface>()
        {
            return (TInterface)Factory[typeof(TInterface)].Create();
        }

        public TObject New<TObject>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            if(TryNewObject(GetMappingType(typeof(TObject)), manualParameters, out var raw)) {
                return (TObject)raw;
            }

            throw new Exception($"{typeof(TObject)}: create fail");
        }

        public TObject New<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return New<TObject>(Enumerable.Empty<object>());
        }

        public void Inject<TObject>(TObject target)
            where TObject : class
        {
            InjectCore(ref target);
        }

#if ENABLED_STRUCT
        public void Inject<TObject>(ref TObject target)
            where TObject : struct
        {
            InjectCore(ref target);
        }
#endif

        public virtual IScopeDiContainer Scope()
        {
            var cloneContainer = new ScopeDiContainer();
            foreach(var pair in Mapping) {
                cloneContainer.Mapping.Add(pair.Key, pair.Value);
            }
            foreach(var pair in Factory) {
                cloneContainer.Factory.Add(pair.Key, pair.Value);
            }
            foreach(var pair in Constructors) {
                cloneContainer.Constructors.Add(pair.Key, pair.Value);
            }

            return cloneContainer;
        }

        #endregion

        #region IDiAddContainer

        public void Register<TInterface, TObject>(DiLifecycle lifecycle = DiLifecycle.Transient)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            Register(typeof(TInterface), typeof(TObject), lifecycle, New<TObject>);
        }

        public void Register<TInterface, TObject>(DiCreator creator, DiLifecycle lifecycle = DiLifecycle.Transient)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            Register(typeof(TInterface), typeof(TObject), lifecycle, creator);
        }

        #endregion
    }

    class ScopeDiContainer : DiContainer, IScopeDiContainer
    {
        #region property

        HashSet<Type> RegisteredTypeSet { get; } = new HashSet<Type>();

        #endregion

        #region DependencyInjectionContainer

        protected override void RegisterCore(Type interfaceType, Type objectType, DiLifecycle lifecycle, DiCreator creator)
        {
            if(!RegisteredTypeSet.Contains(interfaceType)) {
                Mapping.Remove(interfaceType);
                Factory.Remove(interfaceType);

                base.RegisterCore(interfaceType, objectType, lifecycle, creator);
                RegisteredTypeSet.Add(interfaceType);
            } else {
                throw new ArgumentException(nameof(interfaceType));
            }
        }

        #endregion

        // 自動生成にしても日本語がすごい
        #region IDisposable Support

        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
                if(disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                this.disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~ScopeDependencyInjectionContainer() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
