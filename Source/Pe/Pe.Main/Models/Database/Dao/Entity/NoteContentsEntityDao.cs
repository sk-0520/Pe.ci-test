using System;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteContentsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class NoteContentsEntityDto: RowDtoBase
        {
            #region property

            public NoteId NoteId { get; set; }
            public string ContentKind { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public bool IsLink { get; set; }
            public string Address { get; set; } = string.Empty;
            public string Encoding { get; set; } = string.Empty;
            public TimeSpan DelayTime { get; set; }
            public long BufferSize { get; set; }
            public TimeSpan RefreshTime { get; set; }
            public bool IsEnabledRefresh { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";
            public static string ContentKind { get; } = "ContentKind";
            public static string IsLink { get; } = "IsLink";
            public static string Content { get; } = "Content";
            public static string Address { get; } = "Address";
            public static string Encoding { get; } = "Encoding";
            public static string DelayTime { get; } = "DelayTime";
            public static string BufferSize { get; } = "BufferSize";
            public static string RefreshTime { get; } = "RefreshTime";
            public static string IsEnabledRefresh { get; } = "IsEnabledRefresh";

            #endregion
        }

        #endregion

        public NoteContentsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private NoteContentsEntityDto ConvertFromData(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var dto = new NoteContentsEntityDto() {
                NoteId = data.NoteId,
                ContentKind = noteContentKindTransfer.ToString(data.ContentKind),
                IsLink = data.IsLink,
                Content = data.Content,
                Address = data.FilePath,
                Encoding = EncodingUtility.ToString(data.Encoding),
                DelayTime = data.DelayTime,
                BufferSize = data.BufferSize,
                RefreshTime = data.RefreshTime,
                IsEnabledRefresh = data.IsEnabledRefresh,
            };

            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        private NoteContentData ConvertFromDto(NoteContentsEntityDto dto)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var data = new NoteContentData() {
                NoteId = dto.NoteId,
                ContentKind = noteContentKindTransfer.ToEnum(dto.ContentKind!),
                Content = dto.Content,
                IsLink = dto.IsLink,
                FilePath = dto.Address,
                Encoding = encodingConverter.Parse(dto.Encoding),
                DelayTime = dto.DelayTime,
                BufferSize = ToInt(dto.BufferSize),
                RefreshTime = dto.RefreshTime,
                IsEnabledRefresh = dto.IsEnabledRefresh,
            };

            return data;
        }

        public bool SelectExistsContent(NoteId noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            return Context.QueryFirst<bool>(statement, param);
        }

        public string SelectFullContent(NoteId noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            return Context.QueryFirst<string>(statement, param);
        }

        public NoteContentData SelectLinkParameter(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };
            var dto = Context.QueryFirst<NoteContentsEntityDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public void InsertNewContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(data, databaseCommonStatus);

            Context.InsertSingle(statement, param);
        }

        public void UpdateContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(data, databaseCommonStatus);

            Context.UpdateByKey(statement, param);
        }

        public void UpdateLinkEnabled(NoteId noteId, string path, Encoding encoding, FileWatchParameter fileWatchParameter, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.NoteId] = noteId;
            parameter[Column.IsLink] = true;
            parameter[Column.Address] = path;
            parameter[Column.Encoding] = EncodingUtility.ToString(encoding);
            parameter[Column.DelayTime] = fileWatchParameter.DelayTime;
            parameter[Column.BufferSize] = (long)fileWatchParameter.BufferSize;
            parameter[Column.RefreshTime] = fileWatchParameter.RefreshTime;
            parameter[Column.IsEnabledRefresh] = fileWatchParameter.IsEnabledRefresh;

            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateLinkDisabled(NoteId noteId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.NoteId] = noteId;
            parameter[Column.IsLink] = true;
            parameter[Column.Address] = string.Empty;

            Context.UpdateByKey(statement, parameter);
        }

        public int DeleteContents(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId
            };

            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
