using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Feedback
{
    /// <summary>
    /// FeedbackWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FeedbackWindow: Window
    {
        public FeedbackWindow()
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
