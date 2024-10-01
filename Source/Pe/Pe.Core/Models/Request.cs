using System;
using System.Drawing;
using System.Reflection.Metadata;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Documents;
using System.Windows.Forms;
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

        /// <inheritdoc cref="Forms.TaskDialogPage.Caption"/>
        public string? Caption { get; set; }
        /// <inheritdoc cref="Forms.TaskDialogPage.Heading"/>
        public string? Heading { get; set; }
        /// <inheritdoc cref="Forms.TaskDialogPage.Message"/>
        public string? Message { get; set; }

        /// <inheritdoc cref="Forms.TaskDialogPage.Footnote"/>
        public Forms.TaskDialogFootnote? Footer { get; set; }

        /// <inheritdoc cref="Forms.TaskDialogPage.Buttons"/>
        public Forms.TaskDialogButtonCollection Buttons { get; set; } = new Forms.TaskDialogButtonCollection();
        /// <inheritdoc cref="Forms.TaskDialogPage.DefaultButton"/>
        public required Forms.TaskDialogButton DefaultButton { get; set; }

        /// <inheritdoc cref="Forms.TaskDialogPage.Icon"/>
        public required Forms.TaskDialogIcon Icon { get; set; }

        /// <inheritdoc cref="Forms.TaskDialogPage.Verification"/>
        public Forms.TaskDialogVerificationCheckBox? Verification { get; set; }

        #endregion
    }

    public static class CommonMessageDialogRequestParameterExtensions
    {
        #region function

        public static Forms.TaskDialogPage ToTaskDialogPage(this CommonMessageDialogRequestParameter parameter)
        {
            return new Forms.TaskDialogPage() {
                Caption = parameter.Caption,
                Heading = parameter.Heading,
                Text = parameter.Message,

                Footnote = parameter.Footer,

                Buttons = parameter.Buttons,
                DefaultButton = parameter.DefaultButton,

                Icon = parameter.Icon,

                Verification = parameter.Verification,

                SizeToContent = true,
                AllowCancel = parameter.Buttons.Count == 1 || parameter.Buttons.Contains(Forms.TaskDialogButton.Cancel),
            };
        }

        #endregion
    }

    public class CommonMessageDialogRequestResponse: RequestResponse
    {
        #region property

        /// <summary>
        /// 応答ボタン。
        /// </summary>
        public required Forms.TaskDialogButton Result { get; init; }
        /// <summary>
        /// チェック状態。
        /// </summary>
        /// <remarks><see langword="null"/>の場合はそもそもチェックUIが存在しない。</remarks>
        public bool? IsChecked { get; init; }

        #endregion
    }

    public class CancelResponse: RequestResponse
    {
        #region property

        public bool ResponseIsCancel { get; set; }

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
