using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public static class UserAgentName
    {
        #region define

        public static class Special
        {
            #region property

            public static string Separator => ":";
            public static string Session => "session";
            public static string Cache => "cache";

            #endregion
        }

        #endregion

        #region property

        public static string SharedSession => Special.Separator + Special.Session;


        #endregion
    }

    /// <summary>
    /// <see cref="HttpClient"/>を意識せずに(寿命とか)に <see cref="IDisposable.Dispose"/> できる子。
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
    /// <see cref="IUserAgent"/>生成処理。
    /// <para>生成したやつは <see cref="IDisposable.Dispose"/> すること。</para>
    /// HttpClientFactoryを使いたかったけど今の仕組みに乗せるのは難しそうな気がした(裏で使うかも)。
    /// </summary>
    public interface IUserAgentFactory
    {
        #region function

        public IUserAgent CreateUserAgent();
        public IUserAgent CreateUserAgent(string name);

        #endregion
    }

}
