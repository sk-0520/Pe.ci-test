using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        FrameworkElement? View { get; set; }
        Window? OwnerWindow { get; set; }

        #endregion

        #region function

        public void ReceiveFileSystemSelectDialogRequest(RequestEventArgs o)
        {
            if(OwnerWindow == null) {
                throw new InvalidOperationException();
            }

            var fileSelectParameter = (FileSystemSelectDialogRequestParameter)o.Parameter;
            FileSystemDialogBase dialog = fileSelectParameter.FileSystemDialogMode switch
            {
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
            var dialog = new IconDialog();
            dialog.IconPath = iconSelectParameter.FileName;
            dialog.IconIndex = iconSelectParameter.IconIndex;
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
