using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteFilesEntityDao: EntityDaoBase
    {
        #region define

        private sealed class NoteFilesEntityDto: RowDtoBase
        {
            #region property

            public NoteId NoteId { get; set; }
            public NoteFileId NoteFileId { get; set; }

            public string FileKind { get; set; } = string.Empty;
            public string FilePath { get; set; } = string.Empty;
            public int Sequence { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";
            public static string NoteFileId { get; } = "NoteFileId";

            #endregion
        }

        #endregion

        public NoteFilesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private NoteFileData ConvertFromDto(NoteFilesEntityDto dto)
        {
            var noteFileKindEnumTransfer = new EnumTransfer<NoteFileKind>();

            return new NoteFileData {
                NoteId = dto.NoteId,
                NoteFileId = dto.NoteFileId,
                NoteFileKind = noteFileKindEnumTransfer.ToEnum(dto.FileKind),
                NoteFilePath = dto.FilePath,
                Sequence = dto.Sequence,
            };
        }

        private NoteFilesEntityDto ConvertFromData(NoteFileData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteFileKindEnumTransfer = new EnumTransfer<NoteFileKind>();

            var dto = new NoteFilesEntityDto {
                NoteId = data.NoteId,
                NoteFileId = data.NoteFileId,
                FileKind = noteFileKindEnumTransfer.ToString(data.NoteFileKind),
                FilePath = data.NoteFilePath,
                Sequence = data.Sequence,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public NoteFileId? SelectNoteFileExistsFilePath(NoteId noteId, string path)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
                FilePath = path,
            };

            return Context.QueryFirstOrDefault<NoteFileId?>(statement, parameter);
        }

        public IEnumerable<NoteFileData> SelectNoteFiles(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId
            };

            return Context.Query<NoteFilesEntityDto>(statement, parameter)
                .Select(a => ConvertFromDto(a))
            ;
        }

        public int SelectNextSequenceNoteFiles(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId
            };

            return Context.QueryFirstOrDefault<int>(statement, parameter);
        }

        public void InsertNoteFiles(NoteFileData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void DeleteNoteFilesById(NoteId noteId, NoteFileId noteFileId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
                NoteFileId = noteFileId
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
