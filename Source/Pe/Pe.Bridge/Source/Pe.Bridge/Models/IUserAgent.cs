using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// <see cref="HttpClient"/>を意識せずに(寿命とか)に <see cref="IDisposable.Dispose"/> できる子。
    /// </summary>
    public interface IUserAgent : IDisposable
    {
        #region function

        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        #endregion
    }

    public static class IUserAgentExtensions
    {
        #region function

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
