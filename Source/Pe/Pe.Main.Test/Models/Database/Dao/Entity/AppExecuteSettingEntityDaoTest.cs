using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppExecuteSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectSettingExecuteSettingTest()
        {
            var test = Test.BuildMainDao<AppExecuteSettingEntityDao>();
            var actual = test.SelectSettingExecuteSetting();
            Assert.False(actual.IsEnabledTelemetry);
            Assert.Empty(actual.UserId);
        }

        #endregion
    }
}
