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
    public class PluginVersionChecksEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testPlugins = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var testChecks = Test.BuildDao<PluginVersionChecksEntityDao>(AccessorKind.Main);

            var pluginId = PluginId.NewId();

            Assert.Empty(testChecks.SelectPluginVersionCheckUrls(pluginId));
            Assert.Equal(0, testChecks.DeletePluginVersionChecks(pluginId));

            testPlugins.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "PluginName",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testChecks.InsertPluginVersionCheckUrl(
                pluginId,
                10,
                "URL1",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Single(testChecks.SelectPluginVersionCheckUrls(pluginId));

            testChecks.InsertPluginVersionCheckUrl(
                pluginId,
                20,
                "URL2",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testChecks.SelectPluginVersionCheckUrls(pluginId).ToArray();
            Assert.Equal(2, actual.Length);
            Assert.Equal("URL1", actual[0]);
            Assert.Equal("URL2", actual[1]);

            Assert.Equal(2, testChecks.DeletePluginVersionChecks(pluginId));
            Assert.Equal(0, testChecks.DeletePluginVersionChecks(pluginId));
        }

        #endregion
    }
}
