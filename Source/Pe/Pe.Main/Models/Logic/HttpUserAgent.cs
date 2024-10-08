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
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public interface IApplicationHttpUserAgentFactory
    {
        #region function

        /// <summary>
        /// アプリケーション用のUAを生成。
        /// </summary>
        /// <returns></returns>
        IHttpUserAgent CreateAppHttpUserAgent();

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IHttpUserAgent"/>
    /// </summary>
    internal class HttpUserAgent: DisposerBase, IHttpUserAgent
    {
        #region variable

        private int _referenceCount;

        #endregion
        public HttpUserAgent(string name, HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            Name = name;
            HttpClient = httpClient;
            this._referenceCount = 1;
        }

        #region property

        private ILogger Logger { get; }
        protected private HttpClient HttpClient { get; }

        public string Name { get; }

        private Stopwatch Stopwatch { get; } = new Stopwatch();
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

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendCoreAsync(request, cancellationToken);
        }

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IHttpUserAgent"/>
    /// </summary>
    internal class UserAgentName: IHttpUserAgentName
    {
        #region function

        private string JoinCore(string name, bool isEnabledSession, bool isEnabledCache)
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
                throw new ArgumentException(null, nameof(name));
            }

            return JoinCore(name, isEnabledSession, isEnabledCache);
        }

        #endregion
    }

    /// <inheritdoc cref="IHttpUserAgentFactory"/>
    internal class HttpUserAgentFactory: IHttpUserAgentFactory
    {
        public HttpUserAgentFactory(WebConfiguration webConfiguration, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            WebConfiguration = webConfiguration;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        private WebConfiguration WebConfiguration { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IDictionary<string, HttpUserAgent> Pool { get; } = new Dictionary<string, HttpUserAgent>();

        private TimeSpan ClearTime { get; } = TimeSpan.FromSeconds(30);

        #endregion

        #region function

        private void SetProxy(SocketsHttpHandler handler, AppProxySettingData proxySetting)
        {
            Debug.Assert(proxySetting.ProxyIsEnabled);

            if(!Uri.TryCreate(proxySetting.ProxyUrl, UriKind.Absolute, out var proxyUri)) {
                Logger.LogWarning("プロキシURLが不正: {Uri}", proxySetting.ProxyUrl);
                return;
            }

            Logger.LogInformation("プロキシを使用: Url = {Uri}, 認証 = {CredentialIsEnabled}", proxyUri, proxySetting.CredentialIsEnabled);
            var proxy = new WebProxy(proxyUri);
            if(proxySetting.CredentialIsEnabled) {
                proxy.Credentials = new NetworkCredential(proxySetting.CredentialUser, proxySetting.CredentialPassword);
            }
            handler.Proxy = proxy;
            handler.UseProxy = true;
        }

        private HttpUserAgent Create(string name)
        {
            var param = name.Split(UserAgentName.Separator, StringSplitOptions.RemoveEmptyEntries);

            var isEnabledCache = param.Any(i => i == UserAgentName.Cache);
            var isEnabledSession = param.Any(i => i == UserAgentName.Session);

            var handler = new SocketsHttpHandler() {
                UseCookies = isEnabledSession,
            };

            AppProxySettingData settingAppProxySettingData;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appProxySettingEntityDao = new AppProxySettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                settingAppProxySettingData = appProxySettingEntityDao.SelectProxySetting();
            }
            if(settingAppProxySettingData.ProxyIsEnabled) {
                SetProxy(handler, settingAppProxySettingData);
            }

            var cacheControl = new CacheControlHeaderValue() {
                NoCache = !isEnabledCache
            };
            var httpClient = new HttpClient(handler, true);
            httpClient.DefaultRequestHeaders.CacheControl = cacheControl;

            var ua = ApplicationStringFormats.GetHttpUserAgentValue(WebConfiguration.ClientUserAgentFormat);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", ua);

            var newUserAgent = new HttpUserAgent(name, httpClient, LoggerFactory);
            return newUserAgent;
        }

        private HttpUserAgent CreateUserAgentCore(string name)
        {
            Debug.Assert(name != null);


            if(Pool.TryGetValue(name, out var ua)) {
                if(ClearTime < ua.LastElapsed) {
                    Logger.LogDebug("再生成: {0}, {1} < {2}", name, ClearTime, ua.LastElapsed);
                    // 参照がなければ完全破棄、参照が残っていればGCに任せる
                    if(ua.ReferenceCount == 0) {
                        Logger.LogInformation("完全破棄: {Name}", name);
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
