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
    public class LauncherStoreAppsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testStoreApps = Test.BuildDao<LauncherStoreAppsEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.StoreApp,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testStoreApps.InsertStoreApp(
                id,
                new LauncherStoreAppData() {
                    Option = "Option",
                    ProtocolAlias = "ProtocolAlias",
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testStoreApps.SelectStoreApp(id);
            Assert.Equal("Option", actual1.Option);
            Assert.Equal("ProtocolAlias", actual1.ProtocolAlias);

            testStoreApps.UpdateStoreApp(
                id,
                new LauncherStoreAppData() {
                    Option = "Option2",
                    ProtocolAlias = "ProtocolAlias2",
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual2 = testStoreApps.SelectStoreApp(id);
            Assert.Equal("Option2", actual2.Option);
            Assert.Equal("ProtocolAlias2", actual2.ProtocolAlias);

            Assert.True(testStoreApps.DeleteStoreApp(id));
            Assert.False(testStoreApps.DeleteStoreApp(id));
        }

        #endregion
    }
}
