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
    public class LauncherAddonsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testAddon = Test.BuildDao<LauncherAddonsEntityDao>(AccessorKind.Main);
            var testPlugin = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);

            var launcherItems = new {
                A = new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "A",
                    Kind = LauncherItemKind.Addon,
                },
                B = new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "B",
                    Kind = LauncherItemKind.Addon,
                },
                C = new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "C",
                    Kind = LauncherItemKind.Addon,
                },
            };
            testItem.InsertLauncherItem(launcherItems.A, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testItem.InsertLauncherItem(launcherItems.B, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testItem.InsertLauncherItem(launcherItems.C, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var pluginId = new {
                AB = PluginId.NewId(),
                C = PluginId.NewId(),
            };

            testPlugin.InsertPluginStateData(new PluginStateData() { PluginId = pluginId.AB, PluginName = "AB", State = PluginState.Enable }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testPlugin.InsertPluginStateData(new PluginStateData() { PluginId = pluginId.C, PluginName = "C", State = PluginState.Enable }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testAddon.InsertAddonPluginId(launcherItems.A.LauncherItemId, pluginId.AB, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testAddon.InsertAddonPluginId(launcherItems.B.LauncherItemId, pluginId.AB, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testAddon.InsertAddonPluginId(launcherItems.C.LauncherItemId, pluginId.C, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Equal(pluginId.AB, testAddon.SelectAddonPluginId(launcherItems.A.LauncherItemId));
            Assert.Equal(pluginId.AB, testAddon.SelectAddonPluginId(launcherItems.B.LauncherItemId));
            Assert.Equal(pluginId.C, testAddon.SelectAddonPluginId(launcherItems.C.LauncherItemId));

            Assert.Contains(launcherItems.A.LauncherItemId, testAddon.SelectLauncherItemIdsByPluginId(pluginId.AB));
            Assert.Contains(launcherItems.B.LauncherItemId, testAddon.SelectLauncherItemIdsByPluginId(pluginId.AB));
            Assert.Single(testAddon.SelectLauncherItemIdsByPluginId(pluginId.C));
            Assert.Contains(launcherItems.C.LauncherItemId, testAddon.SelectLauncherItemIdsByPluginId(pluginId.C));

            testAddon.DeleteLauncherAddonsByPluginId(pluginId.C);
            Assert.Empty(testAddon.SelectLauncherItemIdsByPluginId(pluginId.C));

            testAddon.DeleteLauncherAddonsByPluginId(pluginId.AB);
            Assert.Empty(testAddon.SelectLauncherItemIdsByPluginId(pluginId.AB));
        }

        #endregion
    }
}
