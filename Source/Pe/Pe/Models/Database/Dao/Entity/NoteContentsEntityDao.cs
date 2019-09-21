using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteContentsEntityDao : EntityDaoBase
    {
        public NoteContentsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";

            #endregion
        }

        #endregion

        #region function
        private NoteContentsEntityDto ConvertFromData(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var dto = new NoteContentsEntityDto() {
                NoteId = data.NoteId,
                ContentKind = noteContentKindTransfer.ToString(data.ContentKind),
                Content = data.Content,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelectExistsContent(Guid noteId, NoteContentKind contentKind)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
                ContentKind = noteContentKindTransfer.ToString(contentKind),
            };
            return Commander.QueryFirst<bool>(statement, param);
        }

        public string SelectFullContent(Guid noteId, NoteContentKind contentKind)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
                ContentKind = noteContentKindTransfer.ToString(contentKind),
            };
            return Commander.QueryFirst<string>(statement, param);
        }

        public bool InsertNewContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        public int DeleteContents(Guid noteId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddValue(Column.NoteId, noteId);
            return ExecuteDelete(builder);
        }

        #endregion
    }
}
