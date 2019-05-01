using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Manager
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
        /// 通知領域 メニュー キー。
        /// <para>どうしてたっけなぁ。</para>
        /// </summary>
        string MenuKeys { get; }
        /// <summary>
        /// 通知領域 メニューアイコン。
        /// </summary>
        DependencyObject MenuIcon { get; }
        /// <summary>
        /// 通知領域 メニューアイコンが有効か。
        /// <para>これが真の場合にのみ <see cref="MenuIcon"/> が使用される。</para>
        /// </summary>
        bool MenuHasIcon {get;}
        /// <summary>
        /// 通知領域 メニュー 有効状態。
        /// </summary>
        bool MenuIsEnabled { get; }
        /// <summary>
        /// 通知領域 メニュー チェック状態。
        /// </summary>
        bool MenuIsChecked { get; }

        #endregion
    }
}
