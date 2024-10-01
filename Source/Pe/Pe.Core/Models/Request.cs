using System;
using System.Windows;
using Forms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class RequestParameter
    { }
    public class RequestResponse
    { }

    public sealed class RequestSilentResponse: RequestResponse
    { }

    public class CommonMessageDialogRequestParameter: RequestParameter
    {
        #region property

        public string? Caption { get; set; }
        public string? Heading { get; set; }
        public string? Message { get; set; }
        public string? Footer { get; set; }

        public Forms.TaskDialogButtonCollection Buttons { get; set; } = new Forms.TaskDialogButtonCollection();
        public required Forms.TaskDialogButton DefaultButton { get; set; }

        public required Forms.TaskDialogIcon Icon { get; set; }

        public Forms.TaskDialogVerificationCheckBox? Verification { get; set; }

        #endregion
    }

    public class CommonMessageDialogRequestResponse: RequestResponse
    {
        #region property

        public required Forms.TaskDialogButton Result { get; init; }
        public bool? IsChecked { get; init; }

        #endregion
    }

    public class CancelResponse: RequestResponse
    {
        #region property

        public bool ResponseIsCancel { get; set; }

        #endregion
    }

    public class YesNoResponse: CancelResponse
    {
        #region property

        public bool ResponseIsYes { get; set; }

        #endregion
    }

    public class FileDialogRequestParameter: RequestParameter
    {
        public DialogFilterList Filter { get; } = new DialogFilterList();
        public bool ShowExtensions { get; set; } = true;
        public string FilePath { get; set; } = string.Empty;
    }

    public class FileDialogRequestResponse: CancelResponse
    {
        public string[] ResponseFilePaths { get; set; } = Array.Empty<string>();
    }

    public class FileOpenDialogRequestParameter: FileDialogRequestParameter
    {
        #region property


        #endregion
    }
    public class FileOpenDialogRequestResponse: FileDialogRequestResponse
    {
        #region property


        #endregion
    }

    public class FileSaveDialogRequestParameter: FileDialogRequestParameter
    {
        #region property


        #endregion
    }
    public class FileSaveDialogRequestResponse: FileDialogRequestResponse
    {
        #region property


        #endregion
    }

    public class RequestEventArgs: EventArgs
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
