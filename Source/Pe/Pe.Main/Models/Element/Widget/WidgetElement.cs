using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Widget;
using ContentTypeTextNet.Pe.Main.Views.Converter;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Widget
{
    public class WidgetElement: ElementBase, IViewCloseReceiver
    {
        internal WidgetElement(IWidget widget, IPluginInformations pluginInformations, WidgetAddonContextFactory widgetAddonContextFactory, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, CultureService cultureService, IWindowManager windowManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Widget = widget;
            PluginInformations = pluginInformations;
            WidgetAddonContextFactory = widgetAddonContextFactory;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            CultureService = cultureService;
            WindowManager = windowManager;
            NotifyManager = notifyManager;
        }

        #region property

        IWidget Widget { get; }
        IPluginInformations PluginInformations { get; }
        //PluginContextFactory PluginContextFactory { get; }
        WidgetAddonContextFactory WidgetAddonContextFactory { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        CultureService CultureService { get; }
        IWindowManager WindowManager { get; }
        INotifyManager NotifyManager { get; }
        public bool ViewCreated { get; private set; }

        WindowItem? WindowItem { get; set; }

        #endregion

        #region function

        public string GetMenuHeader()
        {
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformations, reader, true);
                return Widget.GetMenuHeader(context);
            }
        }

        public DependencyObject? GetMenuIcon()
        {
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformations, reader, true);
                return Widget.GetMenuIcon(context);
            }
        }

        Window CreateWindowWidget(WidgetAddonCreateContext context)
        {
            Debug.Assert(Widget.ViewType == WidgetViewType.Window);

            var window = Widget.CreateWindowWidget(context);
            return window;
        }

        Window CreateWebViewWidget(WidgetAddonCreateContext context)
        {
            Debug.Assert(Widget.ViewType == WidgetViewType.WebView);
            throw new NotImplementedException();
        }

        /// <summary>
        /// ウィジェット用にウィンドウを調節。
        /// <para>生成時のスタイル変更は<see cref="WidgetViewModelBase.ReceiveViewInitialized(Window)"/>を参照。</para>
        /// </summary>
        /// <param name="window"></param>
        void TuneWindow(Window window)
        {
            Debug.Assert(!window.IsVisible);

            // タイトルバーは強制的に変更
            var titleConvert = new TitleConverter();
            window.Title = (string)titleConvert.Convert($"{PluginInformations.PluginIdentifiers.PluginName}({PluginInformations.PluginIdentifiers.PluginId})", typeof(string), null!, CultureService.Culture);

            // 最前面強制不可・バインディング解除
            window.Topmost = false;
            // タスクバーにも表示しない
            window.ShowInTaskbar = false;
            // ウィンドウの透明度は Pe 側で制御
            window.Opacity = 0;
        }

        public void ShowView(ViewModelBase callerViewModel)
        {
            if(WindowItem != null) {
                throw new InvalidOperationException(nameof(WindowItem));
            }

            Window window;
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateCreateContex(PluginInformations, reader);
                window = Widget.ViewType switch
                {
                    WidgetViewType.Window => CreateWindowWidget(context),
                    WidgetViewType.WebView => CreateWebViewWidget(context),
                    _ => throw new NotImplementedException(),
                };
            }
            if(window.IsVisible) {
                Logger.LogError("ウィジェットの表示・非表示制御は Pe 側で処理するためウィジェット強制停止");
                window.Close();
                return;
            }
            TuneWindow(window);
            WindowItem = new WindowItem(WindowKind.Widget, this, callerViewModel, window) {
                CloseToDispose = callerViewModel is TemporaryWidgetViewModel, // ダミーのやつは殺して、通知領域のやつは生かしておく
            };
            WindowManager.Register(WindowItem);

            WindowItem.Window.Loaded += Window_Loaded;

            WindowItem.Window.Show();
            ViewCreated = true;
        }

        public void HideView()
        {
            if(!ViewCreated) {
                throw new InvalidOperationException(nameof(ViewCreated));
            }
            if(WindowItem == null) {
                throw new InvalidOperationException(nameof(WindowItem));
            }

            WindowItem.Window.Close();
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public void ReceiveViewClosed(bool isUserOperation)
        {
            if(WindowItem == null) {
                throw new InvalidOperationException(nameof(WindowItem));
            }
            using(var writer = WidgetAddonContextFactory.BarrierWrite()) {
                var context = WidgetAddonContextFactory.CreateClosedContext(PluginInformations, writer);
                Widget.ClosedWidget(context);
                WidgetAddonContextFactory.Save();
            }
            WindowItem.Window.Loaded -= Window_Loaded;
            WindowItem.Window.ContentRendered -= Window_ContentRendered;

            WindowItem = null;
            ViewCreated = false;
        }


        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(WindowItem != null);

            WindowItem.Window.Loaded -= Window_Loaded;
            WindowItem.Window.ContentRendered += Window_ContentRendered;

            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformations, reader, true);
                Widget.OpeningWidget(context);
            }
        }

        private void Window_ContentRendered(object? sender, EventArgs e)
        {
            Debug.Assert(WindowItem != null);

            WindowItem.Window.Opacity = 1;

            // 書き込みは一応OKにしておく
            using(var writer = WidgetAddonContextFactory.BarrierWrite()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformations, writer, false);
                Widget.OpenedWidget(context);
                WidgetAddonContextFactory.Save();
            }
        }
    }
}
