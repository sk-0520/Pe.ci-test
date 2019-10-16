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
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.CustomizeLauncherItem
{
    /// <summary>
    /// CustomizeLauncherItemWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizeLauncherItemWindow : Window
    {
        public CustomizeLauncherItemWindow()
        {
            InitializeComponent();
            ScrollTuner = new ScrollTuner(this, true);
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }
        ScrollTuner ScrollTuner { get; }

        CommandStore CommandStore { get; } = new CommandStore();

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
