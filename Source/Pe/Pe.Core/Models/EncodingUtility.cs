using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// エンコーディング。。。
    /// </summary>
    public static class EncodingUtility
    {
        #region property

        /// <summary>
        /// BOMありUTF-8名。
        /// </summary>
        public static string UTF8BomName => "utf-8bom";
        /// <summary>
        /// BOMなしUTF-8名。
        /// </summary>
        public static string UTF8nName => "utf-8n";

        /// <summary>
        /// BOMありUTF-8エンコーディング。
        /// </summary>
        public static Encoding UTF8Bom => new UTF8Encoding(true);
        /// <summary>
        /// BOMなしUTF-8エンコーディング。
        /// </summary>
        public static Encoding UTF8N => new UTF8Encoding(false);

        #endregion

        #region function

        public static Encoding Parse(string encodingName)
        {
            if(encodingName == UTF8BomName) {
                return UTF8Bom;
            }
            if(encodingName == UTF8nName) {
                return UTF8N;
            }

            return Encoding.GetEncoding(encodingName);
        }

        public static string ToString(Encoding encoding)
        {
            if(encoding is UTF8Encoding utf8) {
                if(utf8.GetPreamble().Length != 0) {
                    return UTF8BomName;
                }
            }

            return encoding.WebName;
        }

        #endregion
    }
}
