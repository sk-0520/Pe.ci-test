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
    public class LauncherSeparatorsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testSeparators = Test.BuildDao<LauncherSeparatorsEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.Separator,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testSeparators.InsertSeparator(
                id,
                new LauncherSeparatorData() {
                    Kind = LauncherSeparatorKind.Line,
                    Width = 123,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testSeparators.SelectSeparator(id);
            Assert.Equal(LauncherSeparatorKind.Line, actual1.Kind);
            Assert.Equal(123, actual1.Width);

            testSeparators.UpdateSeparator(
                id,
                new LauncherSeparatorData() {
                    Kind = LauncherSeparatorKind.Space,
                    Width = 234,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual2 = testSeparators.SelectSeparator(id);
            Assert.Equal(LauncherSeparatorKind.Space, actual2.Kind);
            Assert.Equal(234, actual2.Width);

            Assert.True(testSeparators.DeleteSeparatorByLauncherItemId(id));
            Assert.False(testSeparators.DeleteSeparatorByLauncherItemId(id));
        }

        #endregion
    }
}
