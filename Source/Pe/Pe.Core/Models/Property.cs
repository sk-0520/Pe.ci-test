using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IPropertyGetter
    {
        #region function

        object Get();

        #endregion
    }

    public interface IPropertyGetter<T>
    {
        #region function

        T Get();

        #endregion
    }

    public interface IPropertySetter
    {
        #region function

        void Set(object value);

        #endregion
    }

    public interface IPropertySetter<T>
    {
        #region function

        void Set(T value);

        #endregion
    }

    public static class PropertyFactory
    {
        #region function

        public static ParameterExpression CreateOwner(object owner) => Expression.Parameter(owner.GetType(), nameof(owner));
        public static ParameterExpression CreateOwner<T>() => Expression.Parameter(typeof(T), typeof(T).Name);

        public static MemberExpression GetProperty(ParameterExpression owner, string propertyName) => Expression.PropertyOrField(owner, propertyName);

        public static Func<T> CreateGetter<T>(MemberExpression property)
        {
            var lambda = Expression.Lambda<Func<T>>(
                Expression.Convert(
                    property,
                    property.Type
                )
            );
            return lambda.Compile();
        }

        public static Action<T> CreateSetter<T>(MemberExpression property)
        {
            var lambda = Expression.Lambda<Action<T>>(
                Expression.Assign(
                    property,
                    Expression.Convert(
                        Expression.Parameter(typeof(T), "value"),
                        typeof(T)
                    )
                )
            );
            return lambda.Compile();
        }

        #endregion
    }

}
