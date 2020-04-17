using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    /// <summary>
    /// ウィンドウに紐づくビューモデルにてウィンドウ状態の通知を受け取る。
    /// </summary>
    public interface IViewLifecycleReceiver
    {
        #region function

        /// <summary>
        /// 分離できるって啓蒙してる人らはなんなの、全部添付とかサブクラス作れって言うの
        /// </summary>
        /// <param name="window"></param>
        void ReceiveViewInitialized(Window window);

        /// <summary>
        /// ウィンドウが生成された。
        /// </summary>
        void ReceiveViewLoaded(Window window);

        /// <summary>
        /// ウィンドウが閉じられようとしている。
        /// <para><see cref="ReceiveViewClosing"/>と違い、操作ユーザーが閉じる場合に呼ばれる。</para>
        /// </summary>
        /// <param name="e">キャンセルするかどうか。</param>
        void ReceiveViewUserClosing(CancelEventArgs e);

        /// <summary>
        /// ウィンドウが閉じられようとしている。
        /// <para><see cref="ReceiveViewUserClosing"/>と違い、操作ユーザー以外が閉じる場合(シャットダウンとかかなぁ)でも呼ばれる。</para>
        /// </summary>
        /// <param name="e">キャンセルするかどうか。</param>
        void ReceiveViewClosing(CancelEventArgs e);
        /// <summary>
        /// ウィンドウが閉じられた。
        /// <para>設定状態によるけど基本的にdatacontextは空っぽ。</para>
        /// </summary>
        void ReceiveViewClosed(Window window);

        #endregion
    }

}
