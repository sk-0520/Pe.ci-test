using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Startup
{
    /// <summary>
    /// StartupWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StartupWindow: Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        #region property

        [DiInjection]
        private ILogger? Logger { get; set; }

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        #endregion

        private void root_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsVisibleChanged -= root_IsVisibleChanged;

            Dispatcher.BeginInvoke(new Action(() => {
                Activate();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
    }
}
