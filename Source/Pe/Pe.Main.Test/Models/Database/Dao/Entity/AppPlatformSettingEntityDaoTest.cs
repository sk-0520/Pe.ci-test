using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppPlatformSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void UpdateSupportExplorerTest()
        {
            var test = Test.BuildDao<AppPlatformSettingEntityDao>(AccessorKind.Main);

            Assert.False(test.SelectSettingPlatformSetting().SupportExplorer);
            test.UpdateSupportExplorer(true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectSettingPlatformSetting().SupportExplorer);
        }

        [Fact]
        public void UpdateSuppressSystemIdleTest()
        {
            var test = Test.BuildDao<AppPlatformSettingEntityDao>(AccessorKind.Main);

            Assert.False(test.SelectSettingPlatformSetting().SuppressSystemIdle);
            test.UpdateSuppressSystemIdle(true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectSettingPlatformSetting().SuppressSystemIdle);
        }

        #endregion
    }
}
