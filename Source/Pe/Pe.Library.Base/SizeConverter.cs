using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// (バイト: IEC_80000-13)サイズ変換処理。
    /// </summary>
    public class SizeConverter
    {
        #region property

        /// <summary>
        /// サイズ単位。
        /// </summary>
        public string[] Units { get; } = new[] { "byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", }; // YB とか生きている間に見ることあるんだろうか

        /// <summary>
        /// 1KB のサイズ。
        /// </summary>
        public int KbSize { get; } = 1024;

        #endregion

        #region function

        /// <summary>
        /// 人間様が読みやすい形にサイズ(バイト数)を整える。
        /// </summary>
        /// <param name="byteSize"></param>
        /// <param name="sizeFormat"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public string ConvertHumanReadableByte(long byteSize, string sizeFormat, IReadOnlyList<string> units)
        {
            double size = byteSize;
            int order = 0;
            while(KbSize <= size && ++order < units.Count) {
                size = size / KbSize;
            }

            if(units.Count <= order) {
                order -= 1;
            }

            return string.Format(CultureInfo.InvariantCulture, sizeFormat, size, units[order]);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize, IReadOnlyList<string> terms)
        {
            return ConvertHumanReadableByte(byteSize, "{0:0.00} {1}", terms);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize, string sizeFormat)
        {
            return ConvertHumanReadableByte(byteSize, sizeFormat, Units);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize)
        {
            return ConvertHumanReadableByte(byteSize, Units);
        }

        #endregion
    }
}
