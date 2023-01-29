using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public interface IRequestSender
    {
        #region event

        public event EventHandler<RequestEventArgs>? Raised;

        #endregion

        #region function

        public void Send();
        public void Send(RequestParameter requestParameter);
        public void Send(RequestParameter requestParameter, Action<RequestResponse> callback);

        public Task<RequestResponse> SendAsync(IDispatcherWrapper dispatcherWrapper);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper, CancellationToken token);

        #endregion
    }

    public class RequestSender: BindableBase, IRequestSender
    {
        public RequestSender()
        { }


        #region property

        private static RequestParameter EmptyParameter { get; } = new RequestParameter();

        #endregion

        #region function

        static void EmptyCallback(RequestResponse requestResponse)
        { }

        private void OnRaised(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            Raised?.Invoke(this, new RequestEventArgs(requestParameter, callback));
        }

        #endregion

        #region IRequestSender

        public event EventHandler<RequestEventArgs>? Raised;

        public void Send() => Send(EmptyParameter);
        public void Send(RequestParameter requestParameter) => Send(requestParameter, EmptyCallback);
        public void Send(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            OnRaised(requestParameter, callback);
        }

        public Task<RequestResponse> SendAsync(IDispatcherWrapper dispatcherWrapper) => SendAsync(EmptyParameter, dispatcherWrapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper) => SendAsync(requestParameter, dispatcherWrapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper, CancellationToken token)
        {
            var waitEvent = new ManualResetEventSlim(false);

            RequestResponse? result = null;
            void CustomCallback(RequestResponse requestResponse)
            {
                result = requestResponse;
                waitEvent!.Set();
            }

            return Task.Run(() => {
                using(waitEvent) {
                    dispatcherWrapper.BeginAsync(() => OnRaised(requestParameter, CustomCallback));
                    waitEvent.Wait(token);
                }
                return result ?? new RequestSilentResponse();
            });
        }

        #endregion
    }

    public static class IRequestSenderExtensions
    {
        #region function

        public static void Send<TRequestResponse>(this IRequestSender requestSender, RequestParameter requestParameter, Action<TRequestResponse> callback)
            where TRequestResponse : RequestResponse
        {
            requestSender.Send(requestParameter, r => {
                var response = (TRequestResponse)r;
                callback(response);
            });
        }

        public static TResult Send<TRequestResponse, TResult>(this IRequestSender requestSender, RequestParameter requestParameter, Func<TRequestResponse, TResult> callback)
            where TRequestResponse : RequestResponse
        {
            TResult result = default!;

            requestSender.Send(requestParameter, r => {
                var response = (TRequestResponse)r;
                result = callback(response);
            });

            return result;
        }

        #endregion
    }
}
