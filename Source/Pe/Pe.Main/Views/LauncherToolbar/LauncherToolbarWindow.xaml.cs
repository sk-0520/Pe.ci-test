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
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherToolbar
{
    /// <summary>
    /// LauncherToolbarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherToolbarWindow : Window
    {
        public LauncherToolbarWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }
        LauncherToolbarViewModel ViewModel => (LauncherToolbarViewModel)DataContext;

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        ICommand? _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return this._CloseCommand ?? (this._CloseCommand = new DelegateCommand<RequestEventArgs>(
                    o => {
                        Close();
                    }
                ));
            }
        }

        public ICommand OpenCommonMessageDialogCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                var result = MessageBox.Show(this, parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
                var response = new YesNoResponse();
                switch(result) {
                    case MessageBoxResult.Yes:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = true;
                        break;

                    case MessageBoxResult.No:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = false;
                        break;

                    default:
                        response.ResponseIsCancel = true;
                        response.ResponseIsYes = false;
                        break;
                }

                o.Callback(response);
            }
        ));

        #endregion

        #region function

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
        }

        #endregion
    }
}
