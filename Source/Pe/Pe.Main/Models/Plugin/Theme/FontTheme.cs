using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Theme
{
    internal class FontTheme : ThemeBase, IFontTheme
    {
        public FontTheme(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        { }

        #region function
        #endregion

        #region IFontTheme

        [Obsolete]
        public FontData GetDefaultFont(FontTarget fontTarget)
        {
            var fc = new FontConverter(Logger);

            switch(fontTarget) {
                case FontTarget.LauncherToolbar:
                    throw new NotImplementedException();

                case FontTarget.NoteContent:
                    return new FontData() {
                        FamilyName = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                        IsBold = fc.IsBold(SystemFonts.MessageFontWeight),
                        IsItalic = fc.IsItalic(SystemFonts.MessageFontStyle),
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
