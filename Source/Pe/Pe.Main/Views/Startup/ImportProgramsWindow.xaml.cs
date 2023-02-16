using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Startup
{
    /// <summary>
    /// ImportProgramsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImportProgramsWindow: Window
    {
        public ImportProgramsWindow()
        {
            InitializeComponent();
        }

        #region property

        [Inject]
        private ILogger? Logger { get; set; }

        #endregion

        #region command

        ICommand? _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(this._closeCommand == null) {
                    this._closeCommand = new DelegateCommand<RequestEventArgs>(
                        o => Close()
                    );
                }

                return this._closeCommand;
            }
        }

        #endregion
    }
}
