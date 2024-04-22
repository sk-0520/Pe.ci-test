using System.Windows;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Manager
{
    /// <summary>
    /// 通知領域の表示用。
    /// </summary>
    public interface INotifyArea
    {
        #region property

        /// <summary>
        /// 通知領域 メニューヘッダ。
        /// </summary>
        string MenuHeader { get; }
        /// <summary>
        /// 通知領域 メニュー アクセスキーを持つか。
        /// </summary>
        bool MenuHeaderHasAccessKey { get; }
        /// <summary>
        /// 通知領域 メニュー キー。
        /// </summary>
        KeyGesture? MenuKeyGesture { get; }
        /// <summary>
        /// 通知領域 メニューアイコン。
        /// </summary>
        DependencyObject? MenuIcon { get; }
        /// <summary>
        /// 通知領域 メニューアイコンが有効か。
        /// </summary>
        /// <remarks>
        /// <para>これが真の場合にのみ <see cref="MenuIcon"/> が使用される。</para>
        /// </remarks>
        bool MenuHasIcon { get; }
        /// <summary>
        /// 通知領域 メニュー 有効状態。
        /// </summary>
        bool MenuIsEnabled { get; }
        /// <summary>
        /// 通知領域 メニュー チェック状態。
        /// </summary>
        bool MenuIsChecked { get; }

        /// <summary>
        /// 通知領域 メニュー 選択
        /// </summary>
        ICommand MenuCommand { get; }

        #endregion
    }
}
