using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.ViewModel
{
    /// <summary>
    /// ウィンドウに紐づくビューモデルにてウィンドウ状態の通知を受け取る。
    /// </summary>
    public interface IViewLifecycleReceiver
    {
        #region function

        /// <summary>
        /// ウィンドウが生成された。
        /// </summary>
        void ReceiveViewLoaded(Window window);

        /// <summary>
        /// ウィンドウが閉じられようとしている。
        /// </summary>
        /// <param name="e">キャンセルするかどうか。</param>
        void ReceiveViewClosing(CancelEventArgs e);
        /// <summary>
        /// ウィンドウが閉じられた。
        /// <para>設定状態によるけど基本的にdatacontextは空っぽ。</para>
        /// </summary>
        void ReceiveViewClosed();

        #endregion
    }

}
