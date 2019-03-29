using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        Screen _dockScreen;

        #endregion

        public NoteElement(Guid noteId, Screen dockScreen, IOrderManager orderManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
            OrderManager = orderManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            NoteTheme = noteTheme;
        }

        #region property

        public Guid NoteId { get; }

        /// <summary>
        /// DB から取得して設定したりそれでも保存しなかったりするまさに変数。
        /// </summary>
        public Screen DockScreen
        {
            get => this._dockScreen;
            private set => SetProperty(ref this._dockScreen, value);
        }
        IOrderManager OrderManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        INoteTheme NoteTheme { get; }

        bool ViewCreated { get; set; }

        public bool IsTopmost
        {
            get => this._isTopmost;
            private set => SetProperty(ref this._isTopmost, value);
        }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }
        #endregion

        #region function

        void LoadNote()
        {
            IsVisible = true;
            //TODO: あればそれを読み込んでなければ作る
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadNote();
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return IsVisible;
            }
        }

        public void StartView()
        {
            var windowItem = OrderManager.CreateNoteWindow(this);
            ViewCreated = true;
        }

        #endregion

        #region IWindowCloseReceiver

        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }
}
