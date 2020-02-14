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
        public DateTime Release => Model.UpdateItem.Release;
        public Version Version => Model.UpdateItem.Version;
        public string Revision => Model.UpdateItem.Revision;

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
            //view.wevView.Address = Model.UpdateItem.NoteUri.ToString();
            //view.wevView.Address = "https://bitbucket.org/sk_0520/pe/downloads/update-release.html";
            Model.LoadReleaseNoteDocumentAsync().ContinueWith(t => {
                if(t.IsCompletedSuccessfully) {
                    var htmlSource = t.Result;
                    view.wevView.LoadHtml(htmlSource, Model.UpdateItem.NoteUri.ToString());
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
