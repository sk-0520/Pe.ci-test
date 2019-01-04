using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.View;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Toolbar;

namespace ContentTypeTextNet.Pe.Main.View.Extend
{
    public enum AppDesktopToolbarPosition
    {
        Left,
        Top,
        Right,
        Bottom,
    }

    public interface IAppDesktopToolbarExtendData : INotifyPropertyChanged
    {
        #region property

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        AppDesktopToolbarPosition ToolbarPosition { get; set; }
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        bool IsAutoHide { get; set; }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        bool IsHiding { get; set; }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        TimeSpan AutoHideTime { get; set; }

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect DisplaySize { get; set; }
        /// <summary>
        /// 隠れているバーのサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        Rect HiddenSize { get; set; }

        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        Screen DockScreen { get; set; }

        #endregion
    }

    public class AppDesktopToolbarExtend : WinProcExtendBase<Window, IAppDesktopToolbarExtendData>
    {
        public AppDesktopToolbarExtend(Window view, IAppDesktopToolbarExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        { }
    }
}
