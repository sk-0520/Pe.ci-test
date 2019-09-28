using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
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

        #region function
        private NoteContentsEntityDto ConvertFromData(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var dto = new NoteContentsEntityDto() {
                NoteId = data.NoteId,
                ContentKind = noteContentKindTransfer.ToString(data.ContentKind),
                IsLink = data.IsLink,
                Content = data.Content,
                Address = data.FilePath,
                Encoding = data.Encoding != null ? EncodingUtility.ToString(data.Encoding) : EncodingUtility.ToString(Encoding.UTF8),
                DelayTime = data.DelayTime,
                BufferSize = data.BufferSize,
                RefreshTime = data.RefreshTime,
                IsEnabledRefresh = data.IsEnabledRefresh,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        private NoteContentData ConvertFromDto(NoteContentsEntityDto dto)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var data = new NoteContentData() {
                NoteId = dto.NoteId,
                ContentKind = noteContentKindTransfer.ToEnum(dto.ContentKind!),
                Content = dto.Content,
                IsLink = dto.IsLink,
                FilePath = dto.Address,
                Encoding = dto.Encoding != null ? EncodingUtility.Parse(dto.Encoding) : Encoding.UTF8,
                DelayTime = dto.DelayTime,
                BufferSize = (int)dto.BufferSize,//TODO: ご安全に！
                RefreshTime = dto.RefreshTime,
                IsEnabledRefresh = dto.IsEnabledRefresh,
            };

            return data;
        }

        public bool SelectExistsContent(Guid noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            return Commander.QueryFirst<bool>(statement, param);
        }

        public string SelectFullContent(Guid noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            return Commander.QueryFirst<string>(statement, param);
        }

        public NoteContentData SelectLinkParameter(Guid noteId)
        {
            var builder = CreateSelectBuilder();
            builder.AddValue(Column.NoteId, noteId);
            builder.AddSelect(Column.NoteId);
            builder.AddSelect(Column.IsLink);
            builder.AddSelect(Column.Address);
            builder.AddSelect(Column.Encoding);
            builder.AddSelect(Column.DelayTime);
            builder.AddSelect(Column.BufferSize);
            builder.AddSelect(Column.RefreshTime);
            builder.AddSelect(Column.IsEnabledRefresh);

            var dto = Select<NoteContentsEntityDto>(builder).First();
            return ConvertFromDto(dto);
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

        public bool UpdateLinkEnabled(Guid noteId, string path, Encoding encoding, FileWatchParameter fileWatchParameter, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);

            builder.AddKey(Column.NoteId, noteId);
            builder.AddValue(Column.IsLink, true);
            builder.AddValue(Column.Address, path);
            builder.AddValue(Column.Encoding, EncodingUtility.ToString(encoding));
            builder.AddValue(Column.DelayTime, fileWatchParameter.DelayTime);
            builder.AddValue(Column.BufferSize, fileWatchParameter.BufferSize);
            builder.AddValue(Column.RefreshTime, fileWatchParameter.RefreshTime);
            builder.AddValue(Column.IsEnabledRefresh, fileWatchParameter.IsEnabledRefresh);

            return ExecuteUpdate(builder) == 1;
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
