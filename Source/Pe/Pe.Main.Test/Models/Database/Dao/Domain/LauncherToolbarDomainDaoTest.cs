using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Domain
{
    public class LauncherToolbarDomainDaoTest
    {
        #region define

        private sealed class ScreenData: IScreen
        {
            public required int BitsPerPixel { get; init; }

            public required Rect DeviceBounds { get; init; }

            public required string DeviceName { get; init; }

            public required bool Primary { get; init; }

            public required Rect DeviceWorkingArea { get; init; }
        }

        #endregion

        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectScreenToolbar_NoScreen_Test()
        {
            var testDomain = Test.BuildDao<LauncherToolbarDomainDao>(AccessorKind.Main);
            var testToolbars = Test.BuildDao<LauncherToolbarsEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);

            var toolbarId = LauncherToolbarId.NewId();
            var fontId = FontId.NewId();

            testFonts.InsertFont(
                fontId,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testToolbars.InsertNewToolbar(
                toolbarId,
                fontId,
                "screen",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectScreenToolbar(toolbarId);
            Assert.Equal("screen", actual.ScreenName);
            Assert.Equal(-1, actual.X);
            Assert.Equal(-1, actual.Y);
            Assert.Equal(0, actual.Width);
            Assert.Equal(0, actual.Height);

            Assert.Empty(testDomain.SelectAllScreenToolbars());
        }

        [Fact]
        public void SelectScreenToolbar_Screen_Test()
        {
            var testDomain = Test.BuildDao<LauncherToolbarDomainDao>(AccessorKind.Main);
            var testToolbars = Test.BuildDao<LauncherToolbarsEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var testScreens = Test.BuildDao<ScreensEntityDao>(AccessorKind.Main);

            var toolbarId = LauncherToolbarId.NewId();
            var fontId = FontId.NewId();

            testFonts.InsertFont(
                fontId,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testScreens.InsertScreen(
                new ScreenData() {
                    DeviceName = "screen",
                    Primary = false,
                    BitsPerPixel = 24,
                    DeviceBounds = new Rect(10, 20, 30, 40),
                    DeviceWorkingArea = new Rect(),
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testToolbars.InsertNewToolbar(
                toolbarId,
                fontId,
                "screen",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectScreenToolbar(toolbarId);
            Assert.Equal("screen", actual.ScreenName);
            Assert.Equal(10, actual.X);
            Assert.Equal(20, actual.Y);
            Assert.Equal(30, actual.Width);
            Assert.Equal(40, actual.Height);

            Assert.Single(testDomain.SelectAllScreenToolbars());
        }

        #endregion
    }
}
