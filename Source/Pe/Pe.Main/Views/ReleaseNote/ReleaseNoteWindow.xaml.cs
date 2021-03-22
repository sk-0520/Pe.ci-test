using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.ReleaseNote
{
    /// <summary>
    /// ReleaseNoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReleaseNoteWindow: Window
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
