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

    public class CommonMessageDialogRequestParameter : RequestParameter
    {
        #region property

        public string Message { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;

        #endregion
    }

    public class CancelResponse : RequestResponse
    {
        #region property

        public bool ResponseIsCancel { get; set; }

        #endregion
    }

    public class YesNoResponse : CancelResponse
    {
        #region property

        public bool ResponseIsYes { get; set; }

        #endregion
    }

    public class FileDialogRequestParameter : RequestParameter
    {
        public DialogFilterList Filter { get; } = new DialogFilterList();
        public bool ShowExtensions { get; set; } = true;

        public string FilePath { get; set; } = string.Empty;
    }

    public class FileDialogRequestResponse : CancelResponse
    {
        public string[] ResponseFilePaths { get; set; } = new string[] { };
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
