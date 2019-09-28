using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public class RequestSender
    {
        #region event

        public event EventHandler<RequestEventArgs>? Raised;

        #endregion

        public RequestSender()
        { }


        #region property

        static RequestParameter EmptyParameter { get; } = new RequestParameter();

        #endregion

        #region function

        static void EmptyCallback(RequestResponse requestResponse)
        { }

        void OnRaised(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            Raised!.Invoke(this, new RequestEventArgs(requestParameter, callback));
        }

        public void Send() => Send(EmptyParameter);
        public void Send(RequestParameter requestParameter) => Send(requestParameter, EmptyCallback);
        public void Send(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            OnRaised(requestParameter, callback);
        }

        public Task<RequestResponse> SendAsync(IDispatcherWapper dispatcherWapper) => SendAsync(EmptyParameter, dispatcherWapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWapper dispatcherWapper) => SendAsync(requestParameter, dispatcherWapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWapper dispatcherWapper, CancellationToken token)
        {
            var waitEvent = new ManualResetEventSlim(false);

            RequestResponse? result = null;
            void CustomCallback(RequestResponse requestResponse)
            {
                result = requestResponse;
                waitEvent.Set();
            };

            return Task.Run(() => {
                using(waitEvent) {
                    dispatcherWapper.Begin(() => OnRaised(requestParameter, CustomCallback));
                    waitEvent.Wait(token);
                }
                return result ?? new RequestSilentResponse();
            });
        }


        #endregion
    }
}
