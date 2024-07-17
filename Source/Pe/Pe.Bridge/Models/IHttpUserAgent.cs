using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// <see cref="HttpClient"/>を意識せずに(寿命とか) <see cref="IDisposable.Dispose"/> できる子。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IHttpUserAgent: IDisposable
    {
        #region function

        /// <summary>
        /// HTTP 要求。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        #endregion
    }

    public static class IHttpUserAgentExtensions
    {
        #region function

        /// <summary>
        /// HTTP GET 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        public static Task<HttpResponseMessage> GetAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken);
        }

        /// <summary>
        /// HTTP POST 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        public static Task<HttpResponseMessage> PostAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content }, cancellationToken);
        }

        /// <summary>
        /// HTTP PUT 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        public static Task<HttpResponseMessage> PutAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content }, cancellationToken);
        }

        /// <summary>
        /// HTTP DELETE 要求。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答。</returns>
        public static Task<HttpResponseMessage> DeleteAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri), cancellationToken);
        }


        /// <summary>
        /// 簡易 GET 文字列要求。
        /// </summary>
        /// <param name="httpUserAgent"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 文字列本文。</returns>
        public static Task<string> GetStringAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStringAsync(cancellationToken);
            }, cancellationToken).Unwrap();
        }

        /// <summary>
        /// 簡易 GET <see cref="Stream"/>要求。
        /// </summary>
        /// <param name="httpUserAgent"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 <see cref="Stream"/>本文。</returns>
        public static Task<Stream> GetStreamAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStreamAsync(cancellationToken);
            }, cancellationToken).Unwrap();
        }

        /// <summary>
        /// 簡易 GET byte配列要求。
        /// </summary>
        /// <param name="httpUserAgent"></param>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>HTTP 応答 byte配列本文。</returns>
        public static Task<byte[]> GetByteArrayAsync(this IHttpUserAgent httpUserAgent, Uri requestUri, CancellationToken cancellationToken = default)
        {
            return httpUserAgent.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsByteArrayAsync(cancellationToken);
            }, cancellationToken).Unwrap();
        }

        #endregion
    }

    /// <summary>
    /// <see cref="IHttpUserAgent"/>を生成する際に使用する名前の生成器。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
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
    /// </summary>
    /// <remarks>
    /// <para>生成したやつは <see cref="IDisposable.Dispose"/> すること。</para>
    /// <para>HttpClientFactoryを使いたかったけど今の仕組みに乗せるのは難しそうな気がした(裏で使うかも)。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IHttpUserAgentFactory
    {
        #region property

        IHttpUserAgentName UserAgentName { get; }

        #endregion

        #region function

        /// <summary>
        /// 標準の<see cref="IHttpUserAgent"/>を生成。
        /// </summary>
        /// <remarks>
        /// <para>基本的にこれを使用すればよい。</para>
        /// </remarks>
        /// <returns></returns>
        public IHttpUserAgent CreateUserAgent();
        /// <summary>
        /// <see cref="IHttpUserAgent"/>を名前付きで生成。
        /// </summary>
        /// <param name="name"><see cref="IHttpUserAgentName"/>で作成した名前を使用。</param>
        /// <returns></returns>
        public IHttpUserAgent CreateUserAgent(string name);

        #endregion
    }
}
