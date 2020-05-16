using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// エンコーディング。。。
    /// <para>現状 規格上でも怪しいけど実運用上無視できない utf-8n のためだけのクラス。</para>
    /// </summary>
    public static class EncodingUtility
    {
        #region property

        public static string UTF8nName => "utf-8n";

        public static Encoding UTF8n => new UTF8Encoding(false);

        #endregion

        #region function

        public static Encoding Parse(string encodingName)
        {
            if(encodingName == UTF8nName) {
                return UTF8n;
            }

            return Encoding.GetEncoding(encodingName);
        }

        public static string ToString(Encoding encoding)
        {
            if(encoding is UTF8Encoding utf8) {
                if(utf8.GetPreamble().Length == 0) {
                    return UTF8nName;
                }
            }

            return encoding.WebName;
        }

        #endregion
    }
}
