using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class IDataObjectExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IDataObject"/>から安全にデータを取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGet<T>(this IDataObject @this, [MaybeNullWhen(false)] out T value)
        {
            var type = typeof(T);
            if(@this.GetDataPresent(type)) {
                var data = @this.GetData(type);
                if(data != null) {
                    value = (T)data;
                    return true;
                }
            }
            value = default;
            return false;
        }

        #endregion
    }
}
