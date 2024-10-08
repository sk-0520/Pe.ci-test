using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.VisualBasic.ApplicationServices;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppExecuteSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectSettingExecuteSettingTest()
        {
            var test = Test.BuildDao<AppExecuteSettingEntityDao>(AccessorKind.Main);
            var actual = test.SelectSettingExecuteSetting();
            Assert.False(actual.IsEnabledTelemetry);
            Assert.Empty(actual.UserId);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppExecuteSettingEntityDao>(AccessorKind.Main);

            var expected = new {
                IsEnabledTelemetry = true,
                UserId = "UserId",
            };

            test.UpdateSettingExecuteSetting(new SettingAppExecuteSettingData() {
                IsEnabledTelemetry = expected.IsEnabledTelemetry,
                UserId = expected.UserId,
            }, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = test.SelectSettingExecuteSetting();

            Assert.Equal(expected.IsEnabledTelemetry, actual.IsEnabledTelemetry);
            Assert.Equal(expected.UserId, actual.UserId);
        }

        [Fact]
        public void UpdateExecuteSettingAcceptInputTest()
        {
            var test = Test.BuildDao<AppExecuteSettingEntityDao>(AccessorKind.Main);

            var expected = new {
                IsEnabledTelemetry = true,
                UserId = "UserId",
            };

            test.UpdateExecuteSettingAcceptInput(
                expected.UserId,
                expected.IsEnabledTelemetry,
                Test.DiContainer.New<IDatabaseCommonStatus>()
            );

            var actual = test.SelectSettingExecuteSetting();

            Assert.Equal(expected.IsEnabledTelemetry, actual.IsEnabledTelemetry);
            Assert.Equal(expected.UserId, actual.UserId);
        }


        [Fact]
        public void Select_Latest_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppExecuteSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var entityDaoHelper = new EntityDaoHelper(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "-10",
                    ["UserId"] = "'OLD'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "20",
                    ["UserId"] = "'NEW'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            var actual = test.SelectSettingExecuteSetting();
            Assert.Equal("NEW", actual.UserId);
        }

        #endregion
    }
}
