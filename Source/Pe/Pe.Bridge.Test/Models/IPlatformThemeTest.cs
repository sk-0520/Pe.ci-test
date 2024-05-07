using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class IPlatformThemeLoaderExtensionsTest
    {
        #region function

        [Fact]
        public void GetTaskbarColor_ColorPrevalence_Test()
        {
            var mockPlatformTheme = new Mock<IPlatformTheme>();
            mockPlatformTheme.Setup(a => a.ColorPrevalence).Returns(true);
            mockPlatformTheme.Setup(a => a.AccentColor).Returns(Colors.Red);
            mockPlatformTheme.Setup(a => a.EnableTransparency).Returns(true);

            var actual = mockPlatformTheme.Object.GetTaskbarColor();
            Assert.Equal(Colors.Red, actual);
        }

        [Fact]
        public void GetTaskbarColor_not_ColorPrevalence_Test()
        {
            var mockPlatformTheme = new Mock<IPlatformTheme>();
            mockPlatformTheme.Setup(a => a.ColorPrevalence).Returns(false);
            mockPlatformTheme.Setup(a => a.AccentColor).Returns(Colors.White);
            mockPlatformTheme.Setup(a => a.GetWindowsThemeColors(It.IsAny<PlatformThemeKind>())).Returns(new PlatformThemeColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray));
            mockPlatformTheme.Setup(a => a.EnableTransparency).Returns(true);

            var actual = mockPlatformTheme.Object.GetTaskbarColor();
            Assert.Equal(Colors.Lime, actual);
        }

        [Fact]
        public void GetTaskbarColor_EnableTransparency_Test()
        {
            var mockPlatformTheme = new Mock<IPlatformTheme>();
            mockPlatformTheme.Setup(a => a.ColorPrevalence).Returns(false);
            mockPlatformTheme.Setup(a => a.AccentColor).Returns(Color.FromArgb(0x99, 0xff, 0xff, 0xff));
            mockPlatformTheme.Setup(a => a.GetWindowsThemeColors(It.IsAny<PlatformThemeKind>())).Returns(new PlatformThemeColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray));
            mockPlatformTheme.Setup(a => a.EnableTransparency).Returns(false);

            var actual = mockPlatformTheme.Object.GetTaskbarColor();
            Assert.Equal(Colors.Lime, actual);
        }

        #endregion
    }
}
