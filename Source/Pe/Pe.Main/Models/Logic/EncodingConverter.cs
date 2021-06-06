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

        /// <summary>
        ///
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ToDisplayText(Encoding encoding)
        {
            if(encoding is UTF8Encoding utf8) {
                if(utf8.GetPreamble().Length != 0) {
                    return Properties.Resources.String_Encoding_Name_Utf8Bom;
                }
                return Properties.Resources.String_Encoding_Name_Utf8N;
            }

            switch(encoding.CodePage) {
                case 932:
                    return Properties.Resources.String_Encoding_Name_ShiftJis;

                case 1200:
                    return Properties.Resources.String_Encoding_Name_Utf16LittleEndian;

                case 1201:
                    return Properties.Resources.String_Encoding_Name_Utf16BigEndian;
                case 12000:
                    return Properties.Resources.String_Encoding_Name_Utf32LittleEndian;

                case 20127:
                    return Properties.Resources.String_Encoding_Name_Ascii;

                default:
                    break;
            }

            return EncodingUtility.ToString(encoding);
        }

        #endregion
    }
}
