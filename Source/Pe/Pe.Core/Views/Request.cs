using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public class RequestParameter
    { }
    public class RequestResponse
    { }

    public sealed class RequestSilentResponse : RequestResponse
    { }

    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            Parameter = requestParameter;
            Callback = callback;
        }

        #region property

        public RequestParameter Parameter { get; }

        public Action<RequestResponse> Callback { get; }

        #endregion
    }

    public class RequestSender
    {
        #region event

        public event EventHandler<RequestEventArgs> Raised;

        #endregion

        public RequestSender(IDispatcherWapper dispatcherWapper)
        {
            DispatcherWapper = dispatcherWapper;
        }


        #region property
        static RequestParameter EmptyParameter { get; } = new RequestParameter();

        IDispatcherWapper DispatcherWapper { get; }

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

        public Task<RequestResponse> SendAsync() => SendAsync(EmptyParameter, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter) => SendAsync(requestParameter, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, CancellationToken token)
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
                    DispatcherWapper.Begin(() => OnRaised(requestParameter, CustomCallback));
                    waitEvent.Wait(token);
                }
                return result ?? new RequestSilentResponse();
            });
        }


        #endregion
    }

    public class RequestTrigger : System.Windows.Interactivity.EventTrigger
    {
        public RequestTrigger()
        { }

        public RequestTrigger(string eventName)
            : base(eventName)
        { }

        #region property
        #endregion

        #region TriggerAction

        protected override string GetEventName()
        {
            return nameof(RequestSender.Raised);
        }

        #endregion
    }

}
