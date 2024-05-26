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
                test.Arguments = nameof(test.Arguments);
                test.Description = nameof(test.Description);
                test.WorkingDirectory = nameof(test.WorkingDirectory);
                test.IconPath = nameof(test.IconPath);
                test.IconIndex = 42;
                test.ShowCommand = PInvoke.Windows.SW.SW_NORMAL;

                Assert.Equal(file.FullName, test.TargetPath);
                Assert.Equal(nameof(test.Arguments), test.Arguments);
                Assert.Equal(nameof(test.Description), test.Description);
                Assert.Equal(nameof(test.WorkingDirectory), test.WorkingDirectory);
                Assert.Equal(nameof(test.IconPath), test.IconPath);
                Assert.Equal(42, test.IconIndex);
                Assert.Equal(PInvoke.Windows.SW.SW_NORMAL, test.ShowCommand);

                test.Save(Path.Combine(dir.FullName, "𩸽.lnk"));
            }

            using(var test = new ShortcutFile(Path.Combine(dir.FullName, "𩸽.lnk"))) {
                Assert.Equal(file.FullName, test.TargetPath);
                Assert.Equal(nameof(test.Arguments), test.Arguments);
                Assert.Equal(nameof(test.Description), test.Description);
                Assert.Equal(nameof(test.WorkingDirectory), test.WorkingDirectory);
                Assert.Equal(nameof(test.IconPath), test.IconPath);
                Assert.Equal(42, test.IconIndex);
                Assert.Equal(PInvoke.Windows.SW.SW_NORMAL, test.ShowCommand);
            }

            {
                var test = new ShortcutFile(Path.Combine(dir.FullName, "𩸽.lnk"));
                test.Dispose();

                Assert.Throws<ObjectDisposedException>(() => test.TargetPath);
                Assert.Throws<ObjectDisposedException>(() => test.Arguments);
                Assert.Throws<ObjectDisposedException>(() => test.Description);
                Assert.Throws<ObjectDisposedException>(() => test.WorkingDirectory);
                Assert.Throws<ObjectDisposedException>(() => test.IconPath);
                Assert.Throws<ObjectDisposedException>(() => test.IconIndex);
                Assert.Throws<ObjectDisposedException>(() => test.ShowCommand);
            }
        }

        #endregion
    }
}
