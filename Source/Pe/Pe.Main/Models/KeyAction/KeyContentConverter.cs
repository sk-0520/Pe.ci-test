using System;
using System.Windows.Input;
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

    public sealed class ReplaceContentConverter: KeyContentConverterBase
    {
        #region function

        public Key ToReplaceKey(string content) => ToKey(content);

        public string ToContent(Key key) => key.ToString();

        #endregion
    }

    public sealed class LauncherItemContentConverter: KeyContentConverterBase
    {
        #region function

        public KeyActionContentLauncherItem ToKeyActionContentLauncherItem(string content)
        {
            return Enum.Parse<KeyActionContentLauncherItem>(content, true);
        }

        public string ToContent(KeyActionContentLauncherItem keyActionContentLauncherItem)
        {
            return keyActionContentLauncherItem.ToString();
        }

        #endregion
    }

    public sealed class LauncherToolbarContentConverter: KeyContentConverterBase
    {
        #region function

        public KeyActionContentLauncherToolbar ToKeyActionContentLauncherToolbar(string content)
        {
            return Enum.Parse<KeyActionContentLauncherToolbar>(content, true);
        }

        public string ToContent(KeyActionContentLauncherToolbar keyActionContentLauncherToolbar)
        {
            return keyActionContentLauncherToolbar.ToString();
        }

        #endregion
    }

    public sealed class NoteContentConverter: KeyContentConverterBase
    {
        #region function

        public KeyActionContentNote ToKeyActionContentNote(string content)
        {
            return Enum.Parse<KeyActionContentNote>(content, true);
        }

        public string ToContent(KeyActionContentNote keyActionContentNote)
        {
            return keyActionContentNote.ToString();
        }

        #endregion
    }

}
