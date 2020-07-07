using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class SizeConverter
    {
        #region property

        /// <summary>
        /// サイズ単位。
        /// </summary>
        public string[] Terms { get; } = new[] { "byte", "KB", "MB", "GB", "TB" };

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
        /// <param name="terms"></param>
        /// <returns></returns>
        public string ConvertHumanReadableByte(long byteSize, string sizeFormat, IReadOnlyList<string> terms)
        {
            double size = byteSize;
            int order = 0;
            while(size >= KbSize && ++order < terms.Count) {
                size = size / KbSize;
            }

            return string.Format(sizeFormat, size, terms[order]);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize, IReadOnlyList<string> terms)
        {
            return ConvertHumanReadableByte(byteSize, "{0:0.00} {1}", terms);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize, string sizeFormat)
        {
            return ConvertHumanReadableByte(byteSize, sizeFormat, Terms);
        }

        /// <inheritdoc cref="ConvertHumanReadableByte(long, string, IReadOnlyList{string})"/>
        public string ConvertHumanReadableByte(long byteSize)
        {
            return ConvertHumanReadableByte(byteSize, Terms);
        }

        #endregion
    }
}
