using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class AppCommandSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string IconBox { get; set; } = string.Empty;
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }
        public bool FindFile { get; set; }

        #endregion
    }

    public class AppCommandSettingEntityDao : EntityDaoBase
    {
        public AppCommandSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        public Guid SelectCommandSettingFontId()
        {
            var statement = LoadStatement();
            return Commander.QueryFirst<Guid>(statement);
        }


        public SettingAppCommandSettingData SelectSettingCommandSetting()
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppCommandSettingEntityDto>(statement);
            var result = new SettingAppCommandSettingData() {
                FontId = dto.FontId,
                IconBox = iconBoxTransfer.ToEnum(dto.IconBox),
                HideWaitTime = dto.HideWaitTime,
                FindTag = dto.FindTag,
                FindFile = dto.FindFile,
            };
            return result;
        }


        public bool UpdateSettingCommandSetting(SettingAppCommandSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = new AppCommandSettingEntityDto() {
                FontId = data.FontId,
                IconBox = iconBoxTransfer.ToString(data.IconBox),
                HideWaitTime = data.HideWaitTime,
                FindTag = data.FindTag,
                FindFile = data.FindFile,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }


        #endregion
    }
}
