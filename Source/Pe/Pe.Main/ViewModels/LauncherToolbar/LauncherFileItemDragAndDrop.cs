using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherFileItemDragAndDrop: DragAndDropGuidelineBase
    {
        public LauncherFileItemDragAndDrop(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        { }

        #region property

        public LauncherToolbarShortcutDropMode ShortcutDropMode { get; init; } = LauncherToolbarShortcutDropMode.Confirm;

        #endregion

        #region function

        public IResultSuccess<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e) => Result.CreateFailure<DragParameter>();

        public bool CanDragStart(UIElement sender, MouseEventArgs e) => false;

        public void DragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                // ファイル登録準備
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                } else {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            } else {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        public async Task DropAsync(UIElement sender, DragEventArgs e, Action<string> action, CancellationToken cancellationToken)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    await DispatcherWrapper.BeginAsync(() => action(filePaths[0]));
                    e.Handled = true;
                }
            }
        }

        public void DragLeave(UIElement sender, DragEventArgs e)
        { }

        public void RegisterDropFile(IRequestSender requestSender, string path, Action<string, bool> register)
        {
            if(PathUtility.IsShortcut(path)) {
                if(ShortcutDropMode == LauncherToolbarShortcutDropMode.Confirm) {
                    var request = new CommonMessageDialogRequestParameter() {
                        Message = Properties.Resources.String_LauncherFileItemDragAndDrop_Shortcut_Message,
                        Caption = Properties.Resources.String_LauncherFileItemDragAndDrop_Shortcut_Caption,
                        Buttons = [
                            Forms.TaskDialogButton.Yes,
                            Forms.TaskDialogButton.No,
                            Forms.TaskDialogButton.Cancel,
                        ],
                        DefaultButton = Forms.TaskDialogButton.Yes,
                        Icon = Forms.TaskDialogIcon.Information, // Question がねぇ！
                    };
                    requestSender.Send<CommonMessageDialogRequestResponse>(request, r => {
                        if(r.Result == Forms.TaskDialogButton.Cancel) {
                            Logger.LogTrace("ショートカット登録取り消し");
                            return;
                        }
                        register(path, r.Result == Forms.TaskDialogButton.Yes);
                    });
                } else {
                    var registerTarget = ShortcutDropMode switch {
                        LauncherToolbarShortcutDropMode.Target => true,
                        _ => false,
                    };
                    register(path, registerTarget);
                }
            } else {
                register(path, false);
            }
        }

        #endregion
    }
}
