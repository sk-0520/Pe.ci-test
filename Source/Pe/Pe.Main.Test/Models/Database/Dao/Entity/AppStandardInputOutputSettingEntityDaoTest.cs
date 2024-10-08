using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppStandardInputOutputSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectStandardInputOutputSettingFontIdTest()
        {
            var test = Test.BuildDao<AppStandardInputOutputSettingEntityDao>(AccessorKind.Main);

            var actual = test.SelectStandardInputOutputSettingFontId();
            Assert.NotEqual(FontId.Empty, actual);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppStandardInputOutputSettingEntityDao>(AccessorKind.Main);

            var expected = new SettingAppStandardInputOutputSettingData() {
                ErrorBackgroundColor = Colors.Red,
                ErrorForegroundColor = Colors.Yellow,
                FontId = test.SelectStandardInputOutputSettingFontId(),
                IsTopmost = true,
                OutputBackgroundColor = Colors.Lime,
                OutputForegroundColor = Colors.Blue,
            };

            test.UpdateSettingStandardInputOutputSetting(expected, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectSettingStandardInputOutputSetting();

            Assert.Equal(expected.ErrorBackgroundColor, actual.ErrorBackgroundColor);
            Assert.Equal(expected.ErrorForegroundColor, actual.ErrorForegroundColor);
            Assert.Equal(expected.FontId, actual.FontId);
            Assert.Equal(expected.IsTopmost, actual.IsTopmost);
            Assert.Equal(expected.OutputBackgroundColor, actual.OutputBackgroundColor);
            Assert.Equal(expected.OutputForegroundColor, actual.OutputForegroundColor);
        }

        #endregion
    }
}
