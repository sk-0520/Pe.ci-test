using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Library.Database;
using Xunit;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherGroupItemsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectMaxSequence_Empty_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<LauncherGroupItemsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            Assert.Equal(0, test.SelectMaxSequence(LauncherGroupId.Empty));
        }

        [Fact]
        public void SelectMaxSequence_Init_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var testGroup = Test.DiContainer.Build<LauncherGroupsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var testGroupItem = Test.DiContainer.Build<LauncherGroupItemsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            var launcherGroupId = mainDatabaseAccessor.GetScalar<LauncherGroupId>("select LauncherGroups.LauncherGroupId from LauncherGroups");

            Assert.Equal(0, testGroupItem.SelectMaxSequence(launcherGroupId));
        }

        [Fact]
        public void SelectLauncherItemIds_Empty_Test()
        {
            var test = Test.BuildDao<LauncherGroupItemsEntityDao>(AccessorKind.Main);

            Assert.Empty(test.SelectLauncherItemIds(LauncherGroupId.Empty));
        }

        [Fact]
        public void Insert_Select_Test()
        {
            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testGroup = Test.BuildDao<LauncherGroupsEntityDao>(AccessorKind.Main);
            var testGroupItem = Test.BuildDao<LauncherGroupItemsEntityDao>(AccessorKind.Main);

            var items = new[] {
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                },
            };
            foreach(var item in items) {
                testItem.InsertLauncherItem(item, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }

            var launcherGroupId = LauncherGroupId.NewId();
            testGroup.InsertNewGroup(new Main.Models.Data.LauncherGroupData() {
                LauncherGroupId = launcherGroupId
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testGroupItem.InsertNewItems(
                launcherGroupId,
                items.Select(a => a.LauncherItemId),
                0,
                10,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Equal(items.Select(a => a.LauncherItemId), testGroupItem.SelectLauncherItemIds(launcherGroupId));

            var deleteItem1 = items[1];
            Assert.Equal(1, testGroupItem.DeleteGroupItemsByLauncherItemId(deleteItem1.LauncherItemId));
            var actual1 = testGroupItem.SelectLauncherItemIds(launcherGroupId).ToArray();
            Assert.Contains(items[0].LauncherItemId, actual1);
            Assert.DoesNotContain(items[1].LauncherItemId, actual1);
            Assert.Contains(items[2].LauncherItemId, actual1);
            Assert.Equal(0, testGroupItem.DeleteGroupItemsByLauncherItemId(deleteItem1.LauncherItemId));

            var deleteItem2 = items[2];
            testGroupItem.DeleteGroupItemsLauncherItem(launcherGroupId, deleteItem2.LauncherItemId, 0);
            var actual2 = testGroupItem.SelectLauncherItemIds(launcherGroupId).ToArray();
            Assert.Equal(2, actual2.Length);

            Assert.Equal(2, testGroupItem.DeleteGroupItemsByLauncherGroupId(launcherGroupId));
        }

        #endregion
    }
}
