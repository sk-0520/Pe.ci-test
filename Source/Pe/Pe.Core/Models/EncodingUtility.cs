using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// エンコーディング。。。
    /// </summary>
    public static class EncodingUtility
    {
        #region property

        public static string UTF8BomName => "utf-8bom";

        public static Encoding UTF8Bom => new UTF8Encoding(true);

        #endregion

        #region function

        public static Encoding Parse(string encodingName)
        {
            if(encodingName == UTF8BomName) {
                return UTF8Bom;
            }

            return Encoding.GetEncoding(encodingName);
        }

        public static string ToString(Encoding encoding)
        {
            if(encoding is UTF8Encoding utf8) {
                if(utf8.GetPreamble().Length == 0) {
                    return UTF8BomName;
                }
            }

            return encoding.WebName;
        }

        #endregion
    }
}
