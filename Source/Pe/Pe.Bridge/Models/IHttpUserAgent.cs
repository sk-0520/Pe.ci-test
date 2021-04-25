using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// <see cref="HttpClient"/>を意識せずに(寿命とか)に <see cref="IDisposable.Dispose"/> できる子。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHttpUserAgent: IDisposable
    {
        #region function

        /// <summary>
        /// 簡易 HTTP GET 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        /// <inheritdoc cref="GetAsync(Uri, CancellationToken)"/>
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        /// <summary>
        /// 簡易 HTTP POST 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        /// <inheritdoc cref="PostAsync(Uri, HttpContent, CancellationToken)"/>
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);

        /// <summary>
        /// 簡易 HTTP PUT 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        /// <inheritdoc cref="PutAsync(Uri, HttpContent, CancellationToken)"/>
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);

        /// <summary>
        /// 簡易 HTTP DELETE 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        /// <inheritdoc cref="DeleteAsync(Uri, CancellationToken)"/>
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);

        /// <summary>
        /// 簡易 HTTP 要求。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        /// <inheritdoc cref="SendAsync(HttpRequestMessage, CancellationToken)"/>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        #endregion
    }

    public static class IHttpUserAgentExtensions
    {
        #region function

        /// <summary>
        /// 簡易 GET 文字列要求。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 文字列本文。</returns>
        public static Task<string> GetStringAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStringAsync();
            }, cancellationToken).Unwrap();
        }
        /// <inheritdoc cref="GetStringAsync(IHttpUserAgent, Uri, CancellationToken)"/>
        public static Task<string> GetStringAsync(this IHttpUserAgent @this, Uri requestUri) => GetStringAsync(@this, requestUri, CancellationToken.None);

        /// <summary>
        /// 簡易 GET <see cref="Stream"/>要求。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 <see cref="Stream"/>本文。</returns>
        public static Task<Stream> GetStreamAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStreamAsync();
            }, cancellationToken).Unwrap();
        }
        /// <inheritdoc cref="GetStreamAsync(IHttpUserAgent, Uri, CancellationToken)"/>
        public static Task<Stream> GetStreamAsync(this IHttpUserAgent @this, Uri requestUri) => GetStreamAsync(@this, requestUri, CancellationToken.None);

        /// <summary>
        /// 簡易 GET byte配列要求。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 byte配列本文。</returns>
        public static Task<byte[]> GetByteArrayAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsByteArrayAsync();
            }, cancellationToken).Unwrap();
        }
        /// <inheritdoc cref="GetByteArrayAsync(IHttpUserAgent, Uri, CancellationToken)"/>
        public static Task<byte[]> GetByteArrayAsync(this IHttpUserAgent @this, Uri requestUri) => GetByteArrayAsync(@this, requestUri, CancellationToken.None);

        #endregion
    }

    /// <summary>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHttpUserAgentName
    {
        #region property

        /// <summary>
        /// 分割文字列。
        /// </summary>
        string Separator { get; }
        /// <summary>
        /// セッション使用文字列。
        /// </summary>
        string Session { get; }
        /// <summary>
        /// キャッシュ使用文字列。
        /// </summary>
        string Cache { get; }

        #endregion

        #region function

        string Join(bool isEnabledSession, bool isEnabledCache);
        string Join(string name, bool isEnabledSession, bool isEnabledCache);

        #endregion
    }

    /// <summary>
    /// <see cref="IHttpUserAgent"/>生成処理。
    /// <para>生成したやつは <see cref="IDisposable.Dispose"/> すること。</para>
    /// HttpClientFactoryを使いたかったけど今の仕組みに乗せるのは難しそうな気がした(裏で使うかも)。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHttpUserAgentFactory
    {
        #region property

        IHttpUserAgentName UserAgentName { get; }

        #endregion

        #region function

        public IHttpUserAgent CreateUserAgent();
        public IHttpUserAgent CreateUserAgent(string name);

        #endregion
    }

}
