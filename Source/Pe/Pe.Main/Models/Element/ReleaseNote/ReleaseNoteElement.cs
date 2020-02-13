using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement : ElementBase
    {
        public ReleaseNoteElement(UpdateInfo updateInfo, IReadOnlyUpdateItemData updateItem, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UpdateInfoImpl = updateInfo;
            UpdateItem = updateItem;
        }

        #region property
        UpdateInfo UpdateInfoImpl { get; }
        public IReadOnlyUpdateInfo UpdateInfo => UpdateInfoImpl;

        public IReadOnlyUpdateItemData UpdateItem { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
