using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class PluginLauncherItemSettingsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testPlugins = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var testSettings = Test.BuildDao<PluginLauncherItemSettingsEntityDao>(AccessorKind.Main);

            var pluginId = PluginId.NewId();
            var launcherItemId = LauncherItemId.NewId();

            Assert.Empty(testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.False(testSettings.SelectExistsPluginLauncherItemSetting(pluginId, launcherItemId, string.Empty));
            Assert.Null(testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, string.Empty));

            testPlugins.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testItems.InsertLauncherItem(
                new LauncherItemData() {
                    LauncherItemId = launcherItemId,
                    Kind = LauncherItemKind.File
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testSettings.InsertPluginLauncherItemSetting(
                pluginId,
                launcherItemId,
                string.Empty,
                new PluginSettingRawValue(
                    PluginPersistenceFormat.Text,
                    "EMPTY"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Contains(string.Empty, testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.True(testSettings.SelectExistsPluginLauncherItemSetting(pluginId, launcherItemId, string.Empty));
            Assert.Equal("EMPTY", testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, string.Empty)!.Value);

            testSettings.InsertPluginLauncherItemSetting(
                pluginId,
                launcherItemId,
                "key",
                new PluginSettingRawValue(
                    PluginPersistenceFormat.Text,
                    "KEY"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Contains(string.Empty, testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.Contains("key", testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.True(testSettings.SelectExistsPluginLauncherItemSetting(pluginId, launcherItemId, "key"));
            Assert.Equal("KEY", testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, "key")!.Value);

            testSettings.InsertPluginLauncherItemSetting(
                pluginId,
                launcherItemId,
                "key2",
                new PluginSettingRawValue(
                    PluginPersistenceFormat.Text,
                    "KEY2"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Contains(string.Empty, testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.Contains("key2", testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.True(testSettings.SelectExistsPluginLauncherItemSetting(pluginId, launcherItemId, "key2"));
            Assert.Equal("KEY2", testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, "key2")!.Value);

            testSettings.UpdatePluginLauncherItemSetting(
                pluginId,
                launcherItemId,
                string.Empty,
                new PluginSettingRawValue(
                    PluginPersistenceFormat.Text,
                    "EMPTY2"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Equal("EMPTY2", testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, string.Empty)!.Value);

            Assert.False(testSettings.DeletePluginLauncherItemSetting(pluginId, launcherItemId, "key3"));
            Assert.True(testSettings.DeletePluginLauncherItemSetting(pluginId, launcherItemId, "key2"));
            Assert.Contains(string.Empty, testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.DoesNotContain("key2", testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.DoesNotContain("key3", testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));

            Assert.Equal(2, testSettings.DeletePluginLauncherItemSettingsByPluginId(pluginId));

            Assert.Empty(testSettings.SelectPluginLauncherItemSettingKeys(pluginId, launcherItemId));
            Assert.False(testSettings.SelectExistsPluginLauncherItemSetting(pluginId, launcherItemId, string.Empty));
            Assert.Null(testSettings.SelectPluginLauncherItemValue(pluginId, launcherItemId, string.Empty));
        }

        #endregion
    }
}
