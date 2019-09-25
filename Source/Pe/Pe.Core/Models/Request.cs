using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Views;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class RequestParameter
    { }
    public class RequestResponse
    { }

    public sealed class RequestSilentResponse : RequestResponse
    { }

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
        public bool ShowExtensions { get; set; } = true;
    }

    public class FileDialogRequestResponse : CancelResponse
    {
        public string[]? ResponseFilePaths { get; set; }
    }

    public class FileOpenDialogRequestParameter : FileDialogRequestParameter
    {
        #region property


        #endregion
    }
    public class FileOpenDialogRequestResponse : FileDialogRequestResponse
    {
        #region property


        #endregion
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

    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            Parameter = requestParameter;
            Callback = callback;
        }

        #region property

        public RequestParameter Parameter { get; }

        public Action<RequestResponse> Callback { get; }

        #endregion
    }

}
