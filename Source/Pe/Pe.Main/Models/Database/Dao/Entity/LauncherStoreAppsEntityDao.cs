using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherStoreAppsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherStoreAppsEntityDto: CommonDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }

            public string ProtocolAlias { get; set; } = string.Empty;
            public string Option { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property
            #endregion
        }

        #endregion

        public LauncherStoreAppsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherStoreAppsEntityDto ConvertFromData(LauncherItemId launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var dto = new LauncherStoreAppsEntityDto() {
                LauncherItemId = launcherItemId,
                ProtocolAlias = data.ProtocolAlias,
                Option = data.Option,
            };
            commonStatus.WriteCommonTo(dto);
            return dto;
        }

        private LauncherStoreAppData ConvertFromDto(LauncherStoreAppsEntityDto dto)
        {
            var data = new LauncherStoreAppData() {
                ProtocolAlias = dto.ProtocolAlias,
                Option = dto.Option,
            };
            return data;
        }

        public LauncherStoreAppData SelectStoreApp(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QueryFirst<LauncherStoreAppsEntityDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public void InsertStoreApp(LauncherItemId launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, commonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void UpdateStoreApp(LauncherItemId launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, commonStatus);

            Context.UpdateByKey(statement, dto);
        }

        public bool DeleteStoreApp(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }


        #endregion
    }
}
