using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
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

        [DiInjection]
        private ILogger? Logger { get; set; }

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand<RequestEventArgs>(
            o => Close()
        );

        #endregion
    }
}
