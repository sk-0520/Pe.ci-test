using System;
using System.Collections.Generic;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Standard.DependencyInjection
{
    public static class IDiContainerExtensions
    {
        #region function

        private static List<object> JoinParameters(object manualParameter, object[] manualParameters)
        {
            var parameters = new List<object>(1 + manualParameters.Length) {
                manualParameter,
            };
            parameters.AddRange(manualParameters);

            return parameters;
        }

        /// <summary>
        /// <see cref="IDiContainer.New{TObject}(IEnumerable{object})"/> して <see cref="IDiContainer.Inject{TObject}(TObject)"/> する。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="diContainer"></param>
        /// <param name="manualParameter"></param>
        /// <param name="manualParameters"></param>
        /// <returns></returns>
        public static TObject Build<TObject>(this IDiContainer diContainer, object manualParameter, params object[] manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var parameters = JoinParameters(manualParameter, manualParameters);

            var obj = diContainer.New<TObject>(parameters);
            diContainer.Inject(obj);
            return obj;
        }

        /// <inheritdoc cref="Build{TObject}(IDiContainer, object, object[])"/>
        public static TObject Build<TObject>(this IDiContainer diContainer)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var obj = diContainer.New<TObject>();
            diContainer.Inject(obj);
            return obj;
        }

        private static object? CallCore(IDiContainer diContainer, object instance, string methodName, IReadOnlyList<object> manualParameters)
        {
            var methodInfo = instance.GetType().GetMethod(methodName);
            if(methodInfo is null) {
                throw new DiException($"{nameof(methodInfo)}: {methodName}");
            }

            var obj = diContainer.CallMethod(string.Empty, instance, methodInfo, manualParameters);

            return obj;
        }

        public static TResult Call<TResult>(this IDiContainer diContainer, object instance, string methodName, object manualParameter, params object[] manualParameters)
        {
            var parameters = JoinParameters(manualParameter, manualParameters);

#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return (TResult)CallCore(diContainer, instance, methodName, parameters);
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
        }

        public static TResult Call<TResult>(this IDiContainer diContainer, object instance, string methodName)
        {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return (TResult)CallCore(diContainer, instance, methodName, Array.Empty<object>());
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
        }

        public static void Call(this IDiContainer diContainer, object instance, string methodName, object manualParameter, params object[] manualParameters)
        {
            var parameters = JoinParameters(manualParameter, manualParameters);

            CallCore(diContainer, instance, methodName, parameters);
        }
        public static void Call(this IDiContainer diContainer, object instance, string methodName)
        {
            CallCore(diContainer, instance, methodName, Array.Empty<object>());
        }

        #endregion
    }
}
