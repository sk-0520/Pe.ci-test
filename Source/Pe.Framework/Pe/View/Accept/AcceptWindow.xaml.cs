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
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.View.Accept
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
        ILogger Logger { get; set; }

        #endregion

        #region command

        ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(this._closeCommand == null) {
                    this._closeCommand = new DelegateCommand<InteractionRequestedEventArgs>(
                        o => Close()
                    );
                }

                return this._closeCommand;
            }
        }

        #endregion
    }
}
