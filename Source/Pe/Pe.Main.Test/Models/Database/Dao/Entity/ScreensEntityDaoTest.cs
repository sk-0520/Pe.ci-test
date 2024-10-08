using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class ScreensEntityDaoTest
    {
        #region define

        private sealed class ScreenData: IScreen
        {
            public required int BitsPerPixel { get; init; }

            public required Rect DeviceBounds { get; init; }

            public required  string DeviceName { get;  init; }

            public required bool Primary { get; init; }

            public required Rect DeviceWorkingArea { get; init; }
        }

        #endregion

        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Test()
        {
            var test = Test.BuildDao<ScreensEntityDao>(AccessorKind.Main);

            Assert.False(test.SelectExistsScreen(string.Empty));
            Assert.False(test.SelectExistsScreen("screen"));

            test.InsertScreen(
                new ScreenData() {
                    Primary = true,
                    BitsPerPixel = 24,
                    DeviceName = string.Empty,
                    DeviceWorkingArea = new System.Windows.Rect(0, 0, 90, 190),
                    DeviceBounds = new System.Windows.Rect(0, 0, 100, 200),
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            test.InsertScreen(
                new ScreenData() {
                    Primary = true,
                    BitsPerPixel = 24,
                    DeviceName = "screen",
                    DeviceWorkingArea = new System.Windows.Rect(0, 0, 900, 1900),
                    DeviceBounds = new System.Windows.Rect(0, 0, 1000, 2000),
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.True(test.SelectExistsScreen(string.Empty));
            Assert.True(test.SelectExistsScreen("screen"));
        }


        #endregion
    }
}
