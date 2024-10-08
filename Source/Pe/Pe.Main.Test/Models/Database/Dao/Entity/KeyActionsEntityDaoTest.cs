using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class KeyActionsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Test()
        {
            static void AssertEquals(KeyActionData expected, KeyActionData actual)
            {
                Assert.Equal(expected.KeyActionId, actual.KeyActionId);
                Assert.Equal(expected.KeyActionKind, actual.KeyActionKind);
                Assert.Equal(expected.KeyActionContent, actual.KeyActionContent);
                Assert.Equal(expected.Comment, actual.Comment);
            }

            var test = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);

            var data = new {
                Replace = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.Replace,
                    KeyActionContent = "Replace",
                    Comment = "Comment:Replace",
                },
                Disable = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.Disable,
                    KeyActionContent = "Disable",
                    Comment = "Comment:Disable",
                },
                Command = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.Command,
                    KeyActionContent = "Command",
                    Comment = "Comment:Command",
                },
                LauncherItem = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.LauncherItem,
                    KeyActionContent = "LauncherItem",
                    Comment = "Comment:LauncherItem",
                },
                LauncherToolbar = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.LauncherToolbar,
                    KeyActionContent = "LauncherToolbar",
                    Comment = "Comment:LauncherToolbar",
                },
                Note = new KeyActionData() {
                    KeyActionId = KeyActionId.NewId(),
                    KeyActionKind = KeyActionKind.Note,
                    KeyActionContent = "Note",
                    Comment = "Comment:Note",
                },
            };
            test.InsertKeyAction(data.Replace, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertKeyAction(data.Disable, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertKeyAction(data.Command, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertKeyAction(data.LauncherItem, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertKeyAction(data.LauncherToolbar, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertKeyAction(data.Note, Test.DiContainer.Build<IDatabaseCommonStatus>());

            AssertEquals(data.Replace, test.SelectAllKeyActionsFromKind(KeyActionKind.Replace).Single());
            AssertEquals(data.Disable, test.SelectAllKeyActionsFromKind(KeyActionKind.Disable).Single());
            AssertEquals(data.Command, test.SelectAllKeyActionsFromKind(KeyActionKind.Command).Single());
            AssertEquals(data.LauncherItem, test.SelectAllKeyActionsFromKind(KeyActionKind.LauncherItem).Single());
            AssertEquals(data.LauncherToolbar, test.SelectAllKeyActionsFromKind(KeyActionKind.LauncherToolbar).Single());
            AssertEquals(data.Note, test.SelectAllKeyActionsFromKind(KeyActionKind.Note).Single());

            var actual = test.SelectAllKeyActionsIgnoreKinds([KeyActionKind.Replace, KeyActionKind.Command]).GroupBy(k => k.KeyActionKind, v => v);
            AssertEquals(data.Disable, actual.Single(a => a.Key == KeyActionKind.Disable).Single());
            AssertEquals(data.LauncherItem, actual.Single(a => a.Key == KeyActionKind.LauncherItem).Single());
            AssertEquals(data.LauncherToolbar, actual.Single(a => a.Key == KeyActionKind.LauncherToolbar).Single());
            AssertEquals(data.Note, actual.Single(a => a.Key == KeyActionKind.Note).Single());
            Assert.DoesNotContain(actual, a => a.Key == KeyActionKind.Replace);
            Assert.DoesNotContain(actual, a => a.Key == KeyActionKind.Command);
        }

        [Fact]
        public void Insert_Update_Test()
        {
            var test = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var keyActionId = KeyActionId.NewId();

            var src = new KeyActionData() {
                KeyActionId = keyActionId,
                KeyActionKind = KeyActionKind.Replace,
                KeyActionContent = "Replace",
                Comment = "Comment:Replace",
            };
            var expected = new KeyActionData() {
                KeyActionId = keyActionId,
                KeyActionKind = KeyActionKind.Replace,
                KeyActionContent = "Update",
                Comment = "Comment:Update",
            };

            test.InsertKeyAction(src, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateKeyAction(expected, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectAllKeyActionsFromKind(KeyActionKind.Replace).Single();

            Assert.Equal(expected.KeyActionId, actual.KeyActionId);
            Assert.Equal(expected.KeyActionKind, actual.KeyActionKind);
            Assert.Equal(expected.KeyActionContent, actual.KeyActionContent);
            Assert.Equal(expected.Comment, actual.Comment);
        }

        [Fact]
        public void UpdateUsageCountIncrementTest()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<KeyActionsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var keyActionId = KeyActionId.NewId();

            var data = new KeyActionData() {
                KeyActionId = keyActionId,
                KeyActionKind = KeyActionKind.Note,
                KeyActionContent = "Note",
                Comment = "Comment:Note",
            };

            test.InsertKeyAction(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(0, mainDatabaseAccessor.GetScalar<long>("select KeyActions.UsageCount from KeyActions where KeyActions.KeyActionId = @KeyActionId", new { KeyActionId = keyActionId }));
            test.UpdateUsageCountIncrement(keyActionId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(1, mainDatabaseAccessor.GetScalar<long>("select KeyActions.UsageCount from KeyActions where KeyActions.KeyActionId = @KeyActionId", new { KeyActionId = keyActionId }));
            test.UpdateUsageCountIncrement(keyActionId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(2, mainDatabaseAccessor.GetScalar<long>("select KeyActions.UsageCount from KeyActions where KeyActions.KeyActionId = @KeyActionId", new { KeyActionId = keyActionId }));
        }

        [Fact]
        public void DeleteKeyActionTest()
        {
            var test = Test.BuildDao<KeyActionsEntityDao>(AccessorKind.Main);
            var keyActionId = KeyActionId.NewId();

            var data = new KeyActionData() {
                KeyActionId = keyActionId,
                KeyActionKind = KeyActionKind.LauncherToolbar,
                KeyActionContent = "LauncherToolbar",
                Comment = "Comment:LauncherToolbar",
            };

            test.InsertKeyAction(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Single(test.SelectAllKeyActionsFromKind(KeyActionKind.LauncherToolbar));

            test.DeleteKeyAction(keyActionId);
            Assert.Empty(test.SelectAllKeyActionsFromKind(KeyActionKind.LauncherToolbar));
        }

        #endregion
    }
}
