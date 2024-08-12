using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherItemIconStatusEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var test = Test.BuildDao<LauncherItemIconStatusEntityDao>(AccessorKind.Large);
            var id = LauncherItemId.NewId();

            test.InsertLastUpdatedIconTimestamp(id, new IconScale(IconBox.Small, new System.Windows.Point(1, 1)), DateTime.UtcNow, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertLastUpdatedIconTimestamp(id, new IconScale(IconBox.Normal, new System.Windows.Point(1, 1)), DateTime.UtcNow, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Equal(2, test.SelectLauncherItemIconAllSizeStatus(id).Count());

            Assert.True(test.SelectExistsLauncherItemIconState(id, new IconScale(IconBox.Small, new System.Windows.Point(1, 1))));
            Assert.True(test.SelectExistsLauncherItemIconState(id, new IconScale(IconBox.Normal, new System.Windows.Point(1, 1))));
            Assert.False(test.SelectExistsLauncherItemIconState(id, new IconScale(IconBox.Normal, new System.Windows.Point(2, 1))));
            Assert.False(test.SelectExistsLauncherItemIconState(id, new IconScale(IconBox.Big, new System.Windows.Point(1, 1))));

            var actual1 = test.SelectLauncherItemIconSingleSizeStatus(id, new IconScale(IconBox.Small, new System.Windows.Point(1, 1)));
            Assert.NotNull(actual1);
            Assert.Equal(new IconScale(IconBox.Small, new System.Windows.Point(1, 1)), actual1.IconScale);

            var actual2 = test.SelectLauncherItemIconSingleSizeStatus(id, new IconScale(IconBox.Normal, new System.Windows.Point(1, 1)));
            Assert.NotNull(actual2);
            Assert.Equal(new IconScale(IconBox.Normal, new System.Windows.Point(1, 1)), actual2.IconScale);

            var actual3 = test.SelectLauncherItemIconSingleSizeStatus(id, new IconScale(IconBox.Normal, new System.Windows.Point(2, 1)));
            Assert.Null(actual3);

            var timestamp = DateTime.UtcNow.AddDays(1);
            test.UpdateLastUpdatedIconTimestamp(id, new IconScale(IconBox.Small, new System.Windows.Point(1, 1)), timestamp, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual4 = test.SelectLauncherItemIconSingleSizeStatus(id, new IconScale(IconBox.Small, new System.Windows.Point(1, 1)));
            Assert.NotNull(actual4);
            Assert.Equal(new IconScale(IconBox.Small, new System.Windows.Point(1, 1)), actual4.IconScale);
            Assert.Equal(timestamp, actual4.LastUpdatedTimestamp);

            Assert.Equal(2, test.DeleteAllSizeLauncherItemIconState(id));
            Assert.Empty(test.SelectLauncherItemIconAllSizeStatus(id));
        }

        #endregion
    }
}
