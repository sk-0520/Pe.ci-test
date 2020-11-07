using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherStoreAppsEntityDto : CommonDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }

        public string ProtocolAlias { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;

        #endregion
    }

    public class LauncherStoreAppsEntityDao : EntityDaoBase
    {
        public LauncherStoreAppsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        static class Column
        {
            #region property
            #endregion
        }

        #endregion

        #region function

        LauncherStoreAppsEntityDto ConvertFromData(Guid launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var dto = new LauncherStoreAppsEntityDto() {
                LauncherItemId = launcherItemId,
                ProtocolAlias = data.ProtocolAlias,
                Option = data.Option,
            };
            commonStatus.WriteCommon(dto);
            return dto;
        }

        LauncherStoreAppData ConvertFromDto(LauncherStoreAppsEntityDto dto)
        {
            var data = new LauncherStoreAppData() {
                ProtocolAlias = dto.ProtocolAlias,
                Option = dto.Option,
            };
            return data;
        }

        public LauncherStoreAppData SelectStoreApp(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QueryFirst<LauncherStoreAppsEntityDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public bool InsertStoreApp(Guid launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, commonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool UpdateStoreApp(Guid launcherItemId, LauncherStoreAppData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, commonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
