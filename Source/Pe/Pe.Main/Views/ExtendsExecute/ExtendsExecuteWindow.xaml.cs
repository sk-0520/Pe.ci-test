using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.ExtendsExecute
{
    /// <summary>
    /// ExtendsExecuteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ExtendsExecuteWindow: Window, IDpiScaleOutputor
    {
        public ExtendsExecuteWindow()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }


        #region property
        private DialogRequestReceiver DialogRequestReceiver { get; }

        [Inject]
        private ILogger? Logger { get; set; }

        private CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));

        public ICommand FileSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        ));

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion


    }
}
