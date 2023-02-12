using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteFileViewModel: ElementViewModelBase<NoteFileElement>
    {
        public NoteFileViewModel(NoteFileElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            IconViewer = new IconViewerViewModel(Model.IconImageLoader, DispatcherWrapper, LoggerFactory);
        }

        #region property

        public string FilePath => Model.NoteFilePath;

        public IconViewerViewModel IconViewer { get; set; }

        #endregion
    }
}
