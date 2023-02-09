using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteFilesEntityDao: EntityDaoBase
    {
        #region define

        private class NoteFilesEntityDto: RowDtoBase
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

        #endregion
    }
}
