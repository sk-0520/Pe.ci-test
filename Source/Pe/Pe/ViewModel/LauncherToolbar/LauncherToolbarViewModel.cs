using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar
{
    public class LauncherToolbarViewModel : SingleModelViewModelBase<LauncherToolbarElement>, IAppDesktopToolbarExtendData, ILoggerFactory, IViewLifecycleReceiver
    {
        public LauncherToolbarViewModel(LauncherToolbarElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public AppDesktopToolbarExtend AppDesktopToolbarExtend { get; set; }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IAppDesktopToolbarExtendData

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition
        {
            get => Model.ToolbarPosition;
            set => SetModelValue(value);
        }

        /// <summary>
        /// ドッキング中か。
        /// </summary>
        public bool IsDocking
        {
            get => Model.IsDocking;
            set => SetModelValue(value);
        }

        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        public bool IsAutoHide
        {
            get => Model.IsAutoHide;
            set => SetModelValue(value);
        }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHiding
        {
            get => Model.IsHiding;
            set => SetModelValue(value);
        }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        public TimeSpan AutoHideTimeout => Model.AutoHideTimeout;

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size DisplaySize
        {
            get => Model.DisplaySize;
            set => SetModelValue(value);
        }

        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect DisplayBarArea
        {
            get => Model.DisplayBarArea;
            set => SetModelValue(value);
        }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize
        {
            get => Model.HiddenSize;
        }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HiddenBarArea
        {
            get => Model.HiddenBarArea;
            set => SetModelValue(value);
        }

        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        public bool ExistsFullScreenWindow
        {
            get => Model.ExistsFullScreenWindow;
            set => SetModelValue(value);
        }


        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        public Screen DockScreen => Model.DockScreen;

        #endregion

        #region ILoggerFactory

        public ILogger CreateLogger(string header) => Logger.Factory.CreateLogger(header);

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }


        #endregion
    }
}
