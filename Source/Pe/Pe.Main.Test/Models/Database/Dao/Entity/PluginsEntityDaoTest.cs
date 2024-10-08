using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class PluginsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var pluginId = PluginId.NewId();

            Assert.False(test.SelectExistsPlugin(pluginId));
            Assert.Null(test.SelectPluginStateDataByPluginId(pluginId));

            test.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "PluginName",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.True(test.SelectExistsPlugin(pluginId));
            var actualState = test.SelectPluginStateDataByPluginId(pluginId);
            Assert.NotNull(actualState);
            Assert.Equal(pluginId, actualState.PluginId);
            Assert.Equal(PluginState.Enable, actualState.State);
            Assert.Equal("PluginName", actualState.PluginName);

            test.DeletePlugin(pluginId);
            Assert.False(test.SelectExistsPlugin(pluginId));
            Assert.Null(test.SelectPluginStateDataByPluginId(pluginId));
        }

        [Fact]
        public void Select_Update_LastUsePluginVersion_Test()
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var pluginId = PluginId.NewId();

            test.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "PluginName",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actualLastVersion1 = test.SelectLastUsePluginVersion(pluginId);
            Assert.NotNull(actualLastVersion1);
            Assert.Equal(new Version(0, 0, 0), actualLastVersion1);

            Assert.True(
                test.UpdatePluginRunningState(
                    pluginId,
                    new Version(1, 2, 3),
                    new Version(0, 0, 0),
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                )
            );

            Assert.False(
                test.UpdatePluginRunningState(
                    PluginId.Empty,
                    new Version(10, 20, 30),
                    new Version(0, 0, 0),
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                )
            );

            var actualLastVersion2 = test.SelectLastUsePluginVersion(pluginId);
            Assert.NotNull(actualLastVersion2);
            Assert.Equal(new Version(1, 2, 3), actualLastVersion2);
        }

        [Theory]
        [InlineData(true, PluginState.Enable, new[] { PluginState.Enable })]
        [InlineData(true, PluginState.Disable, new[] { PluginState.Disable })]
        [InlineData(true, PluginState.IllegalVersion, new[] { PluginState.IllegalVersion })]
        [InlineData(true, PluginState.Uninstall, new[] { PluginState.Uninstall })]
        [InlineData(true, PluginState.IllegalAssembly, new[] { PluginState.IllegalAssembly })]
        [InlineData(true, PluginState.Enable, new[] { PluginState.Enable, PluginState.Disable })]
        [InlineData(true, PluginState.Disable, new[] { PluginState.Enable, PluginState.Disable })]
        [InlineData(false, PluginState.Uninstall, new[] { PluginState.Enable, PluginState.Disable })]
        [InlineData(false, PluginState.Enable, new PluginState[0])]
        public void SelectExistsPluginByStateTest(bool expected, PluginState pluginState, PluginState[] pluginStateList)
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);

            foreach(var (state, index) in pluginStateList.Select((state, index) => (state, index))) {
                var pluginId = PluginId.NewId();
                test.InsertPluginStateData(
                    new PluginStateData() {
                        PluginId = pluginId,
                        PluginName = $"PluginName-{index}",
                        State = state,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            }

            Assert.Equal(expected, test.SelectExistsPluginByState(pluginState));
        }

        [Fact]
        public void SelectPluginStateDataTest()
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);

            var pluginIds = new[] {
                PluginId.NewId(),
                PluginId.NewId(),
                PluginId.NewId(),
            };

            var items = pluginIds.Select((a, i) => new PluginStateData() {
                PluginId = a,
                PluginName = i.ToString(CultureInfo.InvariantCulture),
                State = PluginState.Enable,
            }).ToArray();

            foreach(var item in items) {
                test.InsertPluginStateData(
                    item,
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            }

            var actual = test.SelectPluginStateData().ToArray();
            Assert.Equal(items.Length, actual.Length);
            foreach(var (item, actualItem) in items.Zip(actual)) {
                Assert.Equal(item.PluginId, actualItem.PluginId);
            }
        }

        [Fact]
        public void SelectAllLastUsedPluginsTest()
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);

            var pluginIds = new[] {
                PluginId.NewId(),
                PluginId.NewId(),
                PluginId.NewId(),
            };

            var items = pluginIds.Select((a, i) => new PluginStateData() {
                PluginId = a,
                PluginName = i.ToString(CultureInfo.InvariantCulture),
                State = PluginState.Enable,
            }).ToArray();

            foreach(var (item, index) in items.Select((item, index) => (item, index))) {
                test.InsertPluginStateData(
                    item,
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
                test.UpdatePluginRunningState(
                    item.PluginId,
                    new Version(index + 1, 0, 0),
                    new Version(0, 0, 0),
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            }

            var actual = test.SelectAllLastUsedPlugins().ToArray();
            Assert.Equal(items.Length, actual.Length);
            foreach(var (item, actualItem, index) in items.Zip(actual).Select((a,i) => (item:a.First, actualItem:a.Second, index: i))) {
                Assert.Equal(item.PluginId, actualItem.PluginId);
                Assert.Equal(item.PluginName, (index).ToString(CultureInfo.InvariantCulture));
                Assert.Equal(actualItem.Version, new Version(index + 1, 0, 0));
            }
        }

        [Fact]
        public void UpdatePluginStateDataTest()
        {
            var test = Test.BuildDao<PluginsEntityDao>(AccessorKind.Main);
            var pluginId = PluginId.NewId();

            Assert.False(
                test.UpdatePluginStateData(
                    new PluginStateData() {
                        PluginId = pluginId,
                        PluginName = "Test",
                        State = PluginState.Enable,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                )
            );

            test.InsertPluginStateData(
                new PluginStateData() {
                    PluginId = pluginId,
                    PluginName = "Test",
                    State = PluginState.Enable,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.True(
                test.UpdatePluginStateData(
                    new PluginStateData() {
                        PluginId = pluginId,
                        PluginName = "Test2",
                        State = PluginState.Disable,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                )
            );

            var actual = test.SelectPluginStateDataByPluginId( pluginId );
            Assert.NotNull( actual );
            Assert.Equal(pluginId, actual.PluginId);
            Assert.Equal(PluginState.Disable, actual.State);
            Assert.Equal("Test2", actual.PluginName);
        }

        #endregion
    }
}
