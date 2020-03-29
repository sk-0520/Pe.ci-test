using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    public static class IDiContainerExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IDiContainer.New{TObject}(IEnumerable{object})"/> して <see cref="IDiContainer.Inject{TObject}(TObject)"/> する。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="this"></param>
        /// <param name="manualParameter"></param>
        /// <param name="manualParameters"></param>
        /// <returns></returns>
        public static TObject Build<TObject>(this IDiContainer @this, object manualParameter, params object[] manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var parameters = new List<object>(1 + manualParameters.Length) {
                manualParameter,
            };
            parameters.AddRange(manualParameters);

            var obj = @this.New<TObject>(parameters);
            @this.Inject(obj);
            return obj;
        }

        /// <inheritdoc cref="Build{TObject}(IDiContainer, object, object[])"/>
        public static TObject Build<TObject>(this IDiContainer @this)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            var obj = @this.New<TObject>();
            @this.Inject(obj);
            return obj;
        }


        #endregion
    }
}
