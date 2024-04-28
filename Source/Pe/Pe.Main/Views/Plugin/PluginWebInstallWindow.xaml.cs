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
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Plugin
{
    /// <summary>
    /// PluginWebInstallWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PluginWebInstallWindow: Window
    {
        public PluginWebInstallWindow()
        {
            InitializeComponent();
        }

        #region property

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        #endregion
    }
}
