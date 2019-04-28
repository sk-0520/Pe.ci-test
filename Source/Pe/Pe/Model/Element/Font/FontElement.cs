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
    public delegate void ParentUpdater(FontElement fontElement, IDatabaseCommander commander, IDatabaseImplementation implementation);

    public class FontElement : ElementBase
    {
        #region variable

        string _familyName;
        bool _isItalic;
        bool _isBold;
        double _size;

        #endregion

        public FontElement(Guid fontId, ParentUpdater parentUpdater, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IFontTheme fontTheme, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FontId = fontId;
            ParentUpdater = parentUpdater;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            FontTheme = fontTheme;
            IdFactory = idFactory;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.LauncherToolbarMainDatabaseLazyWriterWaitTime, Logger.Factory);
        }

        #region property

        public Guid FontId { get; private set; }
        ParentUpdater ParentUpdater { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IFontTheme FontTheme { get; }
        IIdFactory IdFactory { get; }

        DatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public string FamilyName
        {
            get => this._familyName;
            private set => SetProperty(ref this._familyName, value);
        }

        public bool IsItalic
        {
            get => this._isItalic;
            private set => SetProperty(ref this._isItalic, value);
        }
        public bool IsBold
        {
            get => this._isBold;
            private set => SetProperty(ref this._isBold, value);
        }
        public double Size
        {
            get => this._size;
            private set => SetProperty(ref this._size, value);
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
            FamilyName = data.FamilyName;
            Size = data.Size;
            IsBold = data.IsBold;
            IsItalic = data.IsItalic;
        }

        void CreateAndSaveFontId(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var fontId = IdFactory.CreateFontId();
            var fontConverter = new FontConverter(Logger.Factory);

            var fontData = new FontData() {
                FamilyName = FamilyName,
                Size = Size,
                IsBold = IsBold,
                IsItalic = IsItalic,
            };

            var dao = new FontsEntityDao(commander, StatementLoader, implementation, Logger.Factory);
            dao.InsertFont(fontId, fontData, DatabaseCommonStatus.CreateCurrentAccount());

            FontId = fontId;
            RaisePropertyChanged(nameof(FontId));

            ParentUpdater(this, commander, implementation);
        }

        void UpdateValue(Action<FontsEntityDao, IDatabaseCommonStatus> updater, object uniqueKey)
        {
            MainDatabaseLazyWriter.Stock(c => {
                if(IsDefaultFont) {
                    CreateAndSaveFontId(c, c.Implementation);
                }

                var dao = new FontsEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                updater(dao, DatabaseCommonStatus.CreateCurrentAccount());
            }, uniqueKey);

        }

        public void ChangeFamilyName(string familyName)
        {
            FamilyName = familyName;
            UpdateValue((d, s) => d.UpdateFamilyName(FontId, FamilyName, s), UniqueKeyPool.Get());
        }

        public void ChangeBold(bool isBold)
        {
            IsBold = isBold;
            UpdateValue((d, s) => d.UpdateBold(FontId, IsBold, s), UniqueKeyPool.Get());
        }

        public void ChangeItalic(bool isItalic)
        {
            IsItalic = isItalic;
            UpdateValue((d, s) => d.UpdateItalic(FontId, IsItalic, s), UniqueKeyPool.Get());
        }

        public void ChangeSize(double size)
        {
            Size = size;
            UpdateValue((d, s) => d.UpdateHeight(FontId, Size, s), UniqueKeyPool.Get());
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadFont();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    MainDatabaseLazyWriter.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
