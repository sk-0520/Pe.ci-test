using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public interface IApplicationHttpUserAgentFactory
    {
        #region function

        IHttpUserAgent CreateAppHttpUserAgent();

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IHttpUserAgent"/>
    /// </summary>
    internal class HttpUserAgent: DisposerBase, IHttpUserAgent
    {
        #region variable

        int _referenceCount;

        #endregion
        public HttpUserAgent(string name, HttpClient httpClient, ILoggerFactory loggerFactory)
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

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return GetCoreAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return GetCoreAsync(requestUri, CancellationToken.None);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return PostCoreAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return PostCoreAsync(requestUri, content, CancellationToken.None);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return PutCoreAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            return PutCoreAsync(requestUri, content, CancellationToken.None);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return DeleteCoreAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return DeleteCoreAsync(requestUri, CancellationToken.None);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendCoreAsync(request, cancellationToken);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendCoreAsync(request, CancellationToken.None);
        }

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IHttpUserAgent"/>
    /// </summary>
    internal class UserAgentName: IHttpUserAgentName
    {
        #region function

        string JoinCore(string name, bool isEnabledSession, bool isEnabledCache)
        {
            var builder = new StringBuilder(32);

            if(0 < name.Length) {
                builder.Append(name);
            }

            if(isEnabledSession) {
                if(0 < builder.Length) {
                    builder.Append(Separator);
                }
                builder.Append(Session);
            }

            if(isEnabledCache) {
                if(0 < builder.Length) {
                    builder.Append(Separator);
                }
                builder.Append(Cache);
            }

            return builder.ToString();
        }

        #endregion


        #region IUserAgentName

        /// <summary>
        /// <inheritdoc cref="IHttpUserAgentName.Separator"/>
        /// </summary>
        public string Separator { get; } = ";";
        /// <summary>
        /// <inheritdoc cref="IHttpUserAgentName.Session"/>
        /// </summary>
        public string Session { get; } = "session";
        /// <summary>
        /// <inheritdoc cref="IHttpUserAgentName.Cache"/>
        /// </summary>
        public string Cache { get; } = "cache";

        public string Join(bool isEnabledSession, bool isEnabledCache) => JoinCore(string.Empty, isEnabledSession, isEnabledCache);
        public string Join(string name, bool isEnabledSession, bool isEnabledCache)
        {
            if(name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            if(name.IndexOf(Separator) != -1) {
                throw new ArgumentException(nameof(name));
            }

            return JoinCore(name, isEnabledSession, isEnabledCache);
        }

        #endregion
    }

    /// <inheritdoc cref="IHttpUserAgentFactory"/>
    internal class HttpUserAgentFactory: IHttpUserAgentFactory
    {
        public HttpUserAgentFactory(WebConfiguration webConfiguration, ProxyConfiguration proxyConfiguration, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            WebConfiguration = webConfiguration;
            ProxyConfiguration = proxyConfiguration;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }
        WebConfiguration WebConfiguration { get; }
        ProxyConfiguration ProxyConfiguration { get; }
        IDictionary<string, HttpUserAgent> Pool { get; } = new Dictionary<string, HttpUserAgent>();

        TimeSpan ClearTime { get; } = TimeSpan.FromSeconds(30);

        #endregion

        #region function

        void SetProxy(SocketsHttpHandler handler)
        {
            Debug.Assert(ProxyConfiguration.IsEnabled);

            Logger.LogInformation("プロキシを使用: {0}, {1}", ProxyConfiguration.Uri, ProxyConfiguration.CredentialIsEnabled);
            var proxy = new WebProxy(ProxyConfiguration.Uri);
            if(ProxyConfiguration.CredentialIsEnabled) {
                proxy.Credentials = new NetworkCredential(ProxyConfiguration.CredentialUser, ProxyConfiguration.CredentialPassword);
            }
            handler.Proxy = proxy;
            handler.UseProxy = true;
        }


        HttpUserAgent Create(string name)
        {
            var param = name.Split(UserAgentName.Separator, StringSplitOptions.RemoveEmptyEntries);

            var isEnabledCache = param.Any(i => i == UserAgentName.Cache);
            var isEnabledSession = param.Any(i => i == UserAgentName.Session);

            var handler = new SocketsHttpHandler() {
                UseCookies = isEnabledSession,
            };
            if(ProxyConfiguration.IsEnabled) {
                SetProxy(handler);
            }
            var cacheControl = new CacheControlHeaderValue() {
                NoCache = !isEnabledCache
            };
            var httpClient = new HttpClient(handler, true);
            httpClient.DefaultRequestHeaders.CacheControl = cacheControl;

            var newUserAgent = new HttpUserAgent(name, httpClient, LoggerFactory);
            return newUserAgent;
        }

        HttpUserAgent CreateUserAgentCore(string name)
        {
            Debug.Assert(name != null);


            if(Pool.TryGetValue(name, out var ua)) {
                if(ClearTime < ua.LastElapsed) {
                    Logger.LogDebug("再生成: {0}, {1} < {2}", name, ClearTime, ua.LastElapsed);
                    // 参照がなければ完全破棄、参照が残っていればGCに任せる
                    if(ua.ReferenceCount == 0) {
                        Logger.LogInformation("完全破棄", name);
                        ua.ReleaseClient();
                    }
                    ua.Dispose();

                    var newUserAgent = Create(name);
                    Pool[name] = newUserAgent;
                    return newUserAgent;
                }
                Logger.LogInformation("再使用: {0}", name);
                ua.Lease();
                return ua;
            } else {
                Logger.LogInformation("初回生成: {0}", name);
                var newUserAgent = Create(name);
                Pool.Add(name, newUserAgent);
                return newUserAgent;
            }
        }

        #endregion


        #region IHttpUserAgentFactory

        public IHttpUserAgentName UserAgentName { get; } = new UserAgentName();

        public HttpUserAgent CreateUserAgent() => CreateUserAgentCore(string.Empty);
        IHttpUserAgent IHttpUserAgentFactory.CreateUserAgent() => CreateUserAgent();

        public HttpUserAgent CreateUserAgent(string name) => CreateUserAgentCore(name);
        IHttpUserAgent IHttpUserAgentFactory.CreateUserAgent(string name) => CreateUserAgent(name);

        #endregion
    }
}
