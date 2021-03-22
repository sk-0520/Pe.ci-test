using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public interface IDialogCommand
    {
        #region command

        /// <summary>
        /// 肯定。
        /// </summary>
        ICommand AffirmativeCommand { get; }
        /// <summary>
        /// 否定。
        /// </summary>
        ICommand NegativeCommand { get; }

        #endregion
    }
}
