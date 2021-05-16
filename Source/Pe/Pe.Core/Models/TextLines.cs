using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public readonly struct TextLineInput
    {
        public TextLineInput(string line, int number)
        {
            Line = line;
            Number = number;
        }

        #region property

        public string Line { get; }
        public int Number { get; }

        #endregion
    }

    public class TextLines
    {
        public TextLines()
        { }

        #region property

        public string NewLine { get; init; } = Environment.NewLine;
        public int Capacity { get; init; } = 1024;

        #endregion

        /// <inheritdoc cref="Aggregate(TextReader, Func{TextLineInput, string?})"/>
        public string Aggregate(string text, Func<TextLineInput, string?> func)
        {
            using var reader = new StringReader(text);
            return Aggregate(reader, func);
        }

        /// <summary>
        /// 各行に対して処理した結果を返す。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="func">処理。戻り値が<c>null</c>の場合は該当行は結果に使用されない。</param>
        /// <returns>処理結果。</returns>
        public string Aggregate(TextReader reader, Func<TextLineInput, string?> func)
        {
            var builder = new StringBuilder(Capacity);

            int number = 0;
            foreach(var line in TextUtility.ReadLines(reader)) {
                if(0 != number) {
                    builder.Append(NewLine);
                }

                var input = new TextLineInput(line, ++number);

                var result = func(input);
                if(result is not null) {
                    builder.Append(result);
                }
            }

            return builder.ToString();
        }

    }
}
