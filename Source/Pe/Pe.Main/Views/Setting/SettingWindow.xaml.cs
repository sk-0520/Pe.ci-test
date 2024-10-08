using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow: Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            ScrollAdjuster = new ScrollAdjuster(this);
        }

        #region property

        [DiInjection]
        private ILogger? Logger { get; set; }
        private ScrollAdjuster ScrollAdjuster { get; }

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => {
                Close();
            }
        );

        #endregion

        #region function
        #endregion
    }
}
