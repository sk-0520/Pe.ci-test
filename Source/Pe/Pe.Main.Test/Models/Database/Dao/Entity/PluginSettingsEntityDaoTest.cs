using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class PluginSettingsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testPlugins = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var testSettings = Test.BuildDao<PluginSettingsEntityDao>(AccessorKind.Main);

            var pluginId = PluginId.NewId();
            var launcherItemId = LauncherItemId.NewId();

            Assert.Empty(testSettings.SelectPluginSettingKeys(pluginId));
            Assert.False(testSettings.SelectExistsPluginSetting(pluginId, string.Empty));
            Assert.Null(testSettings.SelectPluginSettingValue(pluginId, string.Empty));

            testPlugins.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "PluginName",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testSettings.InsertPluginSetting(
                pluginId,
                string.Empty,
                new PluginSettingRawValue(
                    Bridge.Plugin.PluginPersistenceFormat.Text,
                    "EMPTY"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testSettings.InsertPluginSetting(
                pluginId,
                "key1",
                new PluginSettingRawValue(
                    Bridge.Plugin.PluginPersistenceFormat.Text,
                    "KEY1"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testSettings.InsertPluginSetting(
                pluginId,
                "key2",
                new PluginSettingRawValue(
                    Bridge.Plugin.PluginPersistenceFormat.Text,
                    "KEY2"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actualKeys = testSettings.SelectPluginSettingKeys(pluginId).ToArray();
            Assert.Equal(3, actualKeys.Length);
            Assert.Contains(string.Empty, actualKeys);
            Assert.Contains("key1", actualKeys);
            Assert.Contains("key2", actualKeys);
            Assert.DoesNotContain("key3", actualKeys);

            Assert.True(testSettings.SelectExistsPluginSetting(pluginId, string.Empty));
            Assert.True(testSettings.SelectExistsPluginSetting(pluginId, "key1"));
            Assert.True(testSettings.SelectExistsPluginSetting(pluginId, "key2"));

            Assert.Equal("EMPTY", testSettings.SelectPluginSettingValue(pluginId, string.Empty)!.Value);
            Assert.Equal("KEY1", testSettings.SelectPluginSettingValue(pluginId, "key1")!.Value);
            Assert.Equal("KEY2", testSettings.SelectPluginSettingValue(pluginId, "key2")!.Value);
            Assert.Null(testSettings.SelectPluginSettingValue(pluginId, "key3"));

            testSettings.UpdatePluginSetting(
                pluginId,
                string.Empty,
                new PluginSettingRawValue(
                    Bridge.Plugin.PluginPersistenceFormat.Text,
                    "EMPTY2"
                ),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            Assert.Equal("EMPTY2", testSettings.SelectPluginSettingValue(pluginId, string.Empty)!.Value);

            Assert.True(testSettings.DeletePluginSetting(pluginId, "key2"));
            Assert.False(testSettings.DeletePluginSetting(pluginId, "key3"));

            Assert.Equal(2, testSettings.SelectPluginSettingKeys(pluginId).Count());

            Assert.Equal(2, testSettings.DeleteAllPluginSettings(pluginId));
            Assert.Empty(testSettings.SelectPluginSettingKeys(pluginId));
        }

        #endregion
    }
}
