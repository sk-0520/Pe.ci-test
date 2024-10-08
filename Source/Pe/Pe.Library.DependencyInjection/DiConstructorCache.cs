using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// コンストラクタ情報キャッシュ。
    /// </summary>
    public sealed partial class DiConstructorCache
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="constructorInfo">コンストラクタ情報。</param>
        /// <param name="parameterInfoItems">コンストラクタのパラメータ。</param>
        public DiConstructorCache(ConstructorInfo constructorInfo, IReadOnlyList<ParameterInfo> parameterInfoItems)
        {
            ConstructorInfo = constructorInfo;
            if(ConstructorInfo.ReflectedType is null) {
                throw new ArgumentNullException($"{nameof(constructorInfo)}.{nameof(ConstructorInfo.ReflectedType)}");
            }
            ParameterInfoItems = parameterInfoItems;

            var map = new Dictionary<ParameterInfo, DiInjectionAttribute>();
            foreach(var parameterInfo in ParameterInfoItems) {
                var attr = parameterInfo.GetCustomAttribute<DiInjectionAttribute>();
                if(attr != null) {
                    map.Add(parameterInfo, attr);
                }
            }
            ParameterInjections = map;
        }

        #region property

        public ConstructorInfo ConstructorInfo { get; }
        public IReadOnlyList<ParameterInfo> ParameterInfoItems { get; }
        public IReadOnlyDictionary<ParameterInfo, DiInjectionAttribute> ParameterInjections { get; }
        private Func<object?[], object>? Creator { get; set; }

        #endregion

        #region function

        private IEnumerable<ParameterExpression> CreateParameterExpressions()
        {
            return ParameterInfoItems
                .Select((p, i) => Expression.Parameter(typeof(object), p.Name))
            ;
        }

        private IEnumerable<UnaryExpression> CreateConvertExpressions(IEnumerable<ParameterExpression> parameterExpressions)
        {
            return ParameterInfoItems
                .Zip(parameterExpressions, (pi, pe) => Expression.Convert(pe, pi.ParameterType))
            ;
        }

        private FuncN CreateFunction<FuncN>()
        {
            var parameterExpressions = CreateParameterExpressions().ToList();
            var convertExpressions = CreateConvertExpressions(parameterExpressions).ToList();
            Debug.Assert(parameterExpressions.Count == convertExpressions.Count);

            var constructorNewParams = Expression.Lambda<FuncN>(
                Expression.Convert(
                    Expression.New(
                        ConstructorInfo,
                        convertExpressions
                    ),
                    typeof(object)
                ),
                ConstructorInfo.ReflectedType!.FullName + "_" + ParameterInfoItems.Count.ToString(CultureInfo.InvariantCulture),
                parameterExpressions
            );
            var creator = constructorNewParams.Compile();

            return creator;
        }

        /// <summary>
        /// コンストラクタの呼び出し。
        /// </summary>
        /// <remarks>
        /// <para>内部実装は DiConstructorCacheImpl.tt にて機械生成。</para>
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Create(object?[] parameters)
        {
            if(Creator == null) {
                if(ParameterInfoItems.Count == 0) {
                    var newExp = Expression.New(ConstructorInfo);
                    var lambda = Expression.Lambda<Func<object>>(newExp);
                    var creator = lambda.Compile();
                    Creator = p => creator();
                } else {
                    Creator = CreateCore(parameters);
                }
            }

            return Creator(parameters);
        }

        #endregion
    }
}
