using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemsEntityDao : EntityDaoBase
    {
        #region define

        private interface IReadOnlyLauncherItemsRowDto: IReadOnlyRowDtoBase
        {
            #region property

            Guid LauncherItemId { get; }

            string Code { get; }
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

        private class LauncherItemsRowDto: RowDtoBase, IReadOnlyLauncherItemsRowDto
        {
            #region IReadOnlyLauncherItemRowDto

            public Guid LauncherItemId { get; set; }

            public string Code { get; set; } = string.Empty;
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

        #endregion

        public LauncherItemsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation , loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        #region function

        LauncherItemsRowDto ConvertFromData(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var dto = new LauncherItemsRowDto() {
                LauncherItemId = data.LauncherItemId,
                Code = data.Code,
                Name = data.Name,
                Kind = kindEnumTransfer.ToString(data.Kind),
                IconPath = data.Icon.Path,
                IconIndex = data.Icon.Index,
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                Comment = data.Comment,
            };

            commonStatus.WriteCommon(dto);

            return dto;
        }

        LauncherItemData ConvertFromDto(IReadOnlyLauncherItemsRowDto dto)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var data = new LauncherItemData() {
                LauncherItemId = dto.LauncherItemId,
                Name = dto.Name,
                Code = dto.Code,
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

        /// <summary>
        /// 同じようなコード一覧を取得。
        /// </summary>
        /// <param name="baseCode"></param>
        /// <returns></returns>
        public IEnumerable<string> SelectFuzzyCodes(string baseCode)
        {
            var statement = LoadStatement();
            return Context.Query<string>(statement, new { BaseCode = baseCode });
        }

        public IEnumerable<Guid> SelectAllLauncherItemIds()
        {
            var statement = LoadStatement();
            return Context.Query<Guid>(statement);
        }

        public LauncherItemData SelectLauncherItem(Guid launcherItemId)
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

        public bool SelectExistsLauncherItem(Guid launcherItemId)
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
            Context.Execute(statement, dto);
        }

        public bool UpdateExecuteCountIncrement(Guid launcherItemId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateCustomizeLauncherItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool DeleteLauncherItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
