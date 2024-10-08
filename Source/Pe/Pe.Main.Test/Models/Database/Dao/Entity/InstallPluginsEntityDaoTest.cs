using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class InstallPluginsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Select_NotFound_Test()
        {
            var test = Test.BuildDao<InstallPluginsEntityDao>(AccessorKind.Temporary);
            var actual1 = test.SelectExistsInstallPluginByPluginId(PluginId.Empty);
            Assert.False(actual1);

            var actual2 = test.SelectExistsInstallPlugin();
            Assert.False(actual2);
        }

        [Fact]
        public void Insert_Select_Test()
        {
            var test = Test.BuildDao<InstallPluginsEntityDao>(AccessorKind.Temporary);
            var data = new PluginInstallData(
                PluginId.NewId(),
                "PluginName",
                new Version(1,2,3),
                PluginInstallMode.New,
                "extract",
                "install"
            );
            test.InsertInstallPlugin(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual1 = test.SelectExistsInstallPluginByPluginId(data.PluginId);
            Assert.True(actual1);

            var actual2 = test.SelectExistsInstallPlugin();
            Assert.True(actual2);
        }

        [Fact]
        public void Insert_Delete_Select_Test()
        {
            var test = Test.BuildDao<InstallPluginsEntityDao>(AccessorKind.Temporary);
            var data = new PluginInstallData(
                PluginId.NewId(),
                "PluginName",
                new Version(1, 2, 3),
                PluginInstallMode.New,
                "extract",
                "install"
            );
            Assert.Empty(test.SelectInstallPlugins());

            test.InsertInstallPlugin(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual1 = test.SelectExistsInstallPluginByPluginId(data.PluginId);
            Assert.True(actual1);

            var actual2 = test.SelectExistsInstallPlugin();
            Assert.True(actual2);

            var actual5 = test.SelectInstallPlugins();
            Assert.Single(actual5);
            Assert.Equal(data.PluginId, actual5.Single().PluginId);
            Assert.Equal("PluginName", actual5.Single().PluginName);

            test.DeleteInstallPlugin(data.PluginId);

            var actual3 = test.SelectExistsInstallPluginByPluginId(data.PluginId);
            Assert.False(actual3);

            var actual4 = test.SelectExistsInstallPlugin();
            Assert.False(actual4);
        }

        #endregion
    }
}
