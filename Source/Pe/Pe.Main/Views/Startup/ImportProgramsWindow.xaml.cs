using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Core.Views;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.Views.Startup
{
    /// <summary>
    /// ImportProgramsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImportProgramsWindow : Window
    {
        public ImportProgramsWindow()
        {
            InitializeComponent();
        }

        #region property

        [Inject]
        ILogger? Logger { get; set; }

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
