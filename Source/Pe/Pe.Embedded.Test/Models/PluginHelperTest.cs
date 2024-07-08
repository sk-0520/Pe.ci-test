using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Embedded.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Embedded.Test.Models
{
    public class PluginHelperTest
    {
        #region function

        [Fact]
        public void GetPluginAssemblyTest()
        {
            var assembly = PluginHelper.GetPluginAssembly();
            var actual = assembly.GetName();
            Assert.Equal("Pe.Embedded", actual.Name);
        }

        #endregion
    }
}
