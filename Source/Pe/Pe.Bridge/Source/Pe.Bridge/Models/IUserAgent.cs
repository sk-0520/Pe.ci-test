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
    public interface IUserAgent : IDisposable
    {
        #region function

        Task<HttpResponseMessage> GetAsync(Uri requestUri);
        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);

        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        #endregion
    }

    public static class IUserAgentExtensions
    {
        #region function

        public static Task<string> GetStringAsync(this IUserAgent @this, Uri requestUri) => GetStringAsync(@this, requestUri, CancellationToken.None);
        public static Task<string> GetStringAsync(this IUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStringAsync();
            }, cancellationToken).Unwrap();
        }

        public static Task<Stream> GetStreamAsync(this IUserAgent @this, Uri requestUri) => GetStreamAsync(@this, requestUri, CancellationToken.None);
        public static Task<Stream> GetStreamAsync(this IUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
        {
            return @this.GetAsync(requestUri, cancellationToken).ContinueWith(t => {
                cancellationToken.ThrowIfCancellationRequested();
                return t.Result.Content.ReadAsStreamAsync();
            }, cancellationToken).Unwrap();
        }

        public static Task<byte[]> GetByteArrayAsync(this IUserAgent @this, Uri requestUri) => GetByteArrayAsync(@this, requestUri, CancellationToken.None);
        public static Task<byte[]> GetByteArrayAsync(this IUserAgent @this, Uri requestUri, CancellationToken cancellationToken)
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
    public interface IUserAgentName
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
    /// <see cref="IUserAgent"/>生成処理。
    /// <para>生成したやつは <see cref="IDisposable.Dispose"/> すること。</para>
    /// HttpClientFactoryを使いたかったけど今の仕組みに乗せるのは難しそうな気がした(裏で使うかも)。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IUserAgentFactory
    {
        #region property

        IUserAgentName UserAgentName { get; }

        #endregion

        #region function

        public IUserAgent CreateUserAgent();
        public IUserAgent CreateUserAgent(string name);

        #endregion
    }

}
