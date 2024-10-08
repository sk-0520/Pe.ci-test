using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherEnvVarsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testEnv = Test.BuildDao<LauncherEnvVarsEntityDao>(AccessorKind.Main);
            var data = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "Data",
                Kind = LauncherItemKind.File,
            };
            testItem.InsertLauncherItem(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Empty(testEnv.SelectEnvVarItems(data.LauncherItemId));

            testEnv.InsertEnvVarItems(
                data.LauncherItemId,
                new[] {
                    new LauncherEnvironmentVariableData() {
                        Name = "Name1",
                        Value = "Value1",
                    },
                    new LauncherEnvironmentVariableData() {
                        Name = "Name2",
                        Value = "Value2",
                    },
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actualEnvItems = testEnv.SelectEnvVarItems(data.LauncherItemId).ToArray();
            Assert.Equal("Name1", actualEnvItems[0].Name);
            Assert.Equal("Value1", actualEnvItems[0].Value);
            Assert.Equal("Name2", actualEnvItems[1].Name);
            Assert.Equal("Value2", actualEnvItems[1].Value);

            testEnv.DeleteEnvVarItemsByLauncherItemId(data.LauncherItemId);
            Assert.Empty(testEnv.SelectEnvVarItems(data.LauncherItemId));
        }

        #endregion

    }
}
