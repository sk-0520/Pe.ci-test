using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    /// エンコーディング。。。
    /// <para>現状 規格上でも怪しいけど実運用上無視できない utf-8n のためだけのクラス。</para>
    /// </summary>
    public class EncodingUtility
    {
        #region property

        public static Encoding UTF8n => new UTF8Encoding(false);

        #endregion

        #region function

        public static Encoding Parse(string encodingName)
        {
            if(encodingName == "utf-8n") {
                return UTF8n;
            }

            return Encoding.GetEncoding(encodingName);
        }

        #endregion
    }
}
