using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Bridge.Model.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    internal class FontTheme : ThemeBase, IFontTheme
    {
        public FontTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region function
        #endregion

        #region IFontTheme

        public FontData GetDefaultFont(FontTarget fontTarget)
        {
            var fc = new FontConverter(Lf.Create());

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
