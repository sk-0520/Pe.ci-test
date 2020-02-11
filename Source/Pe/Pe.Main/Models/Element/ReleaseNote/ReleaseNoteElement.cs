using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement : ElementBase
    {
        public ReleaseNoteElement(UpdateInfo updateInfo, IReadOnlyUpdateItemData updateItem, ReleaseNoteItemData releaseNoteItem, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UpdateInfoImpl = updateInfo;
            UpdateItem = updateItem;
            ReleaseNoteItem = releaseNoteItem;
        }

        #region property
        UpdateInfo UpdateInfoImpl { get; }
        public IReadOnlyUpdateInfo UpdateInfo => UpdateInfoImpl;

        public IReadOnlyUpdateItemData UpdateItem { get; }
        public ReleaseNoteItemData ReleaseNoteItem { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
