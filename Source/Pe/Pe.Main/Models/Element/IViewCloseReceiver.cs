using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{
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
        void ReceiveViewClosed();

        #endregion
    }
}
