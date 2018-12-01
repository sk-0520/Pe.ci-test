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

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = false)]
    public class DiInjectionAttribute : Attribute
    { }

    /// <summary>
    ///
    /// </summary>
    public class DependencyInjectionContainer
    {
        #region property

        public static DependencyInjectionContainer Current { get; } = new DependencyInjectionContainer();

        Dictionary<Type, Type> Mapping { get; } = new Dictionary<Type, Type>();
        Dictionary<Type, Func<object>> Factory { get; } = new Dictionary<Type, Func<object>>();

        #endregion

        #region function

        void AddCreateCore(Type interfaceType, Type objectType, Func<object> factory)
        {
            Mapping.Add(interfaceType, objectType);
            Factory.Add(interfaceType, factory);
        }

        void AddSingletonCore(Type interfaceType, Type objectType, Func<object> factory)
        {
            var lazy = new Lazy<object>(factory);
            Mapping.Add(interfaceType, objectType);
            Factory.Add(interfaceType, () => lazy.Value);
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

        /// <summary>
        /// シンプルなマッピングを追加。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        public void Add<TInterface, TObject>(DiLifecycle lifecycle = DiLifecycle.Create)
            where TObject : new()
        {
            AddCore(typeof(TInterface), typeof(TObject), lifecycle, () => new TObject());
        }

        /// <summary>
        /// 自分で作る版のマッピング。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lifecycle"></param>
        /// <param name="factory"></param>
        public void Add<TInterface, TObject>(DiLifecycle lifecycle, Func<object> factory)
        {
            AddCore(typeof(TInterface), typeof(TObject), lifecycle, factory);
        }

        object GetCore(Type interfaceType)
        {
            return Factory[interfaceType]();
        }

        /// <summary>
        /// 登録されているオブジェクトを取得。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Get<TInterface>()
        {
            var result = GetCore(typeof(TInterface));
            return (TInterface)result;
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
                } else {
                    // コンテナ内に存在しない場合は入力パラメータを順番に使用する
                    var item = manualParameterItems.FirstOrDefault(i => i.Key == parameterInfo.ParameterType);
                    if(item.Key == default(Type)) {
                        // 無かったのでこのパラメータは生成できない
                        return null;
                    }
                    arguments.Add(item.Value);
                    manualParameterItems.Remove(item);
                }
            }

            return arguments;

        }

        bool NewCore<T>(ConstructorInfo constructor, IEnumerable<object> manualParameters, out T createdObject)
        {
            var parameters = constructor.GetParameters();

            if(!parameters.Any()) {
                createdObject = (T)constructor.Invoke(null);
                return true;
            }

            var arguments = CreateParameters(parameters, manualParameters);
            if(arguments == null) {
                createdObject = default(T);
                return false;
            }
            if(arguments.Count != parameters.Length) {
                createdObject = default(T);
                return false;
            }

            createdObject = (T)constructor.Invoke(arguments.ToArray());
            return true;
        }

        public T New<T>(IEnumerable<object> manualParameters)
        {
            var type = typeof(T);

            // 引数が多いものを優先
            var constructorItems = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Select(c => new {
                    Constructor = c,
                    Parameters = c.GetParameters(),
                    Attribute = c.GetCustomAttributes(typeof(DiInjectionAttribute), false).OfType<DiInjectionAttribute>().FirstOrDefault()
                })
                .Where(i => i.Attribute != null ? true: i.Constructor.IsPublic)
                .OrderBy(i => i.Attribute != null ? 0 : 1)
                .ThenByDescending(i => i.Parameters.Length)
                .ToList()
            ;

            foreach(var constructorItem in constructorItems) {
                if(NewCore<T>(constructorItem.Constructor, manualParameters, out var obj)) {
                    return obj;
                }
            }

            throw new Exception($"{typeof(T)}: create fail");
        }

        public T New<T>()
        {
            return New<T>(Enumerable.Empty<object>());
        }


        #endregion
    }
}
