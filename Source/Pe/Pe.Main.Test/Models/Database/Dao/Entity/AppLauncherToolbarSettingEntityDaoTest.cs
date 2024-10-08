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
    public class AppLauncherToolbarSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectAppLauncherToolbarSettingFontIdTest()
        {
            var test = Test.BuildDao<AppLauncherToolbarSettingEntityDao>(AccessorKind.Main);

            var actual = test.SelectAppLauncherToolbarSettingFontId();
            Assert.NotEqual(FontId.Empty, actual);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppLauncherToolbarSettingEntityDao>(AccessorKind.Main);

            var expected = new AppLauncherToolbarSettingData() {
                ContentDropMode = LauncherToolbarContentDropMode.ExtendsExecute,
                ShortcutDropMode = LauncherToolbarShortcutDropMode.Target,
                GroupMenuPosition = LauncherGroupPosition.Bottom,
            };

            test.UpdateSettingLauncherToolbarSetting(expected, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = test.SelectSettingLauncherToolbarSetting();

            Assert.Equal(expected.ContentDropMode, actual.ContentDropMode);
            Assert.Equal(expected.ShortcutDropMode, actual.ShortcutDropMode);
            Assert.Equal(expected.GroupMenuPosition, actual.GroupMenuPosition);
        }


        [Fact]
        public void Select_Latest_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppLauncherToolbarSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var entityDaoHelper = new EntityDaoHelper(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "-10",
                    ["GroupMenuPosition"] = "'top'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "20",
                    ["GroupMenuPosition"] = "'bottom'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            var actual = test.SelectSettingLauncherToolbarSetting();
            Assert.Equal(LauncherGroupPosition.Bottom, actual.GroupMenuPosition);
        }

        #endregion
    }
}
