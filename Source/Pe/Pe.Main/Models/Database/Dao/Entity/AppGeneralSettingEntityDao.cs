using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class AppGeneralSettingEntityDto : CommonDtoBase
    {
        #region property
        public string Language { get; set; } = string.Empty;
        public string UserBackupDirectoryPath { get; set; } = string.Empty;
        #endregion
    }

    public class AppGeneralSettingEntityDao : EntityDaoBase
    {
        public AppGeneralSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppGeneralSettingData SelectSettingGeneralSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppGeneralSettingEntityDto>(statement);
            var result = new SettingAppGeneralSettingData() {
                Language = dto.Language,
                UserBackupDirectoryPath = dto.UserBackupDirectoryPath,
            };
            return result;
        }

        public string SelectLanguage()
        {
            var statement = LoadStatement();
            return Commander.QueryFirst<string>(statement);
        }
        public string SelectUserBackupDirectoryPath()
        {
            var statement = LoadStatement();
            return Commander.QueryFirst<string>(statement);
        }

        public bool UpdateSettingGeneralSetting(SettingAppGeneralSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppGeneralSettingEntityDto() {
                Language = data.Language,
                UserBackupDirectoryPath = data.UserBackupDirectoryPath,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }



        #endregion
    }
}
