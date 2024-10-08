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
    public class PluginWidgetSettingsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testPlugins = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var testSettings = Test.BuildDao<PluginWidgetSettingsEntityDao>(AccessorKind.Main);

            var pluginId = PluginId.NewId();

            testPlugins.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "PluginName",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.False(testSettings.SelectExistsPluginWidgetSetting(pluginId));
            Assert.False(testSettings.SelectPluginWidgetTopmost(pluginId));

            testSettings.InsertPluginWidgetTopmost(pluginId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(testSettings.SelectExistsPluginWidgetSetting(pluginId));
            Assert.True(testSettings.SelectPluginWidgetTopmost(pluginId));

            var actual1 = testSettings.SelectPluginWidgetSetting(pluginId);
            Assert.True(double.IsNaN(actual1.X));
            Assert.True(double.IsNaN(actual1.Y));
            Assert.True(double.IsNaN(actual1.Width));
            Assert.True(double.IsNaN(actual1.Height));
            Assert.False(actual1.IsVisible);
            Assert.False(actual1.IsTopmost);

            Assert.Equal(1, testSettings.DeletePluginWidgetSettingsByPluginId(pluginId));
            Assert.Equal(0, testSettings.DeletePluginWidgetSettingsByPluginId(pluginId));
            Assert.False(testSettings.SelectExistsPluginWidgetSetting(pluginId));
            Assert.False(testSettings.SelectPluginWidgetTopmost(pluginId));

            testSettings.InsertPluginWidgetSetting(
                pluginId,
                new PluginWidgetSettingData() {
                    X = 10,
                    Y = 20,
                    Width =30,
                    Height = 40,
                    IsTopmost = true,
                    IsVisible = true,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual2 = testSettings.SelectPluginWidgetSetting(pluginId);
            Assert.Equal(10, actual2.X);
            Assert.Equal(20, actual2.Y);
            Assert.Equal(30, actual2.Width);
            Assert.Equal(40, actual2.Height);
            Assert.True(actual2.IsVisible);
            Assert.False(actual2.IsTopmost);
            Assert.True(testSettings.SelectExistsPluginWidgetSetting(pluginId));
            Assert.True(testSettings.SelectPluginWidgetTopmost(pluginId));

            testSettings.UpdatePluginWidgetSetting(
                pluginId,
                new PluginWidgetSettingData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                    IsTopmost = true,
                    IsVisible = false,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual3 = testSettings.SelectPluginWidgetSetting(pluginId);
            Assert.Equal(100, actual3.X);
            Assert.Equal(200, actual3.Y);
            Assert.Equal(300, actual3.Width);
            Assert.Equal(400, actual3.Height);
            Assert.False(actual3.IsVisible);
            Assert.False(actual3.IsTopmost);
            Assert.True(testSettings.SelectExistsPluginWidgetSetting(pluginId));
            Assert.True(testSettings.SelectPluginWidgetTopmost(pluginId));

            testSettings.UpdatePluginWidgetTopmost(pluginId, false, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(testSettings.SelectPluginWidgetTopmost(pluginId));

            Assert.Equal(1, testSettings.DeletePluginWidgetSettingsByPluginId(pluginId));
            Assert.Equal(0, testSettings.DeletePluginWidgetSettingsByPluginId(pluginId));
        }

        #endregion
    }
}
