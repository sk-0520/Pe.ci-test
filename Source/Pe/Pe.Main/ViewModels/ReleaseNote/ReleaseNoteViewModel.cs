using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.UsageStatistics;
using ContentTypeTextNet.Pe.Main.Views.ReleaseNote;
using Microsoft.Extensions.Logging;
using CefSharp;
using ContentTypeTextNet.Pe.Main.Models.WebView;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ReleaseNote
{
    public class ReleaseNoteViewModel : ElementViewModelBase<ReleaseNoteElement>, IViewLifecycleReceiver
    {
        public ReleaseNoteViewModel(ReleaseNoteElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        [Timestamp(DateTimeKind.Utc)]
        public DateTime Release => Model?.UpdateItem.Release ?? DateTime.UtcNow;
        public Version Version => Model?.UpdateItem.Version ?? new Version();
        public string Revision => Model?.UpdateItem.Revision ?? string.Empty;

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            var view = (ReleaseNoteWindow)window;
            view.webView.LifeSpanHandler = new PlatformLifeSpanHandler(LoggerFactory);
            view.webView.MenuHandler = new DisableContextMenuHandler();

            Model.LoadReleaseNoteDocumentAsync().ContinueWith(t => {
                if(IsDisposed) {
                    Logger.LogTrace("close");
                    return;
                }

                if(t.IsCompletedSuccessfully) {
                    var htmlSource = t.Result;
                    view.webView.LoadHtml(htmlSource, Model.UpdateItem.NoteUri.ToString());
                } else {
                    view.webView.LoadHtml(Properties.Resources.File_ReleaseNote_ErrorReleaseNote, nameof(Properties.Resources.File_ReleaseNote_ErrorReleaseNote));
                }
            });
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
