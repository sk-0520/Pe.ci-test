using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Font
{
    public class FontElement : ElementBase
    {
        #region variable

        FontFamily _fontFamily;
        FontStyle _fontStyle;
        FontWeight _FontWeight;
        double _fontSize;

        #endregion

        public FontElement(Guid fontId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IFontTheme fontTheme, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FontId = fontId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            FontTheme = fontTheme;
            IdFactory = idFactory;
        }

        #region property

        public Guid FontId { get; private set; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IFontTheme FontTheme { get; }
        IIdFactory IdFactory { get; }

        public FontFamily FontFamily
        {
            get => this._fontFamily;
            private set => SetProperty(ref this._fontFamily, value);
        }
        public FontStyle FontStyle
        {
            get => this._fontStyle;
            private set => SetProperty(ref this._fontStyle, value);
        }
        public FontWeight FontWeight
        {
            get => this._FontWeight;
            private set => SetProperty(ref this._FontWeight, value);
        }
        public double FontSize
        {
            get => this._fontSize;
            private set => SetProperty(ref this._fontSize, value);
        }

        public bool IsDefaultFont => FontId == Guid.Empty;

        #endregion

        #region function

        FontData GetFontData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new FontsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectFont(FontId);
            }
        }

        void LoadFont()
        {
            var data = IsDefaultFont
                ? FontTheme.GetDefaultFont(FontTarget.NoteContent)
                : GetFontData()
            ;
            if(data == null) {
                Logger.Information($"フォントの読み込みに失敗: {FontId}");
                data = FontTheme.GetDefaultFont(FontTarget.NoteContent);
            }
            Debug.Assert(data != null);

            var fc = new FontConverter(Logger.Factory);
            FontFamily = fc.MakeFontFamily(data.Family, SystemFonts.MessageFontFamily);
            FontSize = data.Size;
            FontWeight = fc.ToWeight(data.Bold);
            FontStyle = fc.ToStyle(data.Italic);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadFont();
        }

        #endregion
    }
}
