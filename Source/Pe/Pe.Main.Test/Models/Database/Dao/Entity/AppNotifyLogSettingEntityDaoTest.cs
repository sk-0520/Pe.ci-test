using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppNotifyLogSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppNotifyLogSettingEntityDao>(AccessorKind.Main);

            var expected = new SettingAppNotifyLogSettingData() {
                IsVisible = true,
                Position = NotifyLogPosition.RightBottom,
            };

            test.UpdateSettingNotifyLogSetting(expected, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = test.SelectSettingNotifyLogSetting();

            Assert.Equal(expected.IsVisible, actual.IsVisible);
            Assert.Equal(expected.Position, actual.Position);
        }


        #endregion
    }
}
