using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.StandardInputOutput
{
    /// <summary>
    /// StandardInputOutputWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StandardInputOutputWindow: Window
    {
        public StandardInputOutputWindow()
        {
            InitializeComponent();

            DialogRequestReceiver = new DialogRequestReceiver(this);
        }


        #region property

        [DiInjection]
        private ILogger? Logger { get; set; }
        private StandardInputOutputViewModel ViewModel => (StandardInputOutputViewModel)DataContext;

        private DialogRequestReceiver DialogRequestReceiver { get; }

        #endregion

        #region command

        private ICommand? _FileSelectCommand;
        public ICommand FileSelectCommand => this._FileSelectCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        #endregion
    }
}
