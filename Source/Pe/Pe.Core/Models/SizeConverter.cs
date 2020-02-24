using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class SizeConverter
    {
        #region property

        public string[] Terms { get; } = new[] { "byte", "KB", "MB", "GB" };

        #endregion

        #region function

        public string ConvertHumanLikeByte(long byteSize, string sizeFormat, string[] terms)
        {
            double size = byteSize;
            int order = 0;
            while(size >= 1024 && ++order < terms.Length) {
                size = size / 1024;
            }

            return string.Format(sizeFormat, size, terms[order]);
        }

        public string ConvertHumanLikeByte(long byteSize, string[] terms)
        {
            return ConvertHumanLikeByte(byteSize, "{0:0.00} {1}", terms);
        }

        public string ConvertHumanLikeByte(long byteSize, string sizeFormat)
        {
            return ConvertHumanLikeByte(byteSize, sizeFormat, Terms);
        }

        public string ConvertHumanLikeByte(long byteSize)
        {
            return ConvertHumanLikeByte(byteSize, Terms);
        }

        #endregion
    }
}
