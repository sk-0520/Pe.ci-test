using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement : ElementBase
    {
        public ReleaseNoteElement(UpdateItemData updateItem, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UpdateItem = updateItem;
        }

        #region property

        UpdateItemData UpdateItem { get; }

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
