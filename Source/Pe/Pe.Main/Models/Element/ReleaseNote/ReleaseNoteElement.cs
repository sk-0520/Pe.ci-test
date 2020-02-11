using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement : ElementBase
    {
        public ReleaseNoteElement(UpdateItemData updateItem, ReleaseNoteItemData releaseNoteItem, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UpdateItem = updateItem;
            ReleaseNoteItem = releaseNoteItem;
        }

        #region property

        UpdateItemData UpdateItem { get; }
        ReleaseNoteItemData ReleaseNoteItem { get; }
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
