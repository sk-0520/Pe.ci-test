using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class AppUpdateSettingEntityDto : CommonDtoBase
    {
        #region property

        public string UpdateKind { get; set; } = string.Empty;
        public Version IgnoreVersion { get; set; } = new Version(0, 0, 0, 0);

        #endregion
    }

    public class AppUpdateSettingEntityDao : EntityDaoBase
    {
        public AppUpdateSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string UpdateKind => "UpdateKind";

            #endregion
        }

        #endregion

        #region function

        public SettingAppUpdateSettingData SelectSettingUpdateSetting()
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppUpdateSettingEntityDto>(statement);
            var result = new SettingAppUpdateSettingData() {
                UpdateKind = updateKindTransfer.ToEnum(dto.UpdateKind),
            };
            return result;
        }


        public bool UpdateSettingUpdateSetting(SettingAppUpdateSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var dto = new AppUpdateSettingEntityDto() {
                UpdateKind = updateKindTransfer.ToString(data.UpdateKind),
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool UpdateReleaseVersion(UpdateKind updateKind, IDatabaseCommonStatus commonStatus)
        {
            var updateKindTransfer = new EnumTransfer<UpdateKind>();

            var statement = LoadStatement();
            var dto = new AppUpdateSettingEntityDto() {
                UpdateKind = updateKindTransfer.ToString(updateKind),
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }


        #endregion
    }
}
