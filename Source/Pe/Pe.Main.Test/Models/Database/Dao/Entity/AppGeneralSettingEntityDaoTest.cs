using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
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
            var test = Test.BuildDao<AppGeneralSettingEntityDao>(AccessorKind.Main);

            var actual = test.SelectSettingGeneralSetting();
            Assert.Empty(actual.Language);
            Assert.Empty(actual.UserBackupDirectoryPath);
            Assert.NotEqual(PluginId.Empty, actual.ThemePluginId);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppGeneralSettingEntityDao>(AccessorKind.Main);

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


        [Fact]
        public void Select_Latest_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppGeneralSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var entityDaoHelper = new EntityDaoHelper(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "-10",
                    ["Language"] = "'OLD'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "20",
                    ["Language"] = "'NEW'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            var actual = test.SelectLanguage();
            Assert.Equal("NEW", actual);
        }

        #endregion
    }
}
