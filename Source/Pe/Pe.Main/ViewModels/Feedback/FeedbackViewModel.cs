using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class FeedbackViewModel: ElementViewModelBase<FeedbackElement>, IViewLifecycleReceiver
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
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();
        public RunningStatusViewModel SendStatus { get; }
        public string ErrorMessage => Model.ErrorMessage;

        public TextDocument ContentDocument { get; } = new TextDocument();

        public IReadOnlyList<FeedbackKind> FeedbackKindItems { get; } = Enum.GetValues<FeedbackKind>().ToList();

        [Required]
        public string Subject
        {
            get => this._subject;
            set
            {
                SetProperty(ref this._subject, value);
                ValidateProperty(this._subject);
            }
        }

        public FeedbackKind SelectedFeedbackKind
        {
            get => this._selectedFeedbackKind;
            set => SetProperty(ref this._selectedFeedbackKind, value);
        }

        #endregion

        #region command

        public ICommand ShowSourceUriCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.ShowSourceUri();
             }
        ));

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
                if(Validate()) {
                    var data = new FeedbackInputData() {
                        Subject = Subject,
                        Kind = SelectedFeedbackKind,
                        Content = ContentDocument.Text,
                    };

                    await Model.SendAync(data);
                }
            }
        ));

        public ICommand CorrectCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.Cancel();
             }
        ));

        #endregion

        #region function

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }


        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        { }

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                SendStatus.PropertyChanged -= SendStatus_PropertyChanged;
                if(disposing) {
                    SendStatus.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion


        private void SendStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SendStatus.State)) {
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
    }
}
