using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ウィジェットビュー種別。
    /// </summary>
    public enum WidgetViewKind
    {
        /// <summary>
        /// 通常の<see cref="System.Windows.Window"/>をウィジェット側で生成。
        /// </summary>
        Window,
        /// <summary>
        /// HTML的な。
        /// <para>考え中だけど、Pe側で透明ウィンドウとCEFぶっこんでウィジェット側からそこにHTMLを流し込むイメージ。</para>
        /// <para>IMEはあきらめ。</para>
        /// </summary>
        WebView,
    }

    /// <summary>
    /// ウィジェット！
    /// </summary>
    public interface IWidget
    {
        #region property

        /// <summary>
        /// このウィジェットのビュー種別。
        /// </summary>
        WidgetViewKind ViewKind { get; }

        #endregion

        #region function

        /// <summary>
        /// 通知領域ウィジェットメニューアイコン。
        /// </summary>
        /// <returns></returns>
        object GetMenuIcon(IPluginContext pluginContext);
        /// <summary>
        /// 通知領域ウィジェットメニューヘッダ。
        /// </summary>
        /// <returns></returns>
        string GetMenuHeader(IPluginContext pluginContext);

        /// <summary>
        /// ウィンドウウィジェットを生成。
        /// </summary>
        /// <returns></returns>
        Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext);

        /// <summary>
        /// WebViewウィジェットを生成。
        /// </summary>
        /// <returns></returns>
        IHtmlSource CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext);

        /// <summary>
        /// ウィジェットが閉じられた際に呼ばれる。
        /// </summary>
        void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext);

        #endregion
    }
}
