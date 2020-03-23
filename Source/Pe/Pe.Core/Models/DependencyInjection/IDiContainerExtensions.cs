using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    public static class IDiContainerExtensions
    {
        #region function

        public static TObject Build<TObject>(this IDiContainer @this, params object[] parameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return @this.Make<TObject>(parameters);
        }

        #endregion
    }
}
