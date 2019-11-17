using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public enum FontTarget
    {
        LauncherToolbar,
        NoteContent,
        Command,
    }

    public interface IFontTheme
    {
        #region function

        FontData GetDefaultFont(FontTarget fontTarget);

        #endregion
    }
}
