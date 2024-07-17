using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// アドオン側で作成されるビューの管理。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface ILauncherItemAddonViewSupporter
    {
        #region property

        #endregion

        #region function

        /// <summary>
        /// ランチャーアイテムアドオンのウィンドウを Pe へ登録。
        /// </summary>
        /// <remarks>
        /// <para>登録されたウィンドウは Pe 管理下でそれっぽく制御される。</para>
        /// </remarks>
        /// <param name="window"></param>
        /// <param name="userClosing">ユーザー操作により閉じられようとしている。真: 閉じてOK, 偽: 閉じない。null の場合は閉じる。</param>
        /// <param name="closedWindow">ウィンドウが閉じた。null の場合は何もしない。</param>
        /// <returns>真: 登録成功。</returns>
        Task<bool> RegisterWindowAsync(Window window, Func<bool>? userClosing, Action? closedWindow, CancellationToken cancellationToken);

        #endregion
    }
}
