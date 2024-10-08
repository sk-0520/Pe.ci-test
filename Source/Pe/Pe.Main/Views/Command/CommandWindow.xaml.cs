using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.ViewModels.Command;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Command
{
    /// <summary>
    /// CommandWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CommandWindow: Window, IDpiScaleOutpour
    {
        public CommandWindow()
        {
            InitializeComponent();

            PopupAdjuster = new PopupAdjuster(this, this.popupItems);
        }

        #region property

        private PopupAdjuster PopupAdjuster { get; }
        [DiInjection]
        private ILogger? Logger { get; set; }

        #endregion

        #region command

        private ICommand? _ScrollSelectedItemCommand;
        public ICommand ScrollSelectedItemCommand => this._ScrollSelectedItemCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                this.listItems.ScrollIntoView(this.listItems.SelectedItem);
            }
        );

        private ICommand? _FocusEndCommand;
        public ICommand FocusEndCommand => this._FocusEndCommand ??= new DelegateCommand(
            () => {
                this.inputCommand.Select(this.inputCommand.Text.Length, 0);
            }
        );

        #endregion

        #region function

        #endregion

        #region IsExtendProperty

        public static readonly DependencyProperty IsExtendProperty = DependencyProperty.Register(
            nameof(IsExtend),
            typeof(bool),
            typeof(CommandWindow),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnIsExtendChanged)
            )
        );

        public bool IsExtend
        {
            get { return (bool)GetValue(IsExtendProperty); }
            set { SetValue(IsExtendProperty, value); }
        }

        static void OnIsExtendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CommandWindow;
            if(ctrl != null) {
                ctrl.IsExtend = (bool)e.NewValue;
            }
        }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);

#if DEBUG || BETA
            var devElement = new System.Windows.Controls.Border() {
                Background = new SolidColorBrush(Color.FromArgb(0x60, 0xff, 0xff, 0xff)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(1),
                Padding = new Thickness(1, 0, 1, 0),
                IsHitTestVisible = false,
                Child = new System.Windows.Controls.TextBlock() {
                    Text = Models.BuildStatus.BuildType.ToString(),
                    Opacity = 0.9,
                    FontSize = 7,
                },
            };

            var grid = UIUtility.FindLogicalChildren<Grid>(this).First();
            Grid.SetColumn(devElement, 2);
            grid.Children.Add(devElement);
#endif
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

            IsExtend = Keyboard.Modifiers == ModifierKeys.Shift;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            IsExtend = Keyboard.Modifiers == ModifierKeys.Shift;
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
                InputMethod.SetPreferredImeState(this.inputCommand, InputMethodState.Off);
                this.inputCommand.Focus();
            }
        }
    }
}
