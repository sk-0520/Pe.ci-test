using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Xunit;

namespace ContentTypeTextNet.Pe.Test
{
    /// <summary>
    /// テスト拡張系。
    /// </summary>
    public static class ExAssert
    {
        #region function

        /// <summary>
        /// 複数行文字列の場合に改行符を無視する。
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreMultiLineTextEqualWithoutNewline(string expected, string actual)
        {
            var e = TextUtility.ReadLines(expected).JoinString(Environment.NewLine);
            var a = TextUtility.ReadLines(actual).JoinString(Environment.NewLine);
            Assert.Equal(e, a);
        }

        #endregion
    }
}
