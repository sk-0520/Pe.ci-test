using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Test;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class ShortcutFileTest
    {
        #region function

        [Fact]
        public void CreateLoadTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var file = TestIO.CreateEmptyFile(dir, "𩸽.dat");

            using(var test = new ShortcutFile()) {
                test.TargetPath = file.FullName;

                Assert.Equal(file.FullName, test.TargetPath);

                test.Save(Path.Combine(dir.FullName, "𩸽.lnk"));
            }

            using(var test = new ShortcutFile(Path.Combine(dir.FullName, "𩸽.lnk"))) {
                Assert.Equal(file.FullName, test.TargetPath);
            }
        }

        #endregion
    }
}
