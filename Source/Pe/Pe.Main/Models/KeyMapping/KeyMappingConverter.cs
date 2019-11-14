using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models.KeyMapping
{
    /// <summary>
    /// 主にView用のキー変換処理。
    /// </summary>
    public class KeyMappingConverter
    {
        #region function

        private string ChangePlainKeyText(string keyText)
        {
            return keyText switch
            {
                _ => keyText
            };
        }

        public Key ConvertFromString(string keyText)
        {
            if(string.IsNullOrWhiteSpace(keyText)) {
                return Key.None;
            }

            var s = keyText.ToLowerInvariant().Trim();
            if(s == "none") {
                return Key.None;
            }

            var keyConverter = new KeyConverter();
            var key = (Key)keyConverter.ConvertFromString(s);
            if(key != Key.None) {
                return key;
            }

            var changedKey = ChangePlainKeyText(keyText);
            return (Key)keyConverter.ConvertFromString(changedKey);
        }

        public string ConvertFromKey(Key key)
        {
            if(key == Key.None) {
                return "none";
            }

            var keyConverter = new KeyConverter();
            var keyText = keyConverter.ConvertToString(key).ToLowerInvariant();
            return keyText;
        }

        #endregion
    }
}
