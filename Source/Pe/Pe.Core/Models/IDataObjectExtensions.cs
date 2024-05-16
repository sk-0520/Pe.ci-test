using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class IDataObjectExtensions
    {
        #region property

        private static string[] Formats { get; } = new[] {
            DataFormats.UnicodeText,
            DataFormats.OemText,
            DataFormats.Text,
        };

        #endregion

        #region function

        /// <summary>
        /// <see cref="IDataObject"/>から安全にデータを取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGet<T>(this IDataObject dataObject, [MaybeNullWhen(false)] out T value)
        {
            var type = typeof(T);
            if(dataObject.GetDataPresent(type)) {
                var data = dataObject.GetData(type);
                if(data != null) {
                    value = (T)data;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// 指定データがテキストを保持しているか。
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static bool IsTextPresent(this IDataObject dataObject)
        {
            return Formats.Any(i => dataObject.GetDataPresent(i));
        }

        /// <summary>
        /// テキストデータを取得。
        /// </summary>
        /// <remarks>
        /// <para>あらかじめ<see cref="IsTextPresent(IDataObject)"/>を使用すること。</para>
        /// </remarks>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">テキストデータが存在しない。</exception>
        public static string RequireText(this IDataObject dataObject)
        {
            foreach(var format in Formats) {
                if(dataObject.GetDataPresent(format)) {
                    return (string)dataObject.GetData(format);
                }
            }

            throw new InvalidCastException(string.Join(", ", dataObject.GetFormats()));
        }

        #endregion
    }
}
