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

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        #endregion

    }
}
