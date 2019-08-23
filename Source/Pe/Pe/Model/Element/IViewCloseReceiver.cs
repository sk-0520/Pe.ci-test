using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Element
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
