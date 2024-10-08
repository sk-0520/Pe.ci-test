using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Domain
{
    public class KeyGestureGuideDomainDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectKeyMappings_Empty_Test()
        {
            var test = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var actual = test.SelectKeyMappings(KeyActionKind.Command, "command");
            Assert.Empty(actual.Items);
        }

        [Fact]
        public void SelectKeyMappings_Single_Test()
        {
            var testDomain = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var testActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testMappings = Test.BuildDao<KeyMappingsEntityDao>(AccessorKind.Main);

            var keyActionId = KeyActionId.NewId();

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId,
                    KeyActionKind = KeyActionKind.Command,
                    KeyActionContent = "command"
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.A
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.B
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectKeyMappings(KeyActionKind.Command, "command");
            Assert.Single(actual.Items);
            var actualItem = actual.Items[0];

            Assert.Equal(2, actualItem.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.A, actualItem.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.B, actualItem.Mappings[1].Key);
        }

        [Fact]
        public void SelectKeyMappings_Multi_Test()
        {
            var testDomain = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var testActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testMappings = Test.BuildDao<KeyMappingsEntityDao>(AccessorKind.Main);

            var keyActionId1 = KeyActionId.NewId();
            var keyActionId2 = KeyActionId.NewId();

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId1,
                    KeyActionKind = KeyActionKind.Command,
                    KeyActionContent = "command"
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId1,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.A
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId1,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.B
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId2,
                    KeyActionKind = KeyActionKind.Command,
                    KeyActionContent = "command"
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId2,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.Y
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId2,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.Z
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testDomain.SelectKeyMappings(KeyActionKind.Command, "command");
            Assert.Equal(2, actual1.Items.Count);
            var actualItem1 = actual1.Items[0];
            Assert.Equal(2, actualItem1.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.A, actualItem1.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.B, actualItem1.Mappings[1].Key);

            testActions.UpdateUsageCountIncrement(keyActionId2, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual2 = testDomain.SelectKeyMappings(KeyActionKind.Command, "command");
            Assert.Equal(2, actual2.Items.Count);
            var actualItem2 = actual2.Items[0];
            Assert.Equal(2, actualItem2.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.Y, actualItem2.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.Z, actualItem2.Mappings[1].Key);
        }

        [Fact]
        public void SelectLauncherKeyMappings_Empty_Test()
        {
            var test = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var actual = test.SelectLauncherKeyMappings(LauncherItemId.NewId());
            Assert.Empty(actual.Items);
        }

        [Fact]
        public void SelectLauncherKeyMappings_Single_Test()
        {
            var testDomain = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testMappings = Test.BuildDao<KeyMappingsEntityDao>(AccessorKind.Main);
            var testOptions = Test.BuildDao<KeyOptionsEntityDao>(AccessorKind.Main);

            var launcherItemId = LauncherItemId.NewId();

            testItems.InsertLauncherItem(
                new LauncherItemData() {
                    LauncherItemId = launcherItemId,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var keyActionId = KeyActionId.NewId();

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId,
                    KeyActionKind = KeyActionKind.LauncherItem,
                    KeyActionContent = KeyActionContentLauncherItem.Execute.ToString()
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.A
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.B
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testOptions.InsertOption(
                keyActionId,
                "LauncherItemId",
                launcherItemId.ToString(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectLauncherKeyMappings(launcherItemId);
            Assert.Single(actual.Items);
            var actualItem = actual.Items[0];

            Assert.Equal(2, actualItem.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.A, actualItem.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.B, actualItem.Mappings[1].Key);
        }

        [Fact]
        public void SelectLauncherKeyMappings_Multi_Test()
        {
            var testDomain = Test.BuildDao<KeyGestureGuideDomainDao>(AccessorKind.Main);
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testActions = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var testMappings = Test.BuildDao<KeyMappingsEntityDao>(AccessorKind.Main);
            var testOptions = Test.BuildDao<KeyOptionsEntityDao>(AccessorKind.Main);

            var launcherItemId = LauncherItemId.NewId();

            testItems.InsertLauncherItem(
                new LauncherItemData() {
                    LauncherItemId = launcherItemId,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var keyActionId1 = KeyActionId.NewId();
            var keyActionId2 = KeyActionId.NewId();

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId1,
                    KeyActionKind = KeyActionKind.LauncherItem,
                    KeyActionContent = KeyActionContentLauncherItem.Execute.ToString()
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId1,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.A
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId1,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.B
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testOptions.InsertOption(
                keyActionId1,
                "LauncherItemId",
                launcherItemId.ToString(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testActions.InsertKeyAction(
                new KeyActionData() {
                    KeyActionId = keyActionId2,
                    KeyActionKind = KeyActionKind.LauncherItem,
                    KeyActionContent = KeyActionContentLauncherItem.Execute.ToString()
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testMappings.InsertMapping(
                keyActionId2,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.Y
                },
                1,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testMappings.InsertMapping(
                keyActionId2,
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.Z
                },
                2,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testOptions.InsertOption(
                keyActionId2,
                "LauncherItemId",
                launcherItemId.ToString(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testDomain.SelectLauncherKeyMappings(launcherItemId);
            Assert.Equal(2, actual1.Items.Count);
            var actualItem = actual1.Items[0];

            Assert.Equal(2, actualItem.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.A, actualItem.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.B, actualItem.Mappings[1].Key);

            testActions.UpdateUsageCountIncrement(keyActionId2, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual2 = testDomain.SelectLauncherKeyMappings(launcherItemId);
            Assert.Equal(2, actual2.Items.Count);
            var actualItem2 = actual2.Items[0];
            Assert.Equal(2, actualItem2.Mappings.Count);
            Assert.Equal(System.Windows.Input.Key.Y, actualItem2.Mappings[0].Key);
            Assert.Equal(System.Windows.Input.Key.Z, actualItem2.Mappings[1].Key);
        }

        #endregion
    }
}
