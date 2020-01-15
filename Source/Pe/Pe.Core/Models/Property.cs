using System;
using System.Linq.Expressions;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IPropertyGetter
    {
        #region function

        object Get(object owner);

        #endregion
    }

    public interface IPropertyGetter<TOwner, TValue>
    {
        #region function

        TValue Get(TOwner target);

        #endregion
    }

    public interface IPropertySetter
    {
        #region function

        void Set(object owner, object value);

        #endregion
    }

    public interface IPropertySetter<TOwner, TValue>
    {
        #region function

        void Set(TOwner owner, TValue value);

        #endregion
    }

    public static class PropertyFactory
    {
        #region function

        public static ParameterExpression CreateOwner(object owner) => Expression.Parameter(owner.GetType(), nameof(owner));
        public static ParameterExpression CreateOwner<T>() => Expression.Parameter(typeof(T), typeof(T).Name);

        public static Func<object, object> CreateGetter(ParameterExpression owner, string propertyName)
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
            return new Func<object, object>(o => executor.DynamicInvoke(o)!);
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

        public static Action<object, object> CreateSetter(ParameterExpression owner, string propertyName)
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
            return new Action<object, object>((o, v) => executor.DynamicInvoke(o, v));
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

    public class PropertyAccesser : IPropertyGetter, IPropertySetter
    {
        public PropertyAccesser(object owner, string propertyName)
        {
            var ownerProperty = PropertyFactory.CreateOwner(owner);
            Getter = PropertyFactory.CreateGetter(ownerProperty, propertyName);
            Setter = PropertyFactory.CreateSetter(ownerProperty, propertyName);
        }

        #region property

        Func<object, object> Getter { get; }
        Action<object, object> Setter { get; }

        #endregion


        #region IPropertyGetter

        public object Get(object owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        public void Set(object owner, object value) => Setter(owner, value);

        #endregion
    }

    public class PropertyAccesser<TOwner, TValue> : PropertyAccesser, IPropertyGetter<TOwner, TValue>, IPropertySetter<TOwner, TValue>
    {
        public PropertyAccesser(TOwner owner, string propertyName)
            : base(owner!, propertyName)
        {
            var ownerProperty = PropertyFactory.CreateOwner(owner!);
            Getter = PropertyFactory.CreateGetter<TOwner, TValue>(ownerProperty, propertyName);
            Setter = PropertyFactory.CreateSetter<TOwner, TValue>(ownerProperty, propertyName);
        }

        #region property

        Func<TOwner, TValue> Getter { get; }
        Action<TOwner, TValue> Setter { get; }

        #endregion

        #region IPropertyGetter

        public TValue Get(TOwner owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        public void Set(TOwner owner, TValue value) => Setter(owner, value);

        #endregion

    }

}
