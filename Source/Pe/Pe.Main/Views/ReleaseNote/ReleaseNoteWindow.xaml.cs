using System;
using System.Collections.Generic;
using System.Text;
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

namespace ContentTypeTextNet.Pe.Main.Views.ReleaseNote
{
    /// <summary>
    /// ReleaseNoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReleaseNoteWindow : Window
    {
        public ReleaseNoteWindow()
        {
            InitializeComponent();
        }

        #region property

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command
        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));
        #endregion

    }
}
