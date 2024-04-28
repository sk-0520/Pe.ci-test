using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherToolbar
{
    /// <summary>
    /// LauncherToolbarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherToolbarWindow: Window
    {
        public LauncherToolbarWindow()
        {
            InitializeComponent();
        }

        #region property

        [Inject]
        private ILogger? Logger { get; set; }
        private LauncherToolbarViewModel ViewModel => (LauncherToolbarViewModel)DataContext;

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                Close();
            }
        );

        private ICommand? _OpenCommonMessageDialogCommand;
        public ICommand OpenCommonMessageDialogCommand => this._OpenCommonMessageDialogCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                var result = MessageBox.Show(this, parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
                var response = new YesNoResponse();
                switch(result) {
                    case MessageBoxResult.Yes:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = true;
                        break;

                    case MessageBoxResult.No:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = false;
                        break;

                    default:
                        response.ResponseIsCancel = true;
                        response.ResponseIsYes = false;
                        break;
                }

                o.Callback(response);
            }
        );

        #endregion

        #region function

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);

            var hWnd = HandleUtility.GetWindowHandle(this);
            var hSysMenu = NativeMethods.GetSystemMenu(hWnd, false);
            if(hSysMenu != IntPtr.Zero) {
                NativeMethods.RemoveMenu(hSysMenu, (int)SC.SC_SIZE, MF.MF_BYCOMMAND);
                NativeMethods.RemoveMenu(hSysMenu, (int)SC.SC_MOVE, MF.MF_BYCOMMAND);
            } else {
                Logger!.LogWarning("hSysMenu is IntPtr.Zero");
            }
        }

        #endregion


        private void LauncherContentControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(!ViewModel.IsVerticalLayout) {
                this.scrollViewer.ScrollToHorizontalOffset(this.scrollViewer.HorizontalOffset + -e.Delta);
                e.Handled = true;
            }
        }
    }
}
