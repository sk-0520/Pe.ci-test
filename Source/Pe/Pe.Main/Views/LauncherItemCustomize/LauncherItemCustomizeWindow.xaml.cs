using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// LauncherItemCustomizeWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemCustomizeWindow: Window
    {
        public LauncherItemCustomizeWindow()
        {
            InitializeComponent();
            ScrollTuner = new ScrollTuner(this);
        }

        #region property

        [Inject]
        private ILogger? Logger { get; set; }
        private ScrollTuner ScrollTuner { get; }

        private CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));

        #endregion

        #region function

        #endregion

        #region Window

        protected override void OnClosed(EventArgs e)
        {
            ScrollTuner.Dispose();

            base.OnClosed(e);
        }

        #endregion
    }
}
