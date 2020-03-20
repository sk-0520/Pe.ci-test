using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Views.Feedback;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Feedback
{
    public class FeedbackViewModel : ElementViewModelBase<FeedbackElement>, IViewLifecycleReceiver
    {
        public FeedbackViewModel(FeedbackElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        #endregion

        #region command

        public ICommand SendCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SendAync();
            }
        ));

        #endregion

        #region function

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            var view = (FeedbackWindow)window;
            view.webView.LifeSpanHandler = new PlatformLifeSpanHandler(LoggerFactory);
            view.webView.RequestHandler = new PlatformRequestHandler(LoggerFactory);
            view.webView.MenuHandler = new DisableContextMenuHandler();

            Model.LoadHtmlSourceAsync().ContinueWith(t => {
                var htmlSource = t.Result;
                view.webView.LoadHtml(htmlSource, "http://localhost/" + nameof(FeedbackViewModel));
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        { }


        public void ReceiveViewClosing(CancelEventArgs e)
        { }

        public void ReceiveViewClosed()
        { }

        #endregion
    }
}
