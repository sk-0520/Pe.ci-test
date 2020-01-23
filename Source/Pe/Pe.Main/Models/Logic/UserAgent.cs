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
    public interface IApplicationUserAgentFactory
    {
        #region function

        IUserAgent CreateAppUserAgent();

        #endregion
    }

    internal class UserAgent : DisposerBase, IUserAgent
    {
        #region variable

        int _referenceCount;

        #endregion
        public UserAgent(string name, HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            Name = name;
            HttpClient = httpClient;
            this._referenceCount = 1;
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

        public int ReferenceCount => this._referenceCount;

        #endregion

        #region function

        public void ReleaseClient()
        {
            if(!IsDisposed) {
                HttpClient.Dispose();
            }
        }

        public void Lease()
        {
            Interlocked.Increment(ref this._referenceCount);
        }

        private Task<HttpResponseMessage> GetCoreAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        private Task<HttpResponseMessage> PostCoreAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.PostAsync(requestUri, content, cancellationToken);
        }

        private Task<HttpResponseMessage> PutCoreAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.PutAsync(requestUri, content, cancellationToken);
        }

        private Task<HttpResponseMessage> DeleteCoreAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.DeleteAsync(requestUri, cancellationToken);
        }

        private Task<HttpResponseMessage> SendCoreAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch.Restart();
            return HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }


        #endregion

        #region DisposerBase-IUserAgent

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(0 < this._referenceCount) {
                    Interlocked.Decrement(ref this._referenceCount);
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IUserAgent

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return GetCoreAsync(requestUri, CancellationToken.None);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return GetCoreAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return PostCoreAsync(requestUri, content, CancellationToken.None);
        }
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return PostCoreAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            return PutCoreAsync(requestUri, content, CancellationToken.None);
        }
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return PutCoreAsync(requestUri, content, cancellationToken);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return DeleteCoreAsync(requestUri, CancellationToken.None);
        }
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return DeleteCoreAsync(requestUri, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendCoreAsync(request, CancellationToken.None);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendCoreAsync(request, cancellationToken);
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

        UserAgent CreateUserAgentCore(string name)
        {
            Debug.Assert(name != null);

            UserAgent Create(string name)
            {
                var httpClient = new HttpClient();
                var newUserAgent = new UserAgent(name, httpClient, LoggerFactory);

                return newUserAgent;
            }

            if(Pool.TryGetValue(name, out var ua)) {
                if(ClearTime < ua.LastElapsed) {
                    Logger.LogDebug("再生成: {0}", name);
                    // 参照がなければ完全破棄、参照が残っていればGCに任せる
                    if(ua.ReferenceCount == 0) {
                        Logger.LogTrace("完全破棄", name);
                        ua.ReleaseClient();
                    }
                    ua.Dispose();

                    var newUserAgent = Create(name);
                    Pool[name] = newUserAgent;
                    return newUserAgent;
                }
                Logger.LogDebug("再使用: {0}", name);
                ua.Lease();
                return ua;
            } else {
                Logger.LogDebug("初回生成: {0}", name);
                var newUserAgent = Create(name);
                Pool.Add(name, newUserAgent);
                return newUserAgent;
            }
        }

        #endregion


        #region IUserAgentFactory2

        public UserAgent CreateUserAgent() => CreateUserAgentCore(string.Empty);
        IUserAgent IUserAgentFactory.CreateUserAgent() => CreateUserAgent();

        public UserAgent CreateUserAgent(string name) => CreateUserAgentCore(name);
        IUserAgent IUserAgentFactory.CreateUserAgent(string name) => CreateUserAgent(name);

        #endregion
    }
}
