using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppUpdateSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppUpdateSettingEntityDao>(AccessorKind.Main);

            var expected = new SettingAppUpdateSettingData() {
                UpdateKind = UpdateKind.Notify,
            };

            test.UpdateSettingUpdateSetting(expected, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectSettingUpdateSetting();

            Assert.Equal(expected.UpdateKind, actual.UpdateKind);
        }

        [Fact]
        public void UpdateReleaseVersionTest()
        {
            var test = Test.BuildDao<AppUpdateSettingEntityDao>(AccessorKind.Main);

            test.UpdateReleaseVersion(UpdateKind.None, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectSettingUpdateSetting();

            Assert.Equal(UpdateKind.None, actual.UpdateKind);
        }

        #endregion
    }
}
