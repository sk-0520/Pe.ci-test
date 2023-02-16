using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base.Models
{
    /// <summary>
    /// 命名変換処理。
    /// </summary>
    public class NameConverter
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
                    builder.Append(char.ToLowerInvariant(c));
                    lastUpperIndex = i;
                } else {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// パスカル形式をケバブ形式に変換。
        /// <para>AbcDef -&gt; abc-def</para>
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
        /// <para>AbcDef -&gt; abc_def</para>
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
        /// <para>AbcDef -&gt; abcDef</para>
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

            var builder = new StringBuilder(source.Length);
            var lastUpperIndex = -1;
            for(var i = 0; i < source.Length; i++) {
                var c = source[i];
                if(char.IsUpper(c)) {
                    if(i == 0) {
                        builder.Append(char.ToLowerInvariant(c));
                    } else if(lastUpperIndex == i - 1) {
                        builder.Append(char.ToLowerInvariant(c));
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

        private string ToPascalCore(string source, char separator)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(source));
            Debug.Assert(!char.IsControl(separator));

            var isUpper = true;
            var builder = new StringBuilder(source.Length);
            for(var i = 0; i < source.Length; i++) {
                var c = source[i];
                if(c == separator) {
                    if(i + 1 == source.Length) {
                        break;
                    }

                    var next = source[i + 1];
                    if(next == separator) {
                        continue;
                    }
                    if('a' <= next && next <= 'z') {
                        builder.Append(char.ToUpperInvariant(next));
                        i += 1;
                        isUpper = false;
                        continue;
                    }
                    if('A' <= next && next <= 'Z') {
                        builder.Append(next);
                        i += 1;
                        isUpper = false;
                        continue;
                    }
                }

                if(isUpper) {
                    builder.Append(char.ToUpperInvariant(c));
                    isUpper = false;
                } else {
                    builder.Append(c);

                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// ケバブ形式をパスカル形式に変換。
        /// <para>abc-def -&gt; AbcDef</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string KebabToPascal(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            return ToPascalCore(source, '-');
        }

        /// <summary>
        /// スネーク形式をパスカル形式に変換。
        /// <para>abc_def -&gt; AbcDef</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string SnakeToPascal(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            return ToPascalCore(source, '_');
        }

        #endregion
    }
}
