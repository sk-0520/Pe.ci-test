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
    public class LauncherRedoSuccessExitCodesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testRedoCodes = Test.BuildDao<LauncherRedoSuccessExitCodesEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.File,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Empty(testRedoCodes.SelectRedoSuccessExitCodes(id));

            testRedoCodes.InsertSuccessExitCodes(
                id,
                [10, 20, 30],
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Equal([10, 20, 30], testRedoCodes.SelectRedoSuccessExitCodes(id));

            Assert.Equal(3, testRedoCodes.DeleteSuccessExitCodes(id));
            Assert.Equal(0, testRedoCodes.DeleteSuccessExitCodes(id));
            Assert.Empty(testRedoCodes.SelectRedoSuccessExitCodes(id));
        }

        #endregion
    }
}
