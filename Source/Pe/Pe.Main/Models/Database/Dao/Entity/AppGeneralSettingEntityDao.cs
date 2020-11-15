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
    public class AppGeneralSettingEntityDao: EntityDaoBase
    {
        #region define

        private class AppGeneralSettingEntityDto: CommonDtoBase
        {
            #region property
            public string Language { get; set; } = string.Empty;
            public string UserBackupDirectoryPath { get; set; } = string.Empty;
            public Guid ThemePluginId { get; set; }
            #endregion
        }


        #endregion

        public AppGeneralSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
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
            var dto = Context.QueryFirst<AppGeneralSettingEntityDto>(statement);
            var result = new SettingAppGeneralSettingData() {
                Language = dto.Language,
                UserBackupDirectoryPath = dto.UserBackupDirectoryPath,
                ThemePluginId = dto.ThemePluginId,
            };
            return result;
        }

        public string SelectLanguage()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<string>(statement);
        }

        public string SelectUserBackupDirectoryPath()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<string>(statement);
        }

        public Guid SelectThemePluginId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<Guid>(statement);
        }

        public bool UpdateSettingGeneralSetting(SettingAppGeneralSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppGeneralSettingEntityDto() {
                Language = data.Language,
                UserBackupDirectoryPath = data.UserBackupDirectoryPath,
                ThemePluginId = data.ThemePluginId,
            };
            commonStatus.WriteCommon(dto);
            return Context.Execute(statement, dto) == 1;
        }



        #endregion
    }
}
