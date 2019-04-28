using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class CancelNotification : Notification
    {
        #region property

        public bool ResponseIsCancel { get; set; }

        #endregion
    }

    public class YesNoNotification: CancelNotification
    {
        #region property

        public bool ResponseIsYes { get; set; }

        #endregion
    }

    public class FileSaveDialogNotification: CancelNotification
    {
        #region property
        #endregion
    }
}
