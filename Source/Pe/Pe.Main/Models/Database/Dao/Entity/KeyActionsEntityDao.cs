using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyActionsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class KeyActionsEntityDto: CommonDtoBase
        {
            #region property

            public KeyActionId KeyActionId { get; set; }
            public string KeyActionKind { get; set; } = string.Empty;
            public string KeyActionContent { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string KeyActionId { get; } = "KeyActionId";
            public static string KeyActionKind { get; } = "ActionKind";
            public static string KeyActionContent { get; } = "KeyActionContent";
            public static string KeyActionOption { get; } = "KeyActionOption";
            public static string Comment { get; } = "Comment";

            #endregion
        }

        #endregion

        public KeyActionsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private KeyActionData ConvertFromDto(KeyActionsEntityDto dto)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var result = new KeyActionData() {
                KeyActionId = dto.KeyActionId,
                KeyActionContent = dto.KeyActionContent,
                KeyActionKind = keyActionKindTransfer.ToEnum(dto.KeyActionKind),
                Comment = dto.Comment,
            };

            return result;
        }

        private KeyActionsEntityDto ConvertFromData(KeyActionData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var dto = new KeyActionsEntityDto() {
                KeyActionId = data.KeyActionId,
                KeyActionKind = keyActionKindTransfer.ToString(data.KeyActionKind),
                KeyActionContent = data.KeyActionContent,
                Comment = data.Comment,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public IEnumerable<KeyActionData> SelectAllKeyActionsFromKind(KeyActionKind keyActionKind)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new { KeyActionKind = keyActionKindTransfer.ToString(keyActionKind) };
            return Context.Query<KeyActionsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public IEnumerable<KeyActionData> SelectAllKeyActionsIgnoreKinds(IReadOnlyCollection<KeyActionKind> ignoreKinds)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new { IgnoreKinds = ignoreKinds.Select(i => keyActionKindTransfer.ToString(i)) };
            return Context.Query<KeyActionsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public void InsertKeyAction(KeyActionData keyActionData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(keyActionData, databaseCommonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void UpdateKeyAction(KeyActionData keyActionData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(keyActionData, databaseCommonStatus);

            Context.UpdateByKey(statement, dto);
        }

        public void UpdateUsageCountIncrement(KeyActionId keyActionId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.KeyActionId] = keyActionId;

            Context.UpdateByKey(statement, parameter);
        }

        public void DeleteKeyAction(KeyActionId keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new { KeyActionId = keyActionId };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
