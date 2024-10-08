using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using System.Threading;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Library.Database;
using System.Data;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using System.Net.Http.Json;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Element.Feedback
{
    public class FeedbackElementTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Http | TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public async Task SuccessTest()
        {
            var applicationConfiguration = Test.GetApplicationConfiguration(this);

            var mockCultureService = new Mock<ICultureService>();
            var mockOrderManager = new Mock<IOrderManager>();
            var mockLog = MockLog.Create();

            var mainDatabaseBarrier = Test.DiContainer.Build<IMainDatabaseBarrier>();
            var databaseStatementLoader = Test.DiContainer.Build<IDatabaseStatementLoader>();

            Test.MockHttpUserAgent
                .Setup(a => a.SendAsync(It.Is<HttpRequestMessage>(a => a.Method == HttpMethod.Post), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = JsonContent.Create(new FeedbackResponse() {
                        Success = true,
                        Message = "Message",
                    })
                }))
            ;

            var test = new FeedbackElement(
                applicationConfiguration.Api,
                mainDatabaseBarrier,
                databaseStatementLoader,
                mockCultureService.Object,
                mockOrderManager.Object,
                Test.UserAgentManager,
                mockLog.Factory.Object
            );
            await test.InitializeAsync(CancellationToken.None);

            await test.SendAsync(new Main.Models.Data.FeedbackInputData() {
                Kind = Main.Models.Data.FeedbackKind.Others,
                Subject = "Subject",
                Content = "Content",
            }, CancellationToken.None);

            Assert.Equal(RunningState.End, test.SendStatus.State);

            mockLog.VerifyMessage(LogLevel.Information, "送信完了", Times.Once());
        }

        #endregion
    }
}
