using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
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

    public class FontTheme : ThemeBase, IFontTheme
    {
        public FontTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region function
        #endregion

        #region IFontTheme

        public FontData GetDefaultFont(FontTarget fontTarget)
        {
            var fc = new FontConverter(Logger.Factory);

            switch(fontTarget) {
                case FontTarget.LauncherToolbar:
                    throw new NotImplementedException();

                case FontTarget.NoteContent:
                    return new FontData() {
                        Family = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                        Bold = fc.IsBold(SystemFonts.MessageFontWeight),
                        Italic = fc.IsItalic(SystemFonts.MessageFontStyle),
                        Size = SystemFonts.MessageFontSize,
                    };

                case FontTarget.Command:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
