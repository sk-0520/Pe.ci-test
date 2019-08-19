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

    public class FileDialogNotification : CancelNotification
    {
        public string[] ResponseFilePaths { get; set; }
        public DialogFilterList Filter { get; } = new DialogFilterList();
        public bool ShowExtensions { get; set; }
    }

    public class FileSaveDialogNotification: FileDialogNotification
    {
        #region property


        #endregion
    }
}
