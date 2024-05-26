using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Test;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class IconLoaderTest
    {
        public IconLoaderTest()
        {
            var projectTestPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            IconPath = Path.Combine(projectTestPath, "App-debug.ico");
        }

        #region property

        public string IconPath { get; }

        #endregion

        #region function

        [WpfTheory]
        [InlineData(IconBox.Small)]
        [InlineData(IconBox.Normal)]
        [InlineData(IconBox.Big)]
        [InlineData(IconBox.Large)]
        public void Icon_Test(IconBox iconBox)
        {
            var test = new IconLoader(NullLoggerFactory.Instance);
            var icon = test.Load(IconPath, 0, new IconSize(iconBox, IconSize.DefaultScale));
            Assert.NotNull(icon);
            // いくない
            Assert.True((int)iconBox <= icon.PixelWidth);
            Assert.True((int)iconBox <= icon.PixelHeight);
        }

        #endregion
    }
}
