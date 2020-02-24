using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class IDataObjectExtensions
    {
        #region function

        public static bool TryGet<T>(this IDataObject @this, out T value)
        {
            var type = typeof(T);
            if(@this.GetDataPresent(type)) {
                var data = @this.GetData(type);
                if(data != null) {
                    value = (T)data;
                    return true;
                }
            }
            value = default!;
            return false;
        }

        #endregion
    }
}
