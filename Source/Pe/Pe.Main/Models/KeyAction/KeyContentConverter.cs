using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyContentConverterBase
    {
        #region function

        protected Key ToKey(string content)
        {
            var keyConverter = new KeyConverter();
            return (Key)keyConverter.ConvertFromInvariantString(content);
        }

        #endregion
    }

    public class KeyReplaceContentConverter : KeyContentConverterBase
    {
        #region function

        public Key ToReplaceKey(string content) => ToKey(content);

        public string ToContent(Key key) => key.ToString();

        #endregion
    }

    public class KeyLauncherItemContentConverter : KeyContentConverterBase
    {
        #region function

        public KeyActionContentLauncherItem ToKeyActionContentLauncherItem(string content)
        {
            return EnumUtility.Parse<KeyActionContentLauncherItem>(content);
        }

        public string ToContent(KeyActionContentLauncherItem keyActionContentLauncherItem)
        {
            return keyActionContentLauncherItem.ToString();
        }

        #endregion
    }

}
