using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models
{
    public class PluginWebInstallRequestResponseTest
    {
        #region function

        [Fact]
        public void ArchiveFile_set_null_Test()
        {
            var test = new PluginWebInstallRequestResponse();
            var ex = Assert.Throws<ArgumentNullException>(() => test.ArchiveFile = null!);
            Assert.Equal("value", ex.ParamName);
        }

        #endregion
    }
}
