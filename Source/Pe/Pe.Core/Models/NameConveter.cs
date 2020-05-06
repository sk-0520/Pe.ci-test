using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Microsoft.Windows.Themes;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class NameConveter
    {
        #region function

        private string PascalToSeparatorCase(string source, char separator)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(source));
            Debug.Assert(!char.IsControl(separator));

            var builder = new StringBuilder((int)(source.Length * 1.5));
            var lastUpperIndex = -1;
            for(var i = 0; i < source.Length; i++) {
                var c = source[i];
                if(char.IsUpper(c)) {
                    if(i != 0 && lastUpperIndex != i - 1) {
                        builder.Append(separator);
                    }
                    builder.Append(char.ToLower(c));
                    lastUpperIndex = i;
                } else {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// パスカル形式をケバブ形式に変換。
        /// <para>AbcDef: abc-def</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string PascalToKebab(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            return PascalToSeparatorCase(source, '-');
        }

        /// <summary>
        /// パスカル形式をスネーク形式に変換。
        /// <para>AbcDef: abc_def</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string PascalToSnake(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            return PascalToSeparatorCase(source, '_');
        }

        /// <summary>
        /// パスカル形式をキャメル形式に変換。
        /// <para>AbcDef: abcDef</para>
        /// <para>連続する大文字の2文字目以降は小文字に変換される</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string PascalToCamel(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            var builder = new StringBuilder((int)(source.Length));
            var lastUpperIndex = -1;
            for(var i = 0; i < source.Length; i++) {
                var c = source[i];
                if(char.IsUpper(c)) {
                    if(i == 0) {
                        builder.Append(char.ToLower(c));
                    } else if(lastUpperIndex == i - 1) {
                        builder.Append(char.ToLower(c));
                    } else {
                        builder.Append(c);
                    }
                    lastUpperIndex = i;
                } else {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
