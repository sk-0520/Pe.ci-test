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
        public async Task GetAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                    };
                })
            ;
            var actual = await mock.Object.GetAsync(default!);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task PostAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Post), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                    };
                })
            ;
            var actual = await mock.Object.PostAsync(default!, default!);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Put), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                    };
                })
            ;
            var actual = await mock.Object.PutAsync(default!, default!);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Delete), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                    };
                })
            ;
            var actual = await mock.Object.DeleteAsync(default!, default!);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task GetStringAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
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
                .Setup(a => a.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StreamContent(new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }))
                    };
                })
            ;
            using var stream = await mock.Object.GetStreamAsync(default!);
            var actual = new byte[stream.Length];
            await stream.ReadAsync(actual);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, actual);
        }

        [Fact]
        public async Task GetByteArrayAsyncTest()
        {
            var mock = new Mock<IHttpUserAgent>();
            mock
                .Setup(a => a.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
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
