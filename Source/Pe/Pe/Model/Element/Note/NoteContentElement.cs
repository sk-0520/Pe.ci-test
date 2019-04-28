using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteContentElement: ElementBase
    {
        public NoteContentElement(NoteElement noteElement, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteElement = noteElement;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.NoteMainDatabaseLazyWriterWaitTime, this);
        }

        #region property

        protected NoteElement NoteElement { get; }
        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }

        protected DatabaseLazyWriter MainDatabaseLazyWriter { get; }

        public NoteContentKind ContentKind => NoteElement.ContentKind;

        #endregion

        #region function

        void LoadContent()
        {

        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadContent();
        }

        #endregion
    }
}
