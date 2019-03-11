using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Toolbar
{
    public class LauncherToolbarElement : ContextElementBase, IAppDesktopToolbarExtendData
    {
        public LauncherToolbarElement(Screen dockScreen, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DockScreen = dockScreen;
        }

        #region property

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 表示アイコンサイズ。
        /// </summary>
        public IconScale IconScale { get; private set; }

        /// <summary>
        /// アイコンの余白。
        /// <para>CSS の margin と同じ考え方。</para>
        /// </summary>
        public Thickness IconMargin { get; private set; }

        /// <summary>
        /// ボタンの詰め領域。
        /// <para>CSS の padding と同じ考え方。</para>
        /// </summary>
        public Thickness ButtonPadding { get; private set; }

        /// <summary>
        /// テキストを表示するか。
        /// </summary>
        public bool IsTextVisible { get; private set; }

        /// <summary>
        /// テキスト表示の際の表示幅。
        /// </summary>
        [PixelKind(Px.Logical)]
        public double TextWidth { get; private set; }

        #endregion

        #region function

        #endregion

        #region IAppDesktopToolbarExtendData

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        /// <summary>
        /// ドッキング中か。
        /// </summary>
        public bool IsDocking { get; set; }
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        public bool IsAutoHide { get; set; }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHiding { get; set; }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        public TimeSpan AutoHideTime { get; private set; }

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size DisplaySize { get; set; }
        /// <summary>
        /// 隠れているバーのサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize { get; set; }

        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect ShowLogicalBarArea { get; set; }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public double HideWidth { get; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HideLogicalBarArea { get; set; }

        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        public bool ExistsFullScreenWindow { get; set; }


        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Screen DockScreen { get; }

        #endregion
    }
}
