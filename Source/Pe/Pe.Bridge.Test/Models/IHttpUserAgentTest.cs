using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class IHttpUserAgentExtensionsTest
    {
        #region function

        [Fact]
        public async Task GetStringAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.GetAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("abc")
                    };
                })
            ;
            var actual = await mock.Object.GetStringAsync(default!);
            Assert.Equal("abc", actual);
        }

        [Fact]
        public async Task GetStreamAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.GetAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}))
                    };
                })
            ;
            using var stream = await mock.Object.GetStreamAsync(default!);
            var actual = new byte[stream.Length];
            stream.Read(actual);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, actual);
        }

        [Fact]
        public async Task GetByteArrayAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.GetAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new ByteArrayContent(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
                    };
                })
            ;
            var actual = await mock.Object.GetByteArrayAsync(default!);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, actual);
        }

        #endregion
    }
}
