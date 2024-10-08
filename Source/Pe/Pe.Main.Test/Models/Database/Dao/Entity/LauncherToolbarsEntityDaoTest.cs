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
    public class LauncherToolbarsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Test()
        {
            var testToolbars = Test.BuildDao<LauncherToolbarsEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);

            var toolbarId1 = LauncherToolbarId.NewId();
            var toolbarId2 = LauncherToolbarId.NewId();
            var fontId = FontId.NewId();

            testFonts.InsertFont(
                fontId,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testToolbars.InsertNewToolbar(
                toolbarId1,
                fontId,
                "ScreenName1",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Equal([toolbarId1], testToolbars.SelectAllLauncherToolbarIds());

            testToolbars.InsertNewToolbar(
                toolbarId2,
                fontId,
                "ScreenName2",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.Contains(toolbarId1, testToolbars.SelectAllLauncherToolbarIds());
            Assert.Contains(toolbarId2, testToolbars.SelectAllLauncherToolbarIds());
        }

        [Fact]
        public void UpdateToolbarPositionTest()
        {
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
                "ScreenName",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testToolbars.SelectDisplayData(toolbarId);
            Assert.Equal(AppDesktopToolbarPosition.Right, actual1.ToolbarPosition);

            testToolbars.UpdateToolbarPosition(toolbarId, AppDesktopToolbarPosition.Bottom, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testToolbars.SelectDisplayData(toolbarId);
            Assert.Equal(AppDesktopToolbarPosition.Bottom, actual2.ToolbarPosition);
        }

        [Fact]
        public void UpdateIsTopmostTest()
        {
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
                "ScreenName",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testToolbars.SelectDisplayData(toolbarId);
            Assert.True(actual1.IsTopmost);

            testToolbars.UpdateIsTopmost(toolbarId, false, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testToolbars.SelectDisplayData(toolbarId);
            Assert.False(actual2.IsTopmost);
        }

        [Fact]
        public void UpdateIsAutoHideTest()
        {
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
                "ScreenName",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testToolbars.SelectDisplayData(toolbarId);
            Assert.False(actual1.IsAutoHide);

            testToolbars.UpdateIsAutoHide(toolbarId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testToolbars.SelectDisplayData(toolbarId);
            Assert.True(actual2.IsAutoHide);
        }

        [Fact]
        public void UpdateIsVisibleTest()
        {
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
                "ScreenName",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testToolbars.SelectDisplayData(toolbarId);
            Assert.True(actual1.IsVisible);

            testToolbars.UpdateIsVisible(toolbarId, false, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testToolbars.SelectDisplayData(toolbarId);
            Assert.False(actual2.IsVisible);
        }

        [Fact]
        public void UpdateDisplayDataTest()
        {
            var testToolbars = Test.BuildDao<LauncherToolbarsEntityDao>(AccessorKind.Main);
            var testGroups = Test.BuildDao<LauncherGroupsEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);

            var toolbarId = LauncherToolbarId.NewId();
            var groupId = LauncherGroupId.NewId();
            var fontId1 = FontId.NewId();
            var fontId2 = FontId.NewId();

            testGroups.InsertNewGroup(
                new LauncherGroupData() {
                    LauncherGroupId = groupId
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testFonts.InsertFont(
                fontId1,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            testFonts.InsertFont(
                fontId2,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testToolbars.InsertNewToolbar(
                toolbarId,
                fontId1,
                "ScreenName",
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = testToolbars.SelectDisplayData(toolbarId);
            Assert.Equal(LauncherGroupId.Empty, actual1.LauncherGroupId);
            Assert.Equal(AppDesktopToolbarPosition.Right, actual1.ToolbarPosition);
            Assert.Equal(LauncherToolbarIconDirection.LeftTop, actual1.IconDirection);
            Assert.Equal(IconBox.Normal, actual1.IconBox);
            Assert.Equal(fontId1, actual1.FontId);
            Assert.Equal(TimeSpan.FromMilliseconds(250), actual1.DisplayDelayTime);
            Assert.Equal(TimeSpan.FromSeconds(3), actual1.AutoHideTime);
            Assert.Equal(80, actual1.TextWidth);
            Assert.True(actual1.IsVisible);
            Assert.True(actual1.IsTopmost);
            Assert.False(actual1.IsAutoHide);
            Assert.True(actual1.IsIconOnly);

            testToolbars.UpdateDisplayData(new LauncherToolbarsDisplayData() {
                LauncherToolbarId = toolbarId,
                LauncherGroupId = groupId,
                ToolbarPosition = AppDesktopToolbarPosition.Top,
                IconDirection = LauncherToolbarIconDirection.Center,
                IconBox = IconBox.Large,
                FontId = fontId2,
                DisplayDelayTime = TimeSpan.FromSeconds(5),
                AutoHideTime = TimeSpan.FromDays(5),
                TextWidth = 160,
                IsVisible = false,
                IsTopmost = false,
                IsAutoHide = true,
                IsIconOnly = false,
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testToolbars.SelectDisplayData(toolbarId);
            Assert.Equal(groupId, actual2.LauncherGroupId);
            Assert.Equal(AppDesktopToolbarPosition.Top, actual2.ToolbarPosition);
            Assert.Equal(LauncherToolbarIconDirection.Center, actual2.IconDirection);
            Assert.Equal(IconBox.Large, actual2.IconBox);
            Assert.Equal(fontId2, actual2.FontId);
            Assert.Equal(TimeSpan.FromSeconds(5), actual2.DisplayDelayTime);
            Assert.Equal(TimeSpan.FromDays(5), actual2.AutoHideTime);
            Assert.Equal(160, actual2.TextWidth);
            Assert.False(actual2.IsVisible);
            Assert.False(actual2.IsTopmost);
            Assert.True(actual2.IsAutoHide);
            Assert.False(actual2.IsIconOnly);

        }

        #endregion
    }
}
