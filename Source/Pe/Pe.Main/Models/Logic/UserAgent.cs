using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    internal class UserAgent : DisposerBase, IUserAgent
    {
        public UserAgent(string name, HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            Name = name;
            HttpClient = httpClient;
        }

        #region property

        ILogger Logger { get; }
        protected private HttpClient HttpClient { get; }

        public string Name { get; }

        Stopwatch Stopwatch { get; } = new Stopwatch();
        /// <summary>
        /// 最後に使用してからの経過時間。
        /// </summary>
        public TimeSpan LastElapsed => Stopwatch.Elapsed;

        #endregion

        #region function

        #endregion

        #region DisposerBase-IUserAgent



        #endregion

        #region IUserAgent

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.DeleteAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.GetAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.PostAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.PutAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.SendAsync(request, cancellationToken);
        }

        #endregion

    }

    internal class UserAgentFactory : IUserAgentFactory
    {
        public UserAgentFactory(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IDictionary<string, UserAgent> Pool { get; } = new Dictionary<string, UserAgent>();

        TimeSpan ClearTime { get; } = TimeSpan.FromSeconds(30);

        #endregion

        #region function

        IUserAgent CreateUserAgentCore(string name)
        {
            Debug.Assert(name != null);

            if(Pool.TryGetValue(name, out var ua)) {
                if(ClearTime < ua.LastElapsed) {
                    ua.Dispose();

                    var httpClient = new HttpClient();
                    var newUserAgent = new UserAgent(name, httpClient, LoggerFactory);
                    Pool[name] = newUserAgent;
                    return newUserAgent;
                }
                return ua;
            } else {
                var httpClient = new HttpClient();
                var newUserAgent = new UserAgent(name, httpClient, LoggerFactory);
                Pool.Add(name, newUserAgent);
                return newUserAgent;
            }
        }

        #endregion


        #region IUserAgentFactory

        public IUserAgent CreateUserAgent() => CreateUserAgentCore(string.Empty);

        public IUserAgent CreateUserAgent(string name) => CreateUserAgentCore(name);

        #endregion
    }
}
