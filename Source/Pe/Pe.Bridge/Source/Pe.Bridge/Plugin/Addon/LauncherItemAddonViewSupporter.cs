using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// アドオン側で作成されるビューの管理。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonViewSupporter
    {
        #region property

        /// <summary>
        /// ユーザー操作により閉じられようとしている。
        /// </summary>
        event EventHandler<CancelEventHandler> UserClosing;
        /// <summary>
        /// ウィンドウが閉じた。
        /// </summary>
        event EventHandler ClosedWindow;

        #endregion

        #region property

        #endregion

        #region function

        /// <summary>
        /// ランチャーアイテムアドオンのウィンドウを Pe へ登録。
        /// <para>登録されたウィンドウは Pe 管理下でそれっぽく制御される。</para>
        /// </summary>
        /// <param name="window"></param>
        /// <returns>真: 登録成功。</returns>
        bool RegisterWindow(Window window);

        /// <summary>
        /// ランチャーアイテムアドオンのウィンドウを Pe 管理下から外す。
        /// <para>よほどのことがない限り使用する必要なし。</para>
        /// <para>※多分これの実装は後回し。</para>
        /// </summary>
        /// <param name="window"></param>
        /// <returns>真: 解除成功。</returns>
        void UnregisterWindow(Window window);

        /// <summary>
        /// ランチャーアイテムアドオンのウィンドウが登録されているか。
        /// </summary>
        /// <param name="window"></param>
        /// <returns>真: 登録済み。</returns>
        bool IsRegisteredWindow(Window window);

        #endregion
    }
}
