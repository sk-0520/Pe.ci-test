using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class IDataObjectExtensions
    {
        #region proeprty

        static string[] Formats { get; } = new[] {
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

        /// <summary>
        /// 指定データがテキストを保持しているか。
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsTextPresent(this IDataObject @this)
        {
            return Formats.Any(i => @this.GetDataPresent(i));
        }

        /// <summary>
        /// テキストデータを取得。
        /// <para>あらかじめ<see cref="IsTextPresent(IDataObject)"/>を使用すること。</para>
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">テキストデータが存在しない。</exception>
        public static string GetText(this IDataObject @this)
        {
            foreach(var format in Formats) {
                if(@this.GetDataPresent(format)) {
                    return (string)@this.GetData(format);
                }
            }

            throw new InvalidCastException(string.Join(", ", @this.GetFormats()));
        }

        #endregion
    }
}
