using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UserAgent : DisposerBase, IUserAgent
    {
        public UserAgent(HttpClient httpClient, ILoggerFactory loggerFactory)
            : this(string.Empty, httpClient, loggerFactory)
        { }

        public UserAgent(string name, HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            Name = name;
            HttpClient = httpClient;
        }

        #region property

        ILogger Logger { get; }
        HttpClient HttpClient { get; }

        public string Name { get; }

        #endregion

        #region function

        #endregion


        #region DisposerBase-IUserAgent



        #endregion

        #region IUserAgent

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return HttpClient.DeleteAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return HttpClient.GetAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return HttpClient.PostAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return HttpClient.PutAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return HttpClient.SendAsync(request, cancellationToken);
        }

        #endregion

    }

    public class UserAgentFactory : IUserAgentFactory
    {
        #region IUserAgentFactory

        public IUserAgent CreateUserAgent()
        {
            throw new NotImplementedException();
        }

        public IUserAgent CreateUserAgent(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
