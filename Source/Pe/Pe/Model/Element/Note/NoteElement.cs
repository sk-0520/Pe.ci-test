using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteElement : ElementBase
    {
        #region variable

        Screen _dockScreen;

        #endregion

        public NoteElement(Guid noteId, Screen dockScreen, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
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
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        INoteTheme NoteTheme { get; }

        #endregion

        #region function

        void LoadNote()
        {

        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadNote();
        }

        #endregion
    }
}
