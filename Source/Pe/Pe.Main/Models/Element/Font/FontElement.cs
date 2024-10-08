using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Font
{

    public class FontElement: ElementBase, IFlushable
    {
        #region variable

        private string _familyName = string.Empty;
        private bool _isItalic;
        private bool _isBold;
        private double _size;

        #endregion

        public FontElement(FontId fontId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FontId = fontId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        public FontId FontId { get; protected set; }
        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }

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

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new FontsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectFont(FontId);
            }
        }

        private void LoadFont()
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

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            LoadFont();

            return Task.CompletedTask;
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

    public delegate void ParentUpdater(SavingFontElement fontElement, IDatabaseContext context, IDatabaseImplementation implementation);

    public class SavingFontElement: FontElement
    {
        public SavingFontElement(DefaultFontKind defaultFontKind, FontId fontId, ParentUpdater parentUpdater, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseDelayWriter mainDatabaseDelayWriter, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(fontId, mainDatabaseBarrier, statementLoader, loggerFactory)
        {
            DefaultFontKind = defaultFontKind;
            ParentUpdater = parentUpdater;
            IdFactory = idFactory;

            MainDatabaseDelayWriter = mainDatabaseDelayWriter;
        }

        #region property
        public DefaultFontKind DefaultFontKind { get; }

        private ParentUpdater ParentUpdater { get; }
        private IIdFactory IdFactory { get; }

        private IMainDatabaseDelayWriter MainDatabaseDelayWriter { get; }
        private UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public bool IsDefaultFont { get; private set; } = true;


        //public FontTarget FontTarget { get; }

        #endregion

        #region function

        private void CreateAndSaveFontId(IDatabaseContext context, IDatabaseImplementation implementation)
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

            var dao = new FontsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            dao.InsertFont(fontId, fontData, DatabaseCommonStatus.CreateCurrentAccount());

            FontId = fontId;
            IsDefaultFont = false;
            RaisePropertyChanged(nameof(FontId));

            ParentUpdater(this, context, implementation);
        }

        private void UpdateValueDelaySave(Action<FontsEntityDao, IDatabaseCommonStatus> updater, object uniqueKey)
        {
            ThrowIfDisposed();

            MainDatabaseDelayWriter.Stock(c => {
                if(IsDefaultFont) {
                    CreateAndSaveFontId(c, c.Implementation);
                }

                var dao = new FontsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                updater(dao, DatabaseCommonStatus.CreateCurrentAccount());
            }, uniqueKey);

        }

        #endregion

        #region FontElementBase

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "意図した挙動")]
        public override string FamilyName
        {
            get => base.FamilyName;
            set
            {
                base.FamilyName = value;
                UpdateValueDelaySave((d, s) => d.UpdateFamilyName(FontId, FamilyName, s), UniqueKeyPool.Get());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "意図した挙動")]
        public override bool IsItalic
        {
            get => base.IsItalic;
            set
            {
                base.IsItalic = value;
                UpdateValueDelaySave((d, s) => d.UpdateItalic(FontId, IsItalic, s), UniqueKeyPool.Get());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "意図した挙動")]
        public override bool IsBold
        {
            get => base.IsBold;
            set
            {
                base.IsBold = value;
                UpdateValueDelaySave((d, s) => d.UpdateBold(FontId, IsBold, s), UniqueKeyPool.Get());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "意図した挙動")]
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

            MainDatabaseDelayWriter.SafeFlush();
        }

        protected override FontData GetFontData()
        {
            FontId defaultFontId;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                switch(DefaultFontKind) {
                    case DefaultFontKind.Note: {
                            var dao = new AppNoteSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                            defaultFontId = dao.SelectAppNoteSettingFontId();
                        }
                        break;

                    case DefaultFontKind.Command: {
                            var dao = new AppCommandSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                            defaultFontId = dao.SelectCommandSettingFontId();
                        }
                        break;

                    case DefaultFontKind.LauncherToolbar: {
                            var dao = new AppLauncherToolbarSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
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
