using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows;
using Forms = System.Windows.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Views
{
    public class DialogRequestReceiver
    {
        public DialogRequestReceiver(FrameworkElement view)
        {
            View = view;

            View = view;
            if(View.IsLoaded) {
                OwnerWindow = Window.GetWindow(view);
                View.Unloaded += View_Unloaded;
            } else {
                View.Loaded += View_Loaded;
            }
        }

        #region property

        private FrameworkElement? View { get; set; }
        private Window? OwnerWindow { get; set; }

        #endregion

        #region function

        [Obsolete("これはよくない")]
        public void ReceiveCommonMessageDialogRequest(RequestEventArgs o)
        {
            if(OwnerWindow == null) {
                throw new InvalidOperationException();
            }

            var messageParameter = (CommonMessageDialogRequestParameter)o.Parameter;
            Forms.TaskDialog.ShowDialog(
                HandleUtility.GetWindowHandle(Window.GetWindow(OwnerWindow)),
                messageParameter.ToTaskDialogPage(),
                Forms.TaskDialogStartupLocation.CenterOwner
            );
        }

        public void ReceiveFileSystemSelectDialogRequest(RequestEventArgs o)
        {
            if(OwnerWindow == null) {
                throw new InvalidOperationException();
            }

            var fileSelectParameter = (FileSystemSelectDialogRequestParameter)o.Parameter;
            FileSystemDialogBase dialog = fileSelectParameter.FileSystemDialogMode switch {
                FileSystemDialogMode.FileOpen => new OpenFileDialog(),
                FileSystemDialogMode.FileSave => new SaveFileDialog(),
                FileSystemDialogMode.Directory => new FolderBrowserDialog(),
                _ => throw new NotImplementedException(),
            };
            using(dialog) {
                dialog.FileName = fileSelectParameter.FilePath;
                dialog.Filters.SetRange(fileSelectParameter.Filter);

                if(dialog.ShowDialog(OwnerWindow).GetValueOrDefault()) {
                    o.Callback(new FileSystemSelectDialogRequestResponse() {
                        ResponseIsCancel = false,
                        ResponseFilePaths = new[] { dialog.FileName },
                    });
                } else {
                    o.Callback(new FileSystemSelectDialogRequestResponse() {
                        ResponseIsCancel = true,
                    });
                }
            }
        }

        public void ReceiveIconSelectDialogRequest(RequestEventArgs o)
        {
            var iconSelectParameter = (IconSelectDialogRequestParameter)o.Parameter;
            var dialog = new IconDialog {
                IconPath = iconSelectParameter.FileName,
                IconIndex = iconSelectParameter.IconIndex
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                o.Callback(new IconSelectDialogRequestResponse() {
                    ResponseIsCancel = false,
                    FileName = dialog.IconPath,
                    IconIndex = dialog.IconIndex,
                });
            } else {
                o.Callback(new IconSelectDialogRequestResponse() {
                    ResponseIsCancel = true,
                });
            }
        }

        #endregion

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(View != null);

            View.Loaded -= View_Loaded;
            View.Unloaded += View_Unloaded;
            OwnerWindow = Window.GetWindow(View);
        }

        private void View_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(View != null);

            View.Unloaded -= View_Unloaded;
            View = null;
            OwnerWindow = null;
        }
    }
}
