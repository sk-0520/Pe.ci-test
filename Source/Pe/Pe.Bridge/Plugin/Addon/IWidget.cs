using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ウィジェットビュー種別。
    /// </summary>
    public enum WidgetViewType
    {
        /// <summary>
        /// 通常の<see cref="System.Windows.Window"/>をウィジェット側で生成。
        /// </summary>
        Window,
    }

    /// <summary>
    /// ウィジェット！
    /// </summary>
    /// <remarks>
    /// <para>1アドオンにつき1ウィンドウのみ生成可能な限定的UIとして扱う。ユーザー通常操作の妨げにならず、簡単な機能を提供するUIを目標とする。</para>
    /// <para>プライマリスクリーンの左上を原点とし、表示制御は Pe 側で処理される。各種パラメータの一部は Pe 側で制御され、ウィンドウスタイルは WS_EX_TOOLWINDOW が強制され最大化最小化も不可となる。</para>
    /// </remarks>
    public interface IWidget
    {
        #region property

        /// <summary>
        /// このウィジェットのビュー種別。
        /// </summary>
        WidgetViewType ViewType { get; }

        #endregion

        #region function

        /// <summary>
        /// 通知領域ウィジェットメニューアイコン。
        /// </summary>
        /// <returns></returns>
        DependencyObject? GetMenuIcon(IPluginContext pluginContext);
        /// <summary>
        /// 通知領域ウィジェットメニューヘッダ。
        /// </summary>
        /// <returns></returns>
        string GetMenuHeader(IPluginContext pluginContext);

        /// <summary>
        /// ウィンドウウィジェットを生成。
        /// </summary>
        /// <remarks>
        /// <para>ウィンドウの表示・非表示制御は Pe 側にて処理するため <see cref="Window.Show"/>/<see cref="UIElement.Visibility"/> はウィジェット側で操作しないこと。</para>
        /// </remarks>
        /// <returns></returns>
        Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext);

        /// <summary>
        /// ウィジェットが開かれる際に呼ばれる。
        /// </summary>
        void OpeningWidget(IPluginContext pluginContext);

        /// <summary>
        /// ウィジェットが開かれた際に呼ばれる。
        /// </summary>
        void OpenedWidget(IPluginContext pluginContext);

        /// <summary>
        /// ウィジェットが閉じられた際に呼ばれる。
        /// </summary>
        void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext);

        #endregion
    }
}
