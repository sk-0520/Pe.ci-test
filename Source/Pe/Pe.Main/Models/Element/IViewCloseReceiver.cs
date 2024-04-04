using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    /// <summary>
    /// ビューが閉じられたことを受信する。
    /// </summary>
    public interface IViewCloseReceiver
    {
        #region function

        /// <summary>
        /// ビューがユーザーにより閉じられようとしている。
        /// </summary>
        /// <returns>真: 閉じれる。 偽: キャンセル。</returns>
        bool ReceiveViewUserClosing();
        /// <summary>
        /// ビューが閉じられようとしている。
        /// </summary>
        /// <returns>真: 閉じれる。 偽: キャンセル。</returns>
        bool ReceiveViewClosing();
        /// <summary>
        /// ビューが閉じられた。
        /// </summary>
        /// <param name="isUserOperation">ユーザー操作によって閉じられたか。</param>
        Task ReceiveViewClosedAsync(bool isUserOperation);

        #endregion
    }
}
