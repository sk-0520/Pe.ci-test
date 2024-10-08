using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppCommandSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectCommandSettingFontIdTest()
        {
            var test = Test.BuildDao<AppCommandSettingEntityDao>(AccessorKind.Main);
            var actual = test.SelectCommandSettingFontId();
            Assert.NotEqual(FontId.Empty, actual);
        }

        [Fact]
        public void SelectSettingCommandSettingTest()
        {
            var test = Test.BuildDao<AppCommandSettingEntityDao>(AccessorKind.Main);
            var actual = test.SelectSettingCommandSetting();
            Assert.NotEqual(FontId.Empty, actual.FontId);
            Assert.True(0 < actual.Width);
            Assert.True(TimeSpan.Zero < actual.HideWaitTime);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppCommandSettingEntityDao>(AccessorKind.Main);

            var expected = new {
                FontId = test.SelectCommandSettingFontId(),
                Width = 123,
                HideWaitTime = new TimeSpan(1, 2, 3, 4, 5, 6),
                IconBox = IconBox.Large,
            };

            test.UpdateSettingCommandSetting(new SettingAppCommandSettingData() {
                FontId = expected.FontId,
                Width = expected.Width,
                HideWaitTime = expected.HideWaitTime,
                IconBox = expected.IconBox,
            }, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = test.SelectSettingCommandSetting();

            Assert.Equal(expected.FontId, actual.FontId);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.HideWaitTime, actual.HideWaitTime);
            Assert.Equal(expected.IconBox, actual.IconBox);
        }

        [Fact]
        public void UpdateExecuteSettingAcceptInputTest()
        {
            var test = Test.BuildDao<AppCommandSettingEntityDao>(AccessorKind.Main);

            var current = test.SelectSettingCommandSetting();
            var expected = current.Width + 123;
            test.UpdateCommandSettingWidth(expected, Test.DiContainer.New<IDatabaseCommonStatus>());

            var next = test.SelectSettingCommandSetting();
            Assert.Equal(expected, next.Width);

        }


        [Fact]
        public void Select_Latest_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppCommandSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var entityDaoHelper = new EntityDaoHelper(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "-10",
                    ["Width"] = "-100"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "20",
                    ["Width"] = "200"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            var actual = test.SelectSettingCommandSetting();
            Assert.Equal(200, actual.Width);
        }

        #endregion
    }
}
