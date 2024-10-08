using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Property;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using static System.Net.WebRequestMethods;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class NewVersionCheckerTest
    {
        #region property

        private static ApplicationInformation IgnoreApplicationProcessInformation => new ApplicationInformation(new Version(), string.Empty);

        private Test Test { get; } = Test.Create(TestSetup.Http);

        #endregion

        #region function

        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.SwitchingProtocols)]
        [InlineData(HttpStatusCode.Processing)]
        [InlineData(HttpStatusCode.EarlyHints)]
        [InlineData(HttpStatusCode.Ambiguous)]
        [InlineData(HttpStatusCode.Moved)]
        [InlineData(HttpStatusCode.Found)]
        [InlineData(HttpStatusCode.RedirectMethod)]
        [InlineData(HttpStatusCode.NotModified)]
        [InlineData(HttpStatusCode.UseProxy)]
        [InlineData(HttpStatusCode.Unused)]
        [InlineData(HttpStatusCode.RedirectKeepVerb)]
        [InlineData(HttpStatusCode.PermanentRedirect)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.PaymentRequired)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.MethodNotAllowed)]
        [InlineData(HttpStatusCode.NotAcceptable)]
        [InlineData(HttpStatusCode.ProxyAuthenticationRequired)]
        [InlineData(HttpStatusCode.RequestTimeout)]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.Gone)]
        [InlineData(HttpStatusCode.LengthRequired)]
        [InlineData(HttpStatusCode.PreconditionFailed)]
        [InlineData(HttpStatusCode.RequestEntityTooLarge)]
        [InlineData(HttpStatusCode.RequestUriTooLong)]
        [InlineData(HttpStatusCode.UnsupportedMediaType)]
        [InlineData(HttpStatusCode.RequestedRangeNotSatisfiable)]
        [InlineData(HttpStatusCode.ExpectationFailed)]
        [InlineData(HttpStatusCode.MisdirectedRequest)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.Locked)]
        [InlineData(HttpStatusCode.FailedDependency)]
        [InlineData(HttpStatusCode.UpgradeRequired)]
        [InlineData(HttpStatusCode.PreconditionRequired)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.RequestHeaderFieldsTooLarge)]
        [InlineData(HttpStatusCode.UnavailableForLegalReasons)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotImplemented)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.HttpVersionNotSupported)]
        [InlineData(HttpStatusCode.VariantAlsoNegotiates)]
        [InlineData(HttpStatusCode.InsufficientStorage)]
        [InlineData(HttpStatusCode.LoopDetected)]
        [InlineData(HttpStatusCode.NotExtended)]
        [InlineData(HttpStatusCode.NetworkAuthenticationRequired)]
        public async Task RequestUpdateDataAsync_not_IsSuccessStatusCode_Test(HttpStatusCode httpStatusCode)
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(httpStatusCode)))
            ;

            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);
            var actual = await test.RequestUpdateDataAsync(new Uri("http://localhost.invalid"), CancellationToken.None);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("null")]
        [InlineData("[]")]
        public async Task RequestUpdateDataAsync_throw_Test(string json)
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(json),
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);
            var actual = await test.RequestUpdateDataAsync(new Uri("http://localhost.invalid"), CancellationToken.None);
            Assert.Null(actual);
        }

        public static TheoryData<NewVersionData, string> RequestUpdateDataAsyncData => new() {
            {
                new NewVersionData() {},
                "{}"
            },
            {
                new NewVersionData() {
                    Items = []
                },
                @"{
                    ""items"": [
                    ]
                }"
            },
            {
                new NewVersionData() {
                    Items = [
                        new NewVersionItemData() {
                            Release = new DateTime(2024, 5, 18, 15,0, 0, DateTimeKind.Utc),
                            Version = new Version(1, 2, 3),
                            Revision = "Revision",
                            Platform = "x128",
                            MinimumVersion = new Version(4, 5, 6),
                            NoteUri = new Uri("http://localhost.invalid/note"),
                            ArchiveUri = new Uri("http://localhost.invalid/archive"),
                            ArchiveSize = 1024,
                            ArchiveKind = nameof(NewVersionItemData.ArchiveKind),
                            ArchiveHashKind = nameof(NewVersionItemData.ArchiveHashKind),
                            ArchiveHashValue = nameof(NewVersionItemData.ArchiveHashValue),
                        }
                    ]
                },
                @"{
                    ""items"": [
                        {
                            ""release"": ""2024-05-18T15:00:00Z"",
                            ""version"": ""1.2.3"",
                            ""revision"": ""Revision"",
                            ""platform"": ""x128"",
                            ""minimum_version"": ""4.5.6"",
                            ""note_uri"": ""http://localhost.invalid/note"",
                            ""archive_uri"": ""http://localhost.invalid/archive"",
                            ""archive_size"": 1024,
                            ""archive_kind"": ""ArchiveKind"",
                            ""archive_hash_kind"": ""ArchiveHashKind"",
                            ""archive_hash_value"": ""ArchiveHashValue""
                        }
                    ]
                }"
            }
        };
        [Theory]
        [MemberData(nameof(RequestUpdateDataAsyncData))]
        public async Task RequestUpdateDataAsyncTest(NewVersionData expected, string json)
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(json)
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);
            var actual = await test.RequestUpdateDataAsync(new Uri("http://localhost.invalid"), CancellationToken.None);
            Assert.NotNull(actual);

            Assert.Equal(expected.Items.Length, actual.Items.Length);
            for(var i = 0; i < expected.Items.Length; i++) {
                var expectedItem = expected.Items[i];
                var actualItem = actual.Items[i];

                Assert.Equal(expectedItem.Release, actualItem.Release);
                Assert.Equal(expectedItem.Version, actualItem.Version);
                Assert.Equal(expectedItem.Revision, actualItem.Revision);
                Assert.Equal(expectedItem.Platform, actualItem.Platform);
                Assert.Equal(expectedItem.MinimumVersion, actualItem.MinimumVersion);
                Assert.Equal(expectedItem.NoteUri, actualItem.NoteUri);
                Assert.Equal(expectedItem.ArchiveUri, actualItem.ArchiveUri);
                Assert.Equal(expectedItem.ArchiveSize, actualItem.ArchiveSize);
                Assert.Equal(expectedItem.ArchiveKind, actualItem.ArchiveKind);
                Assert.Equal(expectedItem.ArchiveHashKind, actualItem.ArchiveHashKind);
                Assert.Equal(expectedItem.ArchiveHashValue, actualItem.ArchiveHashValue);
            }
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsync_not_IsSuccessStatusCode_Test()
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);
            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("null")]
        [InlineData("[]")]
        public async Task CheckApplicationNewVersionAsync_Deserialize_Test(string json)
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(json),
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);

            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.Null(actual);
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsync_not_Platform_Test()
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(@"
                    {
                        ""items"": [
                            {
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""1.2.3"",
                                ""revision"": ""Revision"",
                                ""platform"": ""x128"",
                                ""minimum_version"": ""4.5.6"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }
                        ]
                    }
                    "),
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);

            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.Null(actual);
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsync_not_MinimumVersion_Test()
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent($@"
                    {{
                        ""items"": [
                            {{
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""0.0"",
                                ""revision"": ""Revision"",
                                ""platform"": ""{ProcessArchitecture.ApplicationArchitecture}"",
                                ""minimum_version"": ""1.2.3.5"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }}
                        ]
                    }}
                    "),
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);

            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.Null(actual);
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsync_not_Version_Test()
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent($@"
                    {{
                        ""items"": [
                            {{
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""1.2.3.4"",
                                ""revision"": ""Revision"",
                                ""platform"": ""{ProcessArchitecture.ApplicationArchitecture}"",
                                ""minimum_version"": ""1.2.3.4"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }}
                        ]
                    }}
                    "),
                }))
            ;
            var test = new NewVersionChecker(IgnoreApplicationProcessInformation, Test.UserAgentManager, NullLoggerFactory.Instance);

            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.Null(actual);
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsyncTest()
        {
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent($@"
                    {{
                        ""items"": [
                            {{
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""1.2.3.5"",
                                ""revision"": ""Revision"",
                                ""platform"": ""{ProcessArchitecture.ApplicationArchitecture}"",
                                ""minimum_version"": ""1.2.3.4"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }},
                            {{
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""1.2.3.6"",
                                ""revision"": ""Revision"",
                                ""platform"": ""{ProcessArchitecture.ApplicationArchitecture}"",
                                ""minimum_version"": ""1.2.3.4"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }}
                        ]
                    }}
                    "),
                }))
            ;
            var test = new NewVersionChecker(new ApplicationInformation(new Version(1, 2, 3, 4), ProcessArchitecture.ApplicationArchitecture), Test.UserAgentManager, NullLoggerFactory.Instance);

            var actual = await test.CheckApplicationNewVersionAsync(new[] { "http://localhost.invalid/version_check_url_item/1" }, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.Equal(new Version(1, 2, 3, 6), actual.Version);
        }

        [Fact]
        public async Task CheckApplicationNewVersionAsync_not200_throw_200_Test()
        {
            var mockHttpUserAgent = new Mock<IHttpUserAgent>();
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/1")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)))
            ;
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/2")), It.IsAny<CancellationToken>()))
                .Throws(new Exception())
            ;
            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Get && a.RequestUri == new Uri("http://localhost.invalid/version_check_url_item/3")), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent($@"
                    {{
                        ""items"": [
                            {{
                                ""release"": ""2024-05-18T15:00:00Z"",
                                ""version"": ""1.2.3.6"",
                                ""revision"": ""!2!"",
                                ""platform"": ""{ProcessArchitecture.ApplicationArchitecture}"",
                                ""minimum_version"": ""1.2.3.4"",
                                ""note_uri"": ""http://localhost.invalid/note"",
                                ""archive_uri"": ""http://localhost.invalid/archive"",
                                ""archive_size"": 1024,
                                ""archive_kind"": ""ArchiveKind"",
                                ""archive_hash_kind"": ""ArchiveHashKind"",
                                ""archive_hash_value"": ""ArchiveHashValue""
                            }}
                        ]
                    }}
                    "),
                }))
            ;

            var test = new NewVersionChecker(new ApplicationInformation(new Version(1, 2, 3, 4), ProcessArchitecture.ApplicationArchitecture), Test.UserAgentManager, NullLoggerFactory.Instance);
            var actual = await test.CheckApplicationNewVersionAsync(new[] {
                "http://localhost.invalid/version_check_url_item/1",
                "http://localhost.invalid/version_check_url_item/2",
                "http://localhost.invalid/version_check_url_item/3"
            }, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.Equal(new Version(1, 2, 3, 6), actual.Version);
            Assert.Equal("!2!", actual.Revision);
        }

        [Theory]
        [InlineData(null, null, "00000000-0000-0000-0000-000000000000", "1.2.3")]
        [InlineData(null, "", "00000000-0000-0000-0000-000000000000", "1.2.3")]
        [InlineData(null, "  ", "00000000-0000-0000-0000-000000000000", "1.2.3")]
        [InlineData(null, ":", "00000000-0000-0000-0000-000000000000", "1.2.3")]
        [InlineData("http://localhost.invalid/00000000-0000-0000-0000-000000000000/1.02.003", "http://localhost.invalid/${PLUGIN-ID}/${PLUGIN-VERSION}", "00000000-0000-0000-0000-000000000000", "1.2.3")]
        [InlineData("http://localhost.invalid/00000001-0002-0003-0004-000000000005/1.02.003", "http://localhost.invalid/${PLUGIN-ID}/${PLUGIN-VERSION}", "00000001000200030004000000000005", "1.2.3")]
        public void BuildPluginUriTest(string? expectedUrl, string? baseUrl, string pluginId, string pluginVersion)
        {
            var mockUserAgentManager = new Mock<IUserAgentManager>();
            var test = new NewVersionChecker(new ApplicationInformation(new Version(1, 2, 3, 4), ProcessArchitecture.ApplicationArchitecture), mockUserAgentManager.Object, NullLoggerFactory.Instance);
            var actual = test.BuildPluginUri(baseUrl!, PluginId.Parse(pluginId), Version.Parse(pluginVersion));
            Assert.Equal(expectedUrl, actual?.ToString());
        }

        public static TheoryData<Version, IEnumerable<NewVersionItemData>> GetPluginNewVersionItem_null_Data => new()
        {
            // なし
            {
                new Version(),
                Array.Empty<NewVersionItemData>()
            },
            // アーキテクチャ未達
            {
                new Version(),
                new NewVersionItemData[] {
                    new NewVersionItemData() {
                        Platform = "x128",
                    }
                }
            },
            // アプリケーション最小バージョン未達
            {
                new Version(),
                new NewVersionItemData[] {
                    new NewVersionItemData() {
                        Platform = ProcessArchitecture.ApplicationArchitecture,
                        MinimumVersion = new Version(1, 2, 3, 5)
                    }
                }
            },
            // プラグインバージョン未達
            {
                new Version(2, 3, 4, 5),
                new NewVersionItemData[] {
                    new NewVersionItemData() {
                        Platform = ProcessArchitecture.ApplicationArchitecture,
                        MinimumVersion = new Version(1, 2, 3, 4),
                        Version = new Version(2, 3, 4, 5)
                    }
                }
            },
            {
                new Version(2, 3, 4, 6),
                new NewVersionItemData[] {
                    new NewVersionItemData() {
                        Platform = ProcessArchitecture.ApplicationArchitecture,
                        MinimumVersion = new Version(1, 2, 3, 4),
                        Version = new Version(2, 3, 4, 5)
                    }
                }
            }
        };

        [Theory]
        [MemberData(nameof(GetPluginNewVersionItem_null_Data))]
        public void GetPluginNewVersionItem_null_Test(Version pluginVersion, IEnumerable<NewVersionItemData> items)
        {
            var mockUserAgentManager = new Mock<IUserAgentManager>();
            var test = new NewVersionChecker(new ApplicationInformation(new Version(1, 2, 3, 4), ProcessArchitecture.ApplicationArchitecture), mockUserAgentManager.Object, NullLoggerFactory.Instance);
            var actual = test.GetPluginNewVersionItem(pluginVersion, items);
            Assert.Null(actual);
        }

        [Fact]
        public void GetPluginNewVersionItemTest()
        {
            var pluginVersion = new Version(1, 2, 3, 6);
            var items = new[] {
                new NewVersionItemData() {
                    Platform = ProcessArchitecture.ApplicationArchitecture,
                    MinimumVersion = new Version(),
                    Version = new Version(1, 2, 3, 4)
                },
                new NewVersionItemData() {
                    Platform = ProcessArchitecture.ApplicationArchitecture,
                    MinimumVersion = new Version(),
                    Version = new Version(1, 2, 3, 7)
                },
                new NewVersionItemData() {
                    Platform = ProcessArchitecture.ApplicationArchitecture,
                    MinimumVersion = new Version(),
                    Version = new Version(1, 2, 3, 5)
                }
            };

            var mockUserAgentManager = new Mock<IUserAgentManager>();
            var test = new NewVersionChecker(new ApplicationInformation(new Version(1, 2, 3, 4), ProcessArchitecture.ApplicationArchitecture), mockUserAgentManager.Object, NullLoggerFactory.Instance);
            var actual = test.GetPluginNewVersionItem(pluginVersion, items);
            Assert.NotNull(actual);
            Assert.Equal(new Version(1, 2, 3, 7), actual.Version);
        }

        #endregion
    }
}
