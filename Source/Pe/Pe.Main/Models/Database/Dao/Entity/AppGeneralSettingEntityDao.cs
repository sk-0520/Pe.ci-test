using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppGeneralSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppGeneralSettingEntityDto: CommonDtoBase
        {
            #region property

            public string Language { get; set; } = string.Empty;
            public string UserBackupDirectoryPath { get; set; } = string.Empty;
            public PluginId ThemePluginId { get; set; }

            #endregion
        }


        #endregion

        public AppGeneralSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        private static class Column
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

        public PluginId SelectThemePluginId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<PluginId>(statement);
        }

        public void UpdateSettingGeneralSetting(SettingAppGeneralSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppGeneralSettingEntityDto() {
                Language = data.Language,
                UserBackupDirectoryPath = data.UserBackupDirectoryPath,
                ThemePluginId = data.ThemePluginId,
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }

        #endregion
    }
}
