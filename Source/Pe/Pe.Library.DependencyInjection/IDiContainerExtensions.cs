using System;
using System.Collections.Generic;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
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
        /// <see cref="IDiContainer.New{TObject}(IReadOnlyCollection{object})"/> して <see cref="IDiContainer.Inject{TObject}(TObject)"/> する。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDiContainer.New"/>/<see cref="IDiContainer.Get"/> で悩むくらいなら多分状況ワケわからんことになっているのでこれだけ使っておけばいい。</para>
        /// </remarks>
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

        private static object? CallCore(IDiContainer diContainer, object instance, string methodName, IReadOnlyCollection<object> manualParameters)
        {
            var methodInfo = instance.GetType().GetMethod(methodName);
            if(methodInfo is null) {
                throw new DiFunctionMethodNotFoundException($"{nameof(methodInfo)}: {methodName}");
            }

            var obj = diContainer.CallMethod(string.Empty, instance, methodInfo, manualParameters);

            return obj;
        }

        public static TResult Call<TResult>(this IDiContainer diContainer, object instance, string methodName, object manualParameter, params object[] manualParameters)
        {
            var parameters = JoinParameters(manualParameter, manualParameters);

            var raw = CallCore(diContainer, instance, methodName, parameters);
            if(raw is not TResult) {
                throw new DiFunctionResultException();
            }
            return (TResult)raw;
        }

        public static TResult Call<TResult>(this IDiContainer diContainer, object instance, string methodName)
        {
            var raw = CallCore(diContainer, instance, methodName, Array.Empty<object>());
            if(raw is not TResult) {
                throw new DiFunctionResultException();
            }
            return (TResult)raw;
        }

        public static void Call(this IDiContainer diContainer, object instance, string methodName, object manualParameter, params object[] manualParameters)
        {
            var parameters = JoinParameters(manualParameter, manualParameters);

            var raw = CallCore(diContainer, instance, methodName, parameters);
            if(raw is not null) {
                throw new DiFunctionResultException();
            }
        }
        public static void Call(this IDiContainer diContainer, object instance, string methodName)
        {
            var raw = CallCore(diContainer, instance, methodName, Array.Empty<object>());
            if(raw is not null) {
                throw new DiFunctionResultException();
            }
        }

        #endregion
    }
}
