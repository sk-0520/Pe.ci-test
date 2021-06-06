using System;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class EncodingConverter
    {
        public EncodingConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        public static Encoding DefaultEncoding { get; } = EncodingUtility.UTF8N;

        public static Encoding DefaultStandardInputOutputEncoding { get; } = EncodingUtility.Parse("Shift_JIS"); // プラットフォーム固有のんがいいなぁと思う今日この頃な .net core

        ILogger Logger { get; }
        public Encoding Encoding { get; set; } = DefaultEncoding;

        #endregion

        #region function

        public Encoding Parse(string encodingName)
        {
            if(string.IsNullOrWhiteSpace(encodingName)) {
                Logger.LogWarning("エンコード名未入力");
                return Encoding;
            }

            try {
                return EncodingUtility.Parse(encodingName);
            } catch(ArgumentException ex) {
                Logger.LogWarning(ex, ex.Message);
                return Encoding;
            }
        }

        public string ToString(Encoding encoding) => EncodingUtility.ToString(encoding);

        #endregion
    }
}
