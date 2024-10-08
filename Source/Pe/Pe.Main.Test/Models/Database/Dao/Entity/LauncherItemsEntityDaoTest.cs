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
    public class LauncherItemsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var test = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);

            var data1 = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "NAME",
                Kind = LauncherItemKind.File,
                Icon = new IconData() {
                    Index = 1,
                    Path = "PATH"
                },
                IsEnabledCommandLauncher = true,
                Comment = "COMMENT",
            };

            Assert.False(test.SelectExistsLauncherItem(data1.LauncherItemId));
            test.InsertLauncherItem(data1, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectExistsLauncherItem(data1.LauncherItemId));

            var actual1 = test.SelectLauncherItem(data1.LauncherItemId);
            Assert.Equal(data1.LauncherItemId, actual1.LauncherItemId);
            Assert.Equal(data1.Name, actual1.Name);
            Assert.Equal(data1.Kind, actual1.Kind);
            Assert.Equal(data1.Icon.Index, actual1.Icon.Index);
            Assert.Equal(data1.Icon.Path, actual1.Icon.Path);
            Assert.Equal(data1.IsEnabledCommandLauncher, actual1.IsEnabledCommandLauncher);
            Assert.Equal(data1.Comment, actual1.Comment);

            var data2 = new LauncherItemData() {
                LauncherItemId = data1.LauncherItemId,
                Name = "NAME2",
                Kind = LauncherItemKind.File,
                Icon = new IconData() {
                    Index = 1,
                    Path = "PATH2"
                },
                IsEnabledCommandLauncher = true,
                Comment = "COMMENT2",
            };
            test.UpdateCustomizeLauncherItem(data2, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = test.SelectLauncherItem(data2.LauncherItemId);
            Assert.Equal(data2.LauncherItemId, actual2.LauncherItemId);
            Assert.Equal(data2.Name, actual2.Name);
            Assert.Equal(data2.Kind, actual2.Kind);
            Assert.Equal(data2.Icon.Index, actual2.Icon.Index);
            Assert.Equal(data2.Icon.Path, actual2.Icon.Path);
            Assert.Equal(data2.IsEnabledCommandLauncher, actual2.IsEnabledCommandLauncher);
            Assert.Equal(data2.Comment, actual2.Comment);

            test.DeleteLauncherItem(data1.LauncherItemId);
            Assert.False(test.SelectExistsLauncherItem(data1.LauncherItemId));
        }

        [Fact]
        public void Select_Items_Test()
        {
            var test = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);

            var items = new[] {
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "File",
                    Kind = LauncherItemKind.File,
                    Icon = new IconData(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "StoreApp",
                    Kind = LauncherItemKind.StoreApp,
                    Icon = new IconData(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "Addon",
                    Kind = LauncherItemKind.Addon,
                    Icon = new IconData(),
                },
                new LauncherItemData() {
                    LauncherItemId = LauncherItemId.NewId(),
                    Name = "Separator",
                    Kind = LauncherItemKind.Separator,
                    Icon = new IconData(),
                },
            };
            foreach(var item in items) {
                test.InsertLauncherItem(item, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }

            var actualAll = new HashSet<LauncherItemId>(test.SelectAllLauncherItemIds());
            Assert.Contains(items.Single(a => a.Name == "File").LauncherItemId, actualAll);
            Assert.Contains(items.Single(a => a.Name == "StoreApp").LauncherItemId, actualAll);
            Assert.Contains(items.Single(a => a.Name == "Addon").LauncherItemId, actualAll);
            Assert.Contains(items.Single(a => a.Name == "Separator").LauncherItemId, actualAll);

            var actualApp = test.SelectApplicationLauncherItems();
            Assert.Equal(actualApp.Single(a => a.Name == "File").LauncherItemId, items.Single(a => a.Name == "File").LauncherItemId);
            Assert.Equal(actualApp.Single(a => a.Name == "StoreApp").LauncherItemId, items.Single(a => a.Name == "StoreApp").LauncherItemId);
            Assert.Equal(actualApp.Single(a => a.Name == "Addon").LauncherItemId, items.Single(a => a.Name == "Addon").LauncherItemId);
            Assert.Equal(actualApp.Single(a => a.Name == "Separator").LauncherItemId, items.Single(a => a.Name == "Separator").LauncherItemId);
        }

        [Fact]
        public void UpdateExecuteCountIncrementTest()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<LauncherItemsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            var data = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "File",
                Kind = LauncherItemKind.File,
                Icon = new IconData(),
            };
            test.InsertLauncherItem(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var sql = "select LauncherItems.ExecuteCount from LauncherItems where LauncherItems.LauncherItemId = @LauncherItemId";
            var param = new { LauncherItemId = data.LauncherItemId };
            Assert.Equal(0, mainDatabaseAccessor.GetScalar<long>(sql, param));

            test.UpdateExecuteCountIncrement(data.LauncherItemId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(1, mainDatabaseAccessor.GetScalar<long>(sql, param));

            test.UpdateExecuteCountIncrement(data.LauncherItemId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(2, mainDatabaseAccessor.GetScalar<long>(sql, param));
        }

        #endregion
    }
}
