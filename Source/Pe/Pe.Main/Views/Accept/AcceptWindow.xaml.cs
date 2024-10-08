using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Accept
{
    /// <summary>
    /// AcceptWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AcceptWindow: Window
    {
        public AcceptWindow()
        {
            InitializeComponent();
        }

        #region property

        [DiInjection]
        private ILogger? Logger { get; set; }

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(this._CloseCommand == null) {
                    this._CloseCommand = new DelegateCommand<RequestEventArgs>(
                        o => Close()
                    );
                }

                return this._CloseCommand;
            }
        }

        #endregion

        // 以下はコマンド（リンク）をコピーしたときに死ぬので一応の適当対応

        private void documentAccept_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void documentAccept_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C || e.Key == Key.Insert) {
                e.Handled = true;
            }
        }
    }
}
