using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppGeneralSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectSettingGeneralSettingTest()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppGeneralSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            var actual = test.SelectSettingGeneralSetting();
            Assert.Empty(actual.Language);
            Assert.Empty(actual.UserBackupDirectoryPath);
            Assert.NotEqual(PluginId.Empty, actual.ThemePluginId);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppGeneralSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            var expected = new {
                Language = "Language",
                ThemePluginId = PluginId.NewId(),
                UserBackupDirectoryPath = "UserBackupDirectoryPath",
            };

            test.UpdateSettingGeneralSetting(new SettingAppGeneralSettingData() {
                Language = expected.Language,
                ThemePluginId = expected.ThemePluginId,
                UserBackupDirectoryPath = expected.UserBackupDirectoryPath,
            }, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = new {
                Language = test.SelectLanguage(),
                ThemePluginId = test.SelectThemePluginId(),
                UserBackupDirectoryPath = test.SelectUserBackupDirectoryPath(),
            };

            Assert.Equal(expected, actual);

        }

        #endregion
    }
}
