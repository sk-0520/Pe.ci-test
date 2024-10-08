#if DEBUG || BETA
#   define CHECK_PROPERTY_NAME
#endif

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Property
{
    /// <summary>
    /// プロパティアクセス処理キャッシュ。
    /// </summary>
    public class CachedProperty
    {
        /// <summary>
        /// 指定オブジェクトに対してプロパティアクセス処理のキャッシュを構築。
        /// </summary>
        /// <param name="owner">プロパティアクセスを行うオブジェクト。</param>
        public CachedProperty(object owner)
        {
            Owner = owner;
            OwnerExpression = PropertyExpressionFactory.CreateOwner(Owner);
            Properties = OwnerExpression.Type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToDictionary(
                    k => k.Name,
                    v => v
                )
            ;
        }

        #region property

        private object Owner { get; }

        private ParameterExpression OwnerExpression { get; }

        private IReadOnlyDictionary<string, PropertyInfo> Properties { get; }

        private ConcurrentDictionary<string, PropertyAccessor> AccessorCache { get; } = new ConcurrentDictionary<string, PropertyAccessor>(); // CPUの数とかこの層で取得するのしんどいのでコンストラクタでキャパ指定せず。

        #endregion

        #region function

        private PropertyAccessor GetAccessor(string propertyName) => AccessorCache.GetOrAdd(propertyName, s => {
            if(!Properties.TryGetValue(s, out var accessor)) {
                throw new PropertyNotFoundException(s);
            }
            var propertyInfo = Properties[s];
            return new PropertyAccessor(OwnerExpression, propertyInfo);
        });

        public object? Get(string propertyName)
        {
            var accessor = GetAccessor(propertyName);
            return accessor.Get(Owner);
        }

        public void Set(string propertyName, object? value)
        {
            var accessor = GetAccessor(propertyName);
            accessor.Set(Owner, value);
        }

        #endregion
    }

}
