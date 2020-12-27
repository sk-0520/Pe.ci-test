using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// <see cref="HttpClient"/>を意識せずに(寿命とか)に <see cref="IDisposable.Dispose"/> できる子。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHttpUserAgent : IDisposable
    {
        #region function

        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);

        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);

        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        #endregion
    }

    public static class IHttpUserAgentExtensions
    {
        #region function

        public static Task<string> GetStringAsync(this IHttpUserAgent @this, Uri requestUri) => GetStringAsync(@this, requestUri, CancellationToken.None);
        public static Task<string> GetStringAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStringAsync();
            }, cancellationToken).Unwrap();
        }

        public static Task<Stream> GetStreamAsync(this IHttpUserAgent @this, Uri requestUri) => GetStreamAsync(@this, requestUri, CancellationToken.None);
        public static Task<Stream> GetStreamAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStreamAsync();
            }, cancellationToken).Unwrap();
        }

        public static Task<byte[]> GetByteArrayAsync(this IHttpUserAgent @this, Uri requestUri) => GetByteArrayAsync(@this, requestUri, CancellationToken.None);
        public static Task<byte[]> GetByteArrayAsync(this IHttpUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsByteArrayAsync();
            }, cancellationToken).Unwrap();
        }

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
