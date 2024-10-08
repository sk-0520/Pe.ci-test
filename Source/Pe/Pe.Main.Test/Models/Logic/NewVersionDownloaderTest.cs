using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class NewVersionDownloaderTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Http);

        #endregion

        #region function

        [Fact]
        public async Task ChecksumAsync_NotExists_Test()
        {
            var mockLog = MockLog.Create();
            var applicationConfiguration = Test.GetApplicationConfiguration(this);
            var test = Test.DiContainer.Build<NewVersionDownloader>(applicationConfiguration, mockLog.Factory.Object);

            var actual = await test.ChecksumAsync(
                new NewVersionItemData() {
                },
                new System.IO.FileInfo("NUL"),
                Test.DiContainer.Build<NullNotifyProgress>(),
                CancellationToken.None
            );
            Assert.False(actual);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "検査ファイルが存在しない:", Times.Once());
        }

        [Fact]
        public async Task ChecksumAsync_NotFileSize_Test()
        {
            var methodDir = TestIO.InitializeMethod(this);
            var file = TestIO.CreateTextFile(methodDir, "data.dat", "abc");

            var mockLog = MockLog.Create();
            var applicationConfiguration = Test.GetApplicationConfiguration(this);
            var test = Test.DiContainer.Build<NewVersionDownloader>(applicationConfiguration, mockLog.Factory.Object);

            var actual = await test.ChecksumAsync(
                new NewVersionItemData() {
                    ArchiveSize = 0,
                },
                file,
                Test.DiContainer.Build<NullNotifyProgress>(),
                CancellationToken.None
            );
            Assert.False(actual);
            mockLog.VerifyMessageStartsWith(LogLevel.Warning, "ファイルサイズが異なる:", Times.Once());
        }

        #endregion
    }
}
