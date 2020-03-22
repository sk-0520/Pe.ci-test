using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.Views.Feedback;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Feedback
{
    public class FeedbackViewModel : ElementViewModelBase<FeedbackElement>, IViewLifecycleReceiver
    {
        #region variable

        string _subject = string.Empty;
        FeedbackKind _selectedFeedbackKind;

        #endregion
        public FeedbackViewModel(FeedbackElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            SendStatus = new RunningStatusViewModel(Model.SendStatus, LoggerFactory);
            SendStatus.PropertyChanged += SendStatus_PropertyChanged;

            DelayUpdatePreview = new LazyAction(nameof(DelayUpdatePreview), TimeSpan.FromMilliseconds(500), LoggerFactory);

            ContentDocument.TextChanged += Document_TextChanged;
        }

        #region property

        LazyAction DelayUpdatePreview { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();
        public RunningStatusViewModel SendStatus { get; }
        public string ErrorMessage => Model.ErrorMessage;

        public TextDocument ContentDocument { get; } = new TextDocument();
        IWebBrowser? WebView { get; set; }

        public IReadOnlyList<FeedbackKind> FeedbackKindItems { get; } = EnumUtility.GetMembers<FeedbackKind>().ToList();

        public string Subject
        {
            get => this._subject;
            set => SetProperty(ref this._subject, value);
        }

        public FeedbackKind SelectedFeedbackKind
        {
            get => this._selectedFeedbackKind;
            set => SetProperty(ref this._selectedFeedbackKind, value);
        }

        #endregion

        #region command

        public ICommand SetTemplateCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var text = SelectedFeedbackKind switch
                {
                    FeedbackKind.Bug => Properties.Resources.String_Feedback_Comment_Kind_Bug,
                    FeedbackKind.Proposal => Properties.Resources.String_Feedback_Comment_Kind_Proposal,
                    FeedbackKind.Others => Properties.Resources.String_Feedback_Comment_Kind_Others,
                    _ => throw new NotImplementedException(),
                };
                ContentDocument.Text = text;
                DelayUpdatePreview.Flush();
            }
        ));

        //public ICommand SendCommand => GetOrCreateCommand(() => new DelegateCommand<IWebBrowser>(
        //    async (webView) => {
        //        var response = await webView.EvaluateScriptAsync("getInputValues()");
        //        if(response.Success && response.Result != null) {
        //            var json = response.Result.ToString();
        //            var options = new JsonSerializerOptions();
        //            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        //            var data = JsonSerializer.Deserialize<FeedbackInputData>(json, options);
        //            await Model.SendAync(data);
        //        }
        //    }
        //));

        public ICommand SendCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                var data = new FeedbackInputData() {
                    Subject = Subject,
                    Kind = SelectedFeedbackKind,
                    Content = ContentDocument.Text,
                };

                await Model.SendAync(data);
            }
         ));

        #endregion

        #region function

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            var view = (FeedbackWindow)window;
            WebView = view.webView;
            WebView.LifeSpanHandler = new PlatformLifeSpanHandler(LoggerFactory);
            WebView.RequestHandler = new PlatformRequestHandler(LoggerFactory);
            WebView.MenuHandler = new DisableContextMenuHandler();

            Model.LoadHtmlSourceAsync().ContinueWith(t => {
                var htmlSource = t.Result;
                WebView.LoadHtml(htmlSource, "http://localhost/" + nameof(FeedbackViewModel));
            });
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        { }


        public void ReceiveViewClosing(CancelEventArgs e)
        { }

        public void ReceiveViewClosed()
        { }

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                SendStatus.PropertyChanged -= SendStatus_PropertyChanged;
                ContentDocument.TextChanged -= Document_TextChanged;
                if(disposing) {
                    SendStatus.Dispose();
                    DelayUpdatePreview.Clear();
                    DelayUpdatePreview.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion


        private void SendStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SendStatus.State)) {
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        private void Document_TextChanged(object? sender, EventArgs e)
        {
            DelayUpdatePreview.DelayAction(() => {
                DispatcherWrapper.Begin(() => {
                    var text = ContentDocument.Text;
                    // TODO: エスケープ
                    WebView.EvaluateScriptAsync("updatePreview('" + text + "')");
                }, System.Windows.Threading.DispatcherPriority.Render);
            });
        }


    }
}
