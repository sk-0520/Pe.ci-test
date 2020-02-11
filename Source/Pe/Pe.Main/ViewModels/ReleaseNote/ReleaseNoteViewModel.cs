using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.UsageStatistics;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ReleaseNote
{
    public class ReleaseNoteViewModel : ElementViewModelBase<ReleaseNoteElement>
    {
        public ReleaseNoteViewModel(ReleaseNoteElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            Item = new ReleaseNoteItemViewModel(Model.ReleaseNoteItem, LoggerFactory);
        }

        #region property

        [Timestamp(DateTimeKind.Utc)]
        public DateTime Release => Model.UpdateItem.Release;
        public Version Version => Model.UpdateItem.Version;
        public string Revision => Model.UpdateItem.Revision;

        public ReleaseNoteItemViewModel Item { get; }

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
