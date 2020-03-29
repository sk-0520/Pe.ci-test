using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.ViewModels.Command;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Command
{
    /// <summary>
    /// CommandWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CommandWindow : Window, IDpiScaleOutputor
    {
        public CommandWindow()
        {
            InitializeComponent();

            PopupAttacher = new PopupAttacher(this, this.popupItems);
        }

        #region property

        PopupAttacher PopupAttacher { get; }
        CommandStore CommandStore { get; } = new CommandStore();
        [Inject]
        ILogger? Logger { get; set; }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion

        #region function

        public ICommand ScrollSelectedItemCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                this.listItems.ScrollIntoView(this.listItems.SelectedItem);
            }
        ));

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if(e.Source == this.grip) {
                DragMove();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //this.popupItems.IsOpen = true;
            Dispatcher.BeginInvoke(new Action(() => {
                //WindowsUtility.ShowActiveForeground(HandleUtility.GetWindowHandle(this));
                this.inputCommand.Focus();
            }), System.Windows.Threading.DispatcherPriority.SystemIdle);
        }

        /*
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            //this.popupItems.IsOpen = false;
        }
        */

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            Logger.LogInformation("{0}", FocusManager.GetFocusedElement(this));
        }

        #endregion

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // https://social.msdn.microsoft.com/Forums/ja-JP/4f9083f6-a97c-454e-8cce-38be5714d851/listbox1239512461125401250812540124891250112457125401245912473123?forum=wpfja
            ((ListBoxItem)sender).IsSelected = true;
            this.inputCommand.Focus();
        }

        private void root_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // IsVisibleChanged が拾えない悲しみ
            if(DataContext is CommandViewModel viewModel) {
                viewModel.ViewIsVisibleChangedCommand.ExecuteIfCanExecute(this);
            }

            if(IsVisible) {
                WindowsUtility.ShowActiveForeground(HandleUtility.GetWindowHandle(this));
                this.inputCommand.Focus();
            }
        }
    }
}
