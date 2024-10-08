using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// 行毎処理の行データ。
    /// </summary>
    public readonly struct TextLineInput
    {
        public TextLineInput(string line, int number)
        {
            Line = line;
            Number = number;
        }

        #region property

        /// <summary>
        /// 行文字列。
        /// </summary>
        /// <remarks>
        /// <para>改行は含まれない。</para>
        /// </remarks>
        public string Line { get; }
        /// <summary>
        /// 行番号(1基底)。
        /// </summary>
        public int Number { get; }

        #endregion
    }

    /// <summary>
    /// 行毎処理。
    /// </summary>
    public class TextLines
    {
        public TextLines()
            : this(Environment.NewLine)
        { }

        public TextLines(string newLine)
            : this(newLine, 1024)
        { }

        public TextLines(string newLine, int capacity)
        {
            if(capacity <= 0) {
                throw new ArgumentException("<= 0", nameof(capacity));
            }

            NewLine = newLine;
            Capacity = capacity;
        }

        #region property

        /// <summary>
        /// 行結合時に使用される改行符。
        /// </summary>
        public string NewLine { get; }
        /// <summary>
        /// 行結合時の内部予約サイズ。
        /// </summary>
        public int Capacity { get; }

        #endregion

        #region function

        /// <inheritdoc cref="Aggregate(TextReader, Func{TextLineInput, string?})"/>
        public string Aggregate(string text, Func<TextLineInput, string?> func)
        {
            using var reader = new StringReader(text);
            return Aggregate(reader, func);
        }

        /// <summary>
        /// 各行に対して処理した結果を返す。
        /// </summary>
        /// <param name="reader">リーダー。</param>
        /// <param name="func">処理。戻り値が<see langword="null" />の場合は該当行は結果に使用されない。</param>
        /// <returns>処理した各行を<see cref="NewLine"/>で結合した結果。</returns>
        public string Aggregate(TextReader reader, Func<TextLineInput, string?> func)
        {
            var builder = new StringBuilder(Capacity);

            int number = 0;
            var useLine = false;
            foreach(var line in TextUtility.ReadLines(reader)) {
                if(0 != number && useLine) {
                    builder.Append(NewLine);
                }

                var input = new TextLineInput(line, ++number);

                var result = func(input);
                useLine = result is not null;
                if(useLine) {
                    builder.Append(result);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
