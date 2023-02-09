using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteFileElement: ElementBase
    {
        public NoteFileElement(NoteFileData data, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = data.NoteId;
            NoteFileId = data.NoteFileId;
            NoteFileKind = data.NoteFileKind;
            NoteFilePath = data.NoteFilePath;
            Sequence = data.Sequence;

            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        public NoteId NoteId { get; }
        public NoteFileId NoteFileId { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        public NoteFileKind NoteFileKind { get; private set; }
        public string NoteFilePath { get; private set; } = string.Empty;
        public int Sequence { get; private set; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            //nop
        }

        #endregion
    }
}
