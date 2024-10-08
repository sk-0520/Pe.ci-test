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
    public class LauncherItemHistoriesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectHistories_Empty_Test()
        {
            var test = Test.BuildDao<LauncherItemHistoriesEntityDao>(AccessorKind.Main);
            var actual = test.SelectHistories(LauncherItemId.Empty);
            Assert.Empty(actual);
        }

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testHistories = Test.BuildDao<LauncherItemHistoriesEntityDao>(AccessorKind.Main);

            var item = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
            };
            testItems.InsertLauncherItem(item, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testHistories.InsertHistory(item.LauncherItemId, LauncherHistoryKind.Option, "Option1", DateTime.UtcNow.AddMicroseconds(1), Test.DiContainer.Build<IDatabaseCommonStatus>());
            testHistories.InsertHistory(item.LauncherItemId, LauncherHistoryKind.WorkDirectory, "WorkDirectory1", DateTime.UtcNow.AddMicroseconds(2), Test.DiContainer.Build<IDatabaseCommonStatus>());
            testHistories.InsertHistory(item.LauncherItemId, LauncherHistoryKind.Option, "Option2", DateTime.UtcNow.AddMicroseconds(3), Test.DiContainer.Build<IDatabaseCommonStatus>());
            var workDirectory2Timestamp = DateTime.UtcNow.AddMicroseconds(4);
            testHistories.InsertHistory(item.LauncherItemId, LauncherHistoryKind.WorkDirectory, "WorkDirectory2", workDirectory2Timestamp, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = testHistories.SelectHistories(item.LauncherItemId).ToArray();
            var actualOptions = actual.Where(a => a.Kind == LauncherHistoryKind.Option).ToArray();
            Assert.Equal("Option2", actualOptions[0].Value);
            Assert.Equal("Option1", actualOptions[1].Value);
            var actualWorkDirectories = actual.Where(a => a.Kind == LauncherHistoryKind.WorkDirectory).ToArray();
            Assert.Equal("WorkDirectory2", actualWorkDirectories[0].Value);
            Assert.Equal("WorkDirectory1", actualWorkDirectories[1].Value);

            Assert.Equal(0, testHistories.DeleteHistory(item.LauncherItemId, LauncherHistoryKind.Option, "Option3"));
            Assert.Equal(1, testHistories.DeleteHistory(item.LauncherItemId, LauncherHistoryKind.Option, "Option2"));
            Assert.Equal("Option1", testHistories.SelectHistories(item.LauncherItemId).Single(a => a.Kind == LauncherHistoryKind.Option).Value);

            Assert.Equal(1, testHistories.DeleteHistoryByLauncherItemId(item.LauncherItemId, LauncherHistoryKind.WorkDirectory, workDirectory2Timestamp));

            Assert.Equal(2, testHistories.DeleteHistoriesByLauncherItemId(item.LauncherItemId));
            Assert.Empty(testHistories.SelectHistories(item.LauncherItemId));
        }


        #endregion
    }
}
