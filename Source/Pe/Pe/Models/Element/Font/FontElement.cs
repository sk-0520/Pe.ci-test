using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Font
{
    public delegate void ParentUpdater(FontElement fontElement, IDatabaseCommander commander, IDatabaseImplementation implementation);

    public class FontElement : ElementBase, IFlushable
    {
        #region variable

        string? _familyName;
        bool _isItalic;
        bool _isBold;
        double _size;

        #endregion

        public FontElement(Guid fontId, ParentUpdater parentUpdater, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IFontTheme fontTheme, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FontId = fontId;
            ParentUpdater = parentUpdater;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            FontTheme = fontTheme;
            IdFactory = idFactory;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
        }

        #region property

        public Guid FontId { get; private set; }
        ParentUpdater ParentUpdater { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IFontTheme FontTheme { get; }
        IIdFactory IdFactory { get; }

        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public string? FamilyName
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

        public FontData FontData => new FontData() {
            FamilyName = FamilyName,
            Size = Size,
            IsBold = IsBold,
            IsItalic = IsItalic,
            IsUnderline = false,
            IsStrikeThrough = false,
        };



        #endregion

        #region function

        FontData? GetFontData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new FontsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
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
                Logger.LogInformation("フォントの読み込みに失敗: {0}", FontId);
                data = FontTheme.GetDefaultFont(FontTarget.NoteContent);
            }
            Debug.Assert(data != null);

            var fc = new FontConverter(LoggerFactory);
            FamilyName = data.FamilyName;
            Size = data.Size;
            IsBold = data.IsBold;
            IsItalic = data.IsItalic;
        }

        void CreateAndSaveFontId(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var fontId = IdFactory.CreateFontId();
            var fontConverter = new FontConverter(LoggerFactory);

            var fontData = new FontData() {
                FamilyName = FamilyName,
                Size = Size,
                IsBold = IsBold,
                IsItalic = IsItalic,
            };

            var dao = new FontsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
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

                var dao = new FontsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
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

        #region IFlushable

        public void Flush()
        {
            MainDatabaseLazyWriter.SafeFlush();
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
                Flush();
                if(disposing) {
                    MainDatabaseLazyWriter.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
