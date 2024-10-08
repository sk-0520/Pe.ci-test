using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemsEntityDao: EntityDaoBase
    {
        #region define

        private interface IReadOnlyLauncherItemsRowDto: IReadOnlyRowDtoBase
        {
            #region property

            LauncherItemId LauncherItemId { get; }

            string Name { get; }
            string Kind { get; }
            string IconPath { get; }
            long IconIndex { get; }
            bool IsEnabledCommandLauncher { get; }
            long ExecuteCount { get; }
            [DateTimeKind(DateTimeKind.Utc)]
            DateTime LastExecuteTimestamp { get; }
            string Comment { get; }

            #endregion
        }

        private sealed class LauncherItemsRowDto: RowDtoBase, IReadOnlyLauncherItemsRowDto
        {
            #region IReadOnlyLauncherItemRowDto

            public LauncherItemId LauncherItemId { get; set; }

            public string Name { get; set; } = string.Empty;
            public string Kind { get; set; } = string.Empty;
            public string IconPath { get; set; } = string.Empty;
            public long IconIndex { get; set; }
            public bool IsEnabledCommandLauncher { get; set; }
            public long ExecuteCount { get; set; }
            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime LastExecuteTimestamp { get; set; }
            public string Comment { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        public LauncherItemsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherItemsRowDto ConvertFromData(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var dto = new LauncherItemsRowDto() {
                LauncherItemId = data.LauncherItemId,
                Name = data.Name,
                Kind = kindEnumTransfer.ToString(data.Kind),
                IconPath = data.Icon.Path,
                IconIndex = data.Icon.Index,
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                Comment = data.Comment,
            };

            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        private LauncherItemData ConvertFromDto(IReadOnlyLauncherItemsRowDto dto)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var data = new LauncherItemData() {
                LauncherItemId = dto.LauncherItemId,
                Name = dto.Name,
                Kind = kindEnumTransfer.ToEnum(dto.Kind),
                IsEnabledCommandLauncher = dto.IsEnabledCommandLauncher,
                Icon = new IconData() {
                    Path = dto.IconPath,
                    Index = ToInt(dto.IconIndex),
                },
                Comment = dto.Comment,
            };

            return data;
        }

        public IEnumerable<LauncherItemId> SelectAllLauncherItemIds()
        {
            var statement = LoadStatement();
            return Context.Query<LauncherItemId>(statement);
        }

        public LauncherItemData SelectLauncherItem(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QuerySingle<LauncherItemsRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public IEnumerable<LauncherItemData> SelectApplicationLauncherItems()
        {
            var statement = LoadStatement();
            return Context.Query<LauncherItemsRowDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool SelectExistsLauncherItem(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.QueryFirst<bool>(statement, parameter);
        }

        public void InsertLauncherItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void UpdateExecuteCountIncrement(LauncherItemId launcherItemId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;

            Context.UpdateByKey(statement, param);
        }

        public bool UpdateCustomizeLauncherItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            return Context.UpdateByKeyOrNothing(statement, dto);
        }

        public void DeleteLauncherItem(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
