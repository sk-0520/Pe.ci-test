using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Property
{
    internal static class PropertyExpressionFactory
    {
        #region function

        public static ParameterExpression CreateOwner(object owner) => Expression.Parameter(owner.GetType(), nameof(owner));
        public static ParameterExpression CreateOwner<T>() => Expression.Parameter(typeof(T), typeof(T).Name);

        private static void ThrowIfCanNotRead(object owner, MemberExpression member)
        {
            switch(member.Member) {
                case PropertyInfo info:
                    if(!info.CanRead) {
                        throw new PropertyCanNotReadException(owner.GetType(), member.Member.Name);
                    }
                    break;

                case FieldInfo:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static Func<object, object?> CreateGetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            ThrowIfCanNotRead(owner, property);

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
            ThrowIfCanNotRead(owner, property);

            var lambda = Expression.Lambda<Func<TOwner, TValue>>(
                Expression.Convert(
                    property,
                    typeof(TValue)
                ),
                owner
            );
            return lambda.Compile();
        }

        private static void ThrowIfCanNotWrite(object owner, MemberExpression member)
        {
            switch(member.Member) {
                case PropertyInfo info:
                    if(!info.CanWrite) {
                        throw new PropertyCanNotWriteException(owner.GetType(), member.Member.Name);
                    }
                    break;

                case FieldInfo info:
                    if(info.IsInitOnly) {
                        throw new PropertyCanNotWriteException(owner.GetType(), member.Member.Name);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static Action<object, object?> CreateSetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            ThrowIfCanNotWrite(owner, property);
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
            ThrowIfCanNotWrite(owner, property);
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
}
