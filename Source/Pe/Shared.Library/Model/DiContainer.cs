using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// 勉強がてら作ってみる。

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public enum DiLifecycle
    {
        /// <summary>
        /// 毎回作る。
        /// </summary>
        Create,
        /// <summary>
        /// シングルトン。
        /// </summary>
        Singleton,
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectionAttribute : Attribute
    { }

    public interface IDiContainer
    {
        /// <summary>
        /// シンプルなマッピングを追加。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        void Add<TInterface, TObject>(DiLifecycle lifecycle = DiLifecycle.Create)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <summary>
        /// 自分で作る版のマッピング。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifecycle"></param>
        void Add<TInterface, TObject>(Func<TObject> factory, DiLifecycle lifecycle = DiLifecycle.Create)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        TInterface Get<TInterface>();

        T New<T>(IEnumerable<object> manualParameters)
#if !ENABLED_STRUCT
            where T : class
#endif
        ;

        T New<T>()
#if !ENABLED_STRUCT
            where T : class
#endif
        ;

        void Inject<T>(T target)
            where T : class
        ;
#if ENABLED_STRUCT
        void Inject<T>(ref T target)
            where T : struct
        ;
#endif

        IScopeDiContainer Scope();
    }

    public interface IScopeDiContainer: IDiContainer, IDisposable
    { }

    /// <summary>
    ///
    /// </summary>
    public class DiContainer: IDiContainer
    {
        #region property

        public static IDiContainer Current { get; } = new DiContainer();

        protected IDictionary<Type, Type> Mapping { get; } = new Dictionary<Type, Type>();
        protected IDictionary<Type, Func<object>> Factory { get; } = new Dictionary<Type, Func<object>>();

        #endregion

        #region function

        protected virtual void AddCreateCore(Type interfaceType, Type objectType, Func<object> factory)
        {
            Mapping.Add(interfaceType, objectType);
            Factory.Add(interfaceType, factory);
        }

        void AddSingletonCore(Type interfaceType, Type objectType, Func<object> factory)
        {
            var lazy = new Lazy<object>(factory);
            AddCreateCore(interfaceType, objectType, () => lazy.Value);
        }

        void AddCore(Type interfaceType, Type objectType, DiLifecycle lifecycle, Func<object> factory)
        {

            switch(lifecycle) {
                case DiLifecycle.Create:
                    AddCreateCore(interfaceType, objectType, factory);
                    break;

                case DiLifecycle.Singleton:
                    AddSingletonCore(interfaceType, objectType, factory);
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
            var manualParameterItems = new LinkedList<KeyValuePair<Type, object>>(
                manualParameters.Select(o => new KeyValuePair<Type, object>(o.GetType(), o))
            );

            var arguments = new List<object>(parameterInfos.Count);
            foreach(var parameterInfo in parameterInfos) {
                if(Factory.TryGetValue(parameterInfo.ParameterType, out var factory)) {
                    arguments.Add(factory());
                } else if(manualParameterItems.Any()) {
                    // コンテナ内に存在しない場合は入力パラメータを順番に使用する
                    var item = manualParameterItems.FirstOrDefault(i => i.Key == parameterInfo.ParameterType);
                    if(item.Key == default(Type)) {
                        // 無かったのでこのパラメータは生成できない
                        return null;
                    }
                    arguments.Add(item.Value);
                    manualParameterItems.Remove(item);
                } else {
                    // どうしようもねぇ
                    return null;
                }
            }

            return arguments;

        }

        bool TryNewObjectCore(ConstructorInfo constructor, IEnumerable<object> manualParameters, out object createdObject)
        {
            var parameters = constructor.GetParameters();

            if(!parameters.Any()) {
                createdObject = constructor.Invoke(null);
                return true;
            }

            var arguments = CreateParameters(parameters, manualParameters);
            if(arguments == null) {
                createdObject = default(object);
                return false;
            }
            if(arguments.Count != parameters.Length) {
                createdObject = default(object);
                return false;
            }

            createdObject = constructor.Invoke(arguments.ToArray());
            return true;
        }

        bool TryNewObject(Type objectType, IEnumerable<object> manualParameters, out object value)
        {
            // 生成可能なものはこの段階で生成
            if(Factory.TryGetValue(objectType, out var factory)) {
                value = factory();
                return true;
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
                .ToList()
            ;

#if false
            if(!constructorItems.Any() && type.IsValueType) {
                //TODO: 構造体っぽければそのまま作る
            }
#endif

            foreach(var constructorItem in constructorItems) {
                if(TryNewObjectCore(constructorItem.Constructor, manualParameters, out value)) {
                    return true;
                }
            }

            value = default(object);
            return false;
        }

        bool TryGetInstance(Type interfaceType, IEnumerable<object> manualParameters, out object value)
        {
            // 生成可能なものはこの段階で生成
            if(Factory.TryGetValue(interfaceType, out var factory)) {
                value = factory();
                return true;
            }

            return TryNewObject(GetMappingType(interfaceType), manualParameters, out value);
        }

        void InjectCore<T>(ref T target)
#if !ENABLED_STRUCT
            where T : class
#endif
        {
            var members = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
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

        public void Add<TInterface, TObject>(DiLifecycle lifecycle = DiLifecycle.Create)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            AddCore(typeof(TInterface), typeof(TObject), lifecycle, () => {
                return New<TObject>();
            });
        }

        public void Add<TInterface, TObject>(Func<TObject> factory, DiLifecycle lifecycle = DiLifecycle.Create)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            AddCore(typeof(TInterface), typeof(TObject), lifecycle, factory);
        }

        public TInterface Get<TInterface>()
        {
            return (TInterface)Factory[typeof(TInterface)]();
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

        public T New<T>()
#if !ENABLED_STRUCT
            where T: class
#endif
        {
            return New<T>(Enumerable.Empty<object>());
        }

        public void Inject<T>(T target)
            where T : class
        {
            InjectCore(ref target);
        }

#if ENABLED_STRUCT
        public void Inject<T>(ref T target)
            where T : struct
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

            return cloneContainer;
        }

        #endregion
    }

    class ScopeDiContainer: DiContainer, IScopeDiContainer
    {
        #region property

        HashSet<Type> RegisteredTypeSet { get; } = new HashSet<Type>();

        #endregion

        #region DependencyInjectionContainer

        protected override void AddCreateCore(Type interfaceType, Type objectType, Func<object> factory)
        {
            if(!RegisteredTypeSet.Contains(interfaceType)) {
                Mapping.Remove(interfaceType);
                Factory.Remove(interfaceType);

                base.AddCreateCore(interfaceType, objectType, factory);
                RegisteredTypeSet.Add(interfaceType);
            } else {
                throw new ArgumentException(nameof(interfaceType));
            }
        }

        #endregion

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
