using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Views;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class CancelResponse: RequestResponse
    {
        #region property

        public bool ResponseIsCancel { get; set; }

        #endregion
    }

    public class YesNoNotification: CancelResponse
    {
        #region property

        public bool ResponseIsYes { get; set; }

        #endregion
    }

    public class FileDialogRequestParameter : RequestParameter
    {
        public DialogFilterList Filter { get; } = new DialogFilterList();
        public bool ShowExtensions { get; set; }
    }

    public class FileDialogRequestResponse : CancelResponse
    {
        public string[]? ResponseFilePaths { get; set; }
    }

    public class FileSaveDialogRequestParameter : FileDialogRequestParameter
    {
        #region property


        #endregion
    }
    public class FileSaveDialogRequestResponse : FileDialogRequestResponse
    {
        #region property


        #endregion
    }
}
