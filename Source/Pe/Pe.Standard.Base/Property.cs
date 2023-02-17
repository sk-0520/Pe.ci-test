#if DEBUG || BETA
#   define CHECK_PROPERTY_NAME
#endif

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// 汎用プロパティ取得処理。
    /// </summary>
    public interface IPropertyGetter
    {
        #region function

        /// <summary>
        /// 対象オブジェクトからプロパティの値取得。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <returns>値。</returns>
        object? Get(object owner);

        #endregion
    }

    /// <summary>
    /// 型指定プロパティ値取得処理。
    /// </summary>
    /// <typeparam name="TOwner">対象型。</typeparam>
    /// <typeparam name="TValue">プロパティ型</typeparam>
    public interface IPropertyGetter<in TOwner, out TValue>
    {
        #region function

        /// <summary>
        /// 対象オブジェクトからプロパティの値取得。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <returns>値。</returns>
        TValue Get(TOwner owner);

        #endregion
    }

    /// <summary>
    /// 汎用プロパティ設定処理。
    /// </summary>
    public interface IPropertySetter
    {
        #region function

        /// <summary>
        /// 対象オブジェクトのプロパティ値を設定。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <param name="value">値。</param>
        void Set(object owner, object? value);

        #endregion
    }

    /// <summary>
    /// 型指定プロパティ設定処理。
    /// </summary>
    /// <typeparam name="TOwner">対象型。</typeparam>
    /// <typeparam name="TValue">プロパティ型</typeparam>
    public interface IPropertySetter<in TOwner, in TValue>
    {
        #region function

        /// <summary>
        /// 対象オブジェクトのプロパティ値を設定。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <param name="value">値。</param>
        void Set(TOwner owner, TValue value);

        #endregion
    }

    internal static class PropertyExpressionFactory
    {
        #region function

        public static ParameterExpression CreateOwner(object owner) => Expression.Parameter(owner.GetType(), nameof(owner));
        public static ParameterExpression CreateOwner<T>() => Expression.Parameter(typeof(T), typeof(T).Name);

        public static Func<object, object?> CreateGetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);

            var lambda = Expression.Lambda(
                Expression.Convert(
                    property,
                    property.Type
                ),
                owner
            );

            var executor = lambda.Compile();
            return new Func<object, object?>(o => executor.DynamicInvoke(o));
        }
        public static Func<TOwner, TValue> CreateGetter<TOwner, TValue>(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);

            var lambda = Expression.Lambda<Func<TOwner, TValue>>(
                Expression.Convert(
                    property,
                    typeof(TValue)
                ),
                owner
            );
            return lambda.Compile();
        }

        public static Action<object, object?> CreateSetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            var value = Expression.Parameter(typeof(object), "value");

            var lambda = Expression.Lambda(
                Expression.Assign(
                    property,
                    Expression.Convert(value, property.Type)
                ),
                owner, value
            );

            var executor = lambda.Compile();
            return new Action<object, object?>((o, v) => executor.DynamicInvoke(o, v));
        }
        public static Action<TOwner, TValue> CreateSetter<TOwner, TValue>(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            var value = Expression.Parameter(typeof(TValue), "value");

            var lambda = Expression.Lambda<Action<TOwner, TValue>>(
                Expression.Assign(
                    property,
                    Expression.Convert(value, property.Type)
                ),
                owner, value
            );
            return lambda.Compile();
        }

        #endregion
    }

    /// <summary>
    /// 汎用プロパティアクセス処理。
    /// </summary>
    public class PropertyAccesser: IPropertyGetter, IPropertySetter
    {
        public PropertyAccesser(ParameterExpression ownerExpression, PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            if(PropertyInfo.CanWrite) {
                Setter = PropertyExpressionFactory.CreateSetter(ownerExpression, propertyInfo.Name);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyExpressionFactory.CreateGetter(ownerExpression, propertyInfo.Name);
        }
        public PropertyAccesser(object owner, string propertyName)
        {
            var ownerExpression = PropertyExpressionFactory.CreateOwner(owner);
            var propertyInfo = ownerExpression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(propertyInfo == null) {
                throw new ArgumentException($"{nameof(propertyName)}: {propertyName}");
            }
            PropertyInfo = propertyInfo;
            if(PropertyInfo.CanWrite) {
                Setter = PropertyExpressionFactory.CreateSetter(ownerExpression, propertyName);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyExpressionFactory.CreateGetter(ownerExpression, propertyName);
        }

        #region property

        public PropertyInfo PropertyInfo { get; }

        private Func<object, object?> Getter { get; }
        private Action<object, object?> Setter { get; }

        #endregion

        #region function

        private static void ThrowSetter(object owner, object? value) => throw new NotSupportedException();

        #endregion


        #region IPropertyGetter

        /// <inheritdoc cref="IPropertyGetter.Get(object)"/>
        public object? Get(object owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        /// <inheritdoc cref="IPropertySetter.Set(object, object?)"/>
        public void Set(object owner, object? value) => Setter(owner, value);

        #endregion
    }

    /// <summary>
    /// 型指定プロパティアクセス処理。
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PropertyAccesser<TOwner, TValue>: PropertyAccesser, IPropertyGetter<TOwner, TValue>, IPropertySetter<TOwner, TValue>
    {
        public PropertyAccesser([DisallowNull] TOwner owner, string propertyName)
            : base(owner, propertyName)
        {
            var ownerProperty = PropertyExpressionFactory.CreateOwner(owner);
            if(PropertyInfo.CanWrite) {
                Setter = PropertyExpressionFactory.CreateSetter<TOwner, TValue>(ownerProperty, propertyName);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyExpressionFactory.CreateGetter<TOwner, TValue>(ownerProperty, propertyName);
        }

        #region property

        private Func<TOwner, TValue> Getter { get; }
        private Action<TOwner, TValue> Setter { get; }

        #endregion

        #region function

        private static void ThrowSetter(TOwner owner, TValue value) => throw new NotSupportedException();

        #endregion

        #region IPropertyGetter

        /// <inheritdoc cref="IPropertyGetter{TOwner, TValue}.Get(TOwner)"/>
        public TValue Get(TOwner owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        /// <inheritdoc cref="IPropertySetter{TOwner, TValue}.Set(TOwner, TValue)"/>
        public void Set(TOwner owner, TValue value) => Setter(owner, value);

        #endregion

    }

    /// <summary>
    /// プロパティアクセス処理生成。
    /// </summary>
    public static class PropertyAccesserFactory
    {
        #region function

        /// <summary>
        /// 汎用版を生成。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyAccesser Create(object owner, string propertyName) => new PropertyAccesser(owner, propertyName);
        /// <summary>
        /// ジェネリック版を生成。
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="owner"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyAccesser<TOwner, TValue> Create<TOwner, TValue>([DisallowNull] TOwner owner, string propertyName) => new PropertyAccesser<TOwner, TValue>(owner, propertyName);

        #endregion
    }

    /// <summary>
    /// プロパティアクセス処理キャッシュ。
    /// </summary>
    public class PropertyCacher
    {
        /// <summary>
        /// 指定オブジェクトに対してプロパティアクセス処理のキャッシュを構築。
        /// </summary>
        /// <param name="owner">プロパティアクセスを行うオブジェクト。</param>
        public PropertyCacher(object owner)
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

        private ConcurrentDictionary<string, PropertyAccesser> AccesserCache { get; } = new ConcurrentDictionary<string, PropertyAccesser>(); // CPUの数とかこの層で取得するのしんどいのでコンストラクタでキャパ指定せず。

        #endregion

        #region function

        private PropertyAccesser GetAccessor(string propertyName) => AccesserCache.GetOrAdd(propertyName, s => {
            var propertyInfo = Properties[s];
            return new PropertyAccesser(OwnerExpression, propertyInfo);
        });

        public object? Get(string propertyName)
        {
#if CHECK_PROPERTY_NAME
            if(Properties.TryGetValue(propertyName, out var prop)) {
                if(!prop.CanRead) {
                    throw new ArgumentException(nameof(propertyName));
                }
            } else {
                throw new ArgumentException(nameof(propertyName));
            }
#endif
            var accessor = GetAccessor(propertyName);
            return accessor.Get(Owner);
        }

        public void Set(string propertyName, object? value)
        {
#if CHECK_PROPERTY_NAME
            if(Properties.TryGetValue(propertyName, out var prop)) {
                if(!prop.CanWrite) {
                    throw new ArgumentException(nameof(propertyName));
                }
            } else {
                throw new ArgumentException(nameof(propertyName));
            }
#endif
            var accessor = GetAccessor(propertyName);
            accessor.Set(Owner, value);
        }

        #endregion
    }

}
