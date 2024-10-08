using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Property
{
    /// <summary>
    /// 汎用プロパティアクセス処理。
    /// </summary>
    public class PropertyAccessor: IPropertyGetter, IPropertySetter
    {
        public PropertyAccessor(ParameterExpression ownerExpression, PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            if(PropertyInfo.CanWrite) {
                Setter = PropertyExpressionFactory.CreateSetter(ownerExpression, propertyInfo.Name);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyExpressionFactory.CreateGetter(ownerExpression, propertyInfo.Name);
        }
        public PropertyAccessor(object owner, string propertyName)
        {
            var ownerExpression = PropertyExpressionFactory.CreateOwner(owner);
            var propertyInfo = ownerExpression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(propertyInfo == null) {
                throw new PropertyNotFoundException($"{nameof(propertyName)}: {propertyName}");
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
    public class PropertyAccessor<TOwner, TValue>: PropertyAccessor, IPropertyGetter<TOwner, TValue>, IPropertySetter<TOwner, TValue>
    {
        public PropertyAccessor([DisallowNull] TOwner owner, string propertyName)
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

}
