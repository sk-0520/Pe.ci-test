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
    public class LauncherRedoItemsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testRedoItems = Test.BuildDao<LauncherRedoItemsEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.File,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.False(testRedoItems.SelectExistsLauncherRedoItem(id));

            testRedoItems.InsertRedoItem(id, new LauncherRedoData() {
                RedoMode = RedoMode.Count,
                RetryCount = 10,
                WaitTime = TimeSpan.FromSeconds(1),
                SuccessExitCodes = [0, 10, 100],
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(testRedoItems.SelectExistsLauncherRedoItem(id));
            var actual1 = testRedoItems.SelectLauncherRedoItem(id);
            Assert.Equal(RedoMode.Count, actual1.RedoMode);
            Assert.Equal(10, actual1.RetryCount);
            Assert.Equal(TimeSpan.FromSeconds(1), actual1.WaitTime);
            Assert.Empty(actual1.SuccessExitCodes);

            testRedoItems.UpdateRedoItem(id, new LauncherRedoData() {
                RedoMode = RedoMode.Timeout,
                RetryCount = 20,
                WaitTime = TimeSpan.FromSeconds(2),
                SuccessExitCodes = [0, 10, 100],
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testRedoItems.SelectLauncherRedoItem(id);
            Assert.Equal(RedoMode.Timeout, actual2.RedoMode);
            Assert.Equal(20, actual2.RetryCount);
            Assert.Equal(TimeSpan.FromSeconds(2), actual2.WaitTime);
            Assert.Empty(actual1.SuccessExitCodes);

            Assert.True(testRedoItems.DeleteRedoItemByLauncherItemId(id));
            Assert.False(testRedoItems.DeleteRedoItemByLauncherItemId(id));
            Assert.False(testRedoItems.SelectExistsLauncherRedoItem(id));
        }

        #endregion
    }
}
