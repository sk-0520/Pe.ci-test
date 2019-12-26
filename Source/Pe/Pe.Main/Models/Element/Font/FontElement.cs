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

    public class FontElement : ElementBase, IFlushable
    {
        #region variable

        string _familyName = string.Empty;
        bool _isItalic;
        bool _isBold;
        double _size;

        #endregion

        public FontElement(Guid fontId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FontId = fontId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid FontId { get; protected set; }
        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }

        public virtual string FamilyName
        {
            get => this._familyName;
            set => SetProperty(ref this._familyName, value);
        }

        public virtual bool IsItalic
        {
            get => this._isItalic;
            set => SetProperty(ref this._isItalic, value);
        }
        public virtual bool IsBold
        {
            get => this._isBold;
            set => SetProperty(ref this._isBold, value);
        }
        public virtual double Size
        {
            get => this._size;
            set => SetProperty(ref this._size, value);
        }

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

        protected virtual FontData GetFontData()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new FontsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFont(FontId);
            }
        }

        void LoadFont()
        {
            ThrowIfDisposed();

            var data = GetFontData();

            var fc = new FontConverter(LoggerFactory);
            FamilyName = data.FamilyName;
            Size = data.Size;
            IsBold = data.IsBold;
            IsItalic = data.IsItalic;
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
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlushable

        public virtual void Flush()
        { }

        #endregion
    }

    public delegate void ParentUpdater(SavingFontElement fontElement, IDatabaseCommander commander, IDatabaseImplementation implementation);

    public class SavingFontElement : FontElement
    {
        public SavingFontElement(DefaultFontKind defaultFontKind, Guid fontId, ParentUpdater parentUpdater, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IFontTheme fontTheme, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(fontId, mainDatabaseBarrier, statementLoader, loggerFactory)
        {
            DefaultFontKind = defaultFontKind;
            ParentUpdater = parentUpdater;
            FontTheme = fontTheme;
            IdFactory = idFactory;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
        }

        #region property
        public DefaultFontKind DefaultFontKind { get; }

        ParentUpdater ParentUpdater { get; }
        IFontTheme FontTheme { get; }
        IIdFactory IdFactory { get; }

        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public bool IsDefaultFont { get; private set; } = true;


        //public FontTarget FontTarget { get; }

        #endregion

        #region function

        void CreateAndSaveFontId(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();
            if(!IsDefaultFont) {
                throw new InvalidOperationException(nameof(IsDefaultFont));
            }

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
            IsDefaultFont = false;
            RaisePropertyChanged(nameof(FontId));

            ParentUpdater(this, commander, implementation);
        }

        void UpdateValueDelaySave(Action<FontsEntityDao, IDatabaseCommonStatus> updater, object uniqueKey)
        {
            ThrowIfDisposed();

            MainDatabaseLazyWriter.Stock(c => {
                if(IsDefaultFont) {
                    CreateAndSaveFontId(c, c.Implementation);
                }

                var dao = new FontsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                updater(dao, DatabaseCommonStatus.CreateCurrentAccount());
            }, uniqueKey);

        }

        #endregion


        #region FontElementBase

        public override string FamilyName
        {
            get => base.FamilyName;
            set
            {
                base.FamilyName = value;
                UpdateValueDelaySave((d, s) => d.UpdateFamilyName(FontId, FamilyName, s), UniqueKeyPool.Get());
            }
        }
        public override bool IsItalic
        {
            get => base.IsItalic;
            set
            {
                base.IsItalic = value;
                UpdateValueDelaySave((d, s) => d.UpdateItalic(FontId, IsItalic, s), UniqueKeyPool.Get());
            }
        }
        public override bool IsBold
        {
            get => base.IsBold;
            set
            {
                base.IsBold = value;
                UpdateValueDelaySave((d, s) => d.UpdateBold(FontId, IsBold, s), UniqueKeyPool.Get());
            }
        }
        public override double Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                UpdateValueDelaySave((d, s) => d.UpdateHeight(FontId, Size, s), UniqueKeyPool.Get());
            }
        }

        public override void Flush()
        {
            ThrowIfDisposed();

            MainDatabaseLazyWriter.SafeFlush();
        }

        protected override FontData GetFontData()
        {
            Guid defaultFontId;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                switch(DefaultFontKind) {
                    case DefaultFontKind.Note: {
                            var dao = new AppNoteSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                            defaultFontId = dao.SelectAppNoteSettingFontId();
                        }
                        break;

                    case DefaultFontKind.Command: {
                            var dao = new AppCommandSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                            defaultFontId = dao.SelectCommandSettingFontId();
                        }
                        break;

                    case DefaultFontKind.LauncherToolbar: {
                            var dao = new AppLauncherToolbarSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                            defaultFontId = dao.SelectAppLauncherToolbarSettingFontId();
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            IsDefaultFont = FontId == defaultFontId;

            return base.GetFontData();
        }


        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                if(disposing) {
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }


}
