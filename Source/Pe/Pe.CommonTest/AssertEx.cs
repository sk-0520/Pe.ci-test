using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;

using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// テスト拡張系。
    /// </summary>
    public static class AssertEx
    {
        #region function

        private static IEnumerable<string> ReadLines(string s)
        {
            using var reader = new StringReader(s);
            string? line;
            while((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }

        /// <summary>
        /// 複数行文字列の場合に改行符を無視して一致を判定する。
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void EqualMultiLineTextIgnoreNewline(string expected, string actual)
        {
            var e = string.Join(Environment.NewLine, ReadLines(expected));
            var a = string.Join(Environment.NewLine, ReadLines(actual));
            Assert.Equal(e, a);
        }

        /// <summary>
        /// 複数行文字列の場合に改行符を無視して不一致を判定する。
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void NotEqualMultiLineTextIgnoreNewline(string expected, string actual)
        {
            var e = string.Join(Environment.NewLine, ReadLines(expected));
            var a = string.Join(Environment.NewLine, ReadLines(actual));
            Assert.NotEqual(e, a);
        }
        #endregion
    }
}
