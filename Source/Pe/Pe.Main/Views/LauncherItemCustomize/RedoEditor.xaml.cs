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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// RedoEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class RedoEditor: UserControl
    {
        public RedoEditor()
        {
            InitializeComponent();
        }

        #region RedoWaitProperty

        public static readonly DependencyProperty RedoWaitProperty = DependencyProperty.Register(
            nameof(RedoWait),
            typeof(RedoWait),
            typeof(RedoEditor),
            new PropertyMetadata(
                RedoWait.None,
                OnRedoWaitChanged
            )
        );

        public RedoWait RedoWait
        {
            get { return (RedoWait)GetValue(RedoWaitProperty); }
            set { SetValue(RedoWaitProperty, value); }
        }

        private static void OnRedoWaitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.RedoWait = (RedoWait)e.NewValue;
            }
        }

        #endregion

        #region WaitTimeSecondsProperty

        public static readonly DependencyProperty WaitTimeSecondsProperty = DependencyProperty.Register(
            nameof(WaitTimeSeconds),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                0,
                OnWaitTimeSecondsChanged
            )
        );

        public int WaitTimeSeconds
        {
            get { return (int)GetValue(WaitTimeSecondsProperty); }
            set { SetValue(WaitTimeSecondsProperty, value); }
        }

        private static void OnWaitTimeSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.WaitTimeSeconds = (int)e.NewValue;
            }
        }

        #endregion

        #region MinimumWaitTimeSecondsProperty

        public static readonly DependencyProperty MinimumWaitTimeSecondsProperty = DependencyProperty.Register(
            nameof(MinimumWaitTimeSeconds),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                1,
                OnMinimumWaitTimeSecondsChanged
            )
        );

        public int MinimumWaitTimeSeconds
        {
            get { return (int)GetValue(MinimumWaitTimeSecondsProperty); }
            set { SetValue(MinimumWaitTimeSecondsProperty, value); }
        }

        private static void OnMinimumWaitTimeSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.MinimumWaitTimeSeconds = (int)e.NewValue;
            }
        }

        #endregion

        #region MaximumWaitTimeSecondsProperty

        public static readonly DependencyProperty MaximumWaitTimeSecondsProperty = DependencyProperty.Register(
            nameof(MaximumWaitTimeSeconds),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                300,
                OnMaximumWaitTimeSecondsChanged
            )
        );

        public int MaximumWaitTimeSeconds
        {
            get { return (int)GetValue(MaximumWaitTimeSecondsProperty); }
            set { SetValue(MaximumWaitTimeSecondsProperty, value); }
        }

        private static void OnMaximumWaitTimeSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.MaximumWaitTimeSeconds = (int)e.NewValue;
            }
        }

        #endregion

        #region RetryCountProperty

        public static readonly DependencyProperty RetryCountProperty = DependencyProperty.Register(
            nameof(RetryCount),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                0,
                OnRetryCountChanged
            )
        );

        public int RetryCount
        {
            get { return (int)GetValue(RetryCountProperty); }
            set { SetValue(RetryCountProperty, value); }
        }

        private static void OnRetryCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.RetryCount = (int)e.NewValue;
            }
        }

        #endregion

        #region MinimumRetryCountProperty

        public static readonly DependencyProperty MinimumRetryCountProperty = DependencyProperty.Register(
            nameof(MinimumRetryCount),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                1,
                OnMinimumRetryCountChanged
            )
        );

        public int MinimumRetryCount
        {
            get { return (int)GetValue(MinimumRetryCountProperty); }
            set { SetValue(MinimumRetryCountProperty, value); }
        }

        private static void OnMinimumRetryCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.MinimumRetryCount = (int)e.NewValue;
            }
        }

        #endregion

        #region MaximumRetryCountProperty

        public static readonly DependencyProperty MaximumRetryCountProperty = DependencyProperty.Register(
            nameof(MaximumRetryCount),
            typeof(int),
            typeof(RedoEditor),
            new PropertyMetadata(
                int.MaxValue,
                OnMaximumRetryCountChanged
            )
        );

        public int MaximumRetryCount
        {
            get { return (int)GetValue(MaximumRetryCountProperty); }
            set { SetValue(MaximumRetryCountProperty, value); }
        }

        private static void OnMaximumRetryCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.MaximumRetryCount = (int)e.NewValue;
            }
        }

        #endregion

        #region SuccessExitCodesProperty

        public static readonly DependencyProperty SuccessExitCodesProperty = DependencyProperty.Register(
            nameof(SuccessExitCodes),
            typeof(string),
            typeof(RedoEditor),
            new PropertyMetadata(
                string.Empty,
                OnSuccessExitCodesChanged
            )
        );

        public string SuccessExitCodes
        {
            get { return (string)GetValue(SuccessExitCodesProperty); }
            set { SetValue(SuccessExitCodesProperty, value); }
        }

        private static void OnSuccessExitCodesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RedoEditor control) {
                control.SuccessExitCodes = (string)e.NewValue;
            }
        }

        #endregion

    }
}
