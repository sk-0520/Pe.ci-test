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
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Accept
{
    /// <summary>
    /// AcceptWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AcceptWindow : Window
    {
        public AcceptWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }

        ICommand? _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(this._closeCommand == null) {
                    this._closeCommand = new DelegateCommand<RequestSender>(
                        o => Close()
                    );
                }

                return this._closeCommand;
            }
        }

        public MessageReceiver? CloseMessageReceiver2 { get; set; }
        public MessageReceiver CloseMessageReceiver
        {
            get { return (MessageReceiver)GetValue(CloseMessageReceiverProperty); }
            set { SetValue(CloseMessageReceiverProperty, value); }
        }

        public static readonly DependencyProperty CloseMessageReceiverProperty = DependencyProperty.Register(
            nameof(CloseMessageReceiver),
            typeof(MessageReceiver),
            typeof(AcceptWindow),
            new PropertyMetadata(default(MessageReceiver), OnChangedCloseMessageReceiverProperty)
        );

        private static void OnChangedCloseMessageReceiverProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (AcceptWindow)d;
            var prev = window.CloseMessageReceiver;
            if(prev != null) {

            }
            window.CloseMessageReceiver = (MessageReceiver)e.NewValue;
            if(window.CloseMessageReceiver != null) {

            }
        }



        #endregion
    }
}
