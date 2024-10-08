using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Main.ViewModels.Widget;
using ContentTypeTextNet.Pe.Main.Views.Converter;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Widget
{
    public class WidgetElement: ElementBase, IViewCloseReceiver, IPluginId
    {
        internal WidgetElement(IWidget widget, IPlugin plugin, WidgetAddonContextFactory widgetAddonContextFactory, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseDelayWriter mainDatabaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, ICultureService cultureService, IWindowManager windowManager, INotifyManager notifyManager, EnvironmentParameters environmentParameters, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Widget = widget;
            Plugin = plugin;
            WidgetAddonContextFactory = widgetAddonContextFactory;
            MainDatabaseBarrier = mainDatabaseBarrier;
            MainDatabaseDelayWriter = mainDatabaseDelayWriter;
            DatabaseStatementLoader = databaseStatementLoader;
            CultureService = cultureService;
            WindowManager = windowManager;
            NotifyManager = notifyManager;
            EnvironmentParameters = environmentParameters;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IDispatcherWrapper DispatcherWrapper { get; }
        private IWidget Widget { get; }
        private IPlugin Plugin { get; }
        private IPluginInformation PluginInformation => Plugin.PluginInformation;
        //PluginContextFactory PluginContextFactory { get; }
        private WidgetAddonContextFactory WidgetAddonContextFactory { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IMainDatabaseDelayWriter MainDatabaseDelayWriter { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private ICultureService CultureService { get; }
        private IWindowManager WindowManager { get; }
        private INotifyManager NotifyManager { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        public bool ViewCreated { get; private set; }

        public bool IsTopmost { get; private set; }

        private WindowItem? WindowItem { get; set; }

        #endregion

        #region function

        public string GetMenuHeader()
        {
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformation, reader, true);
                return Widget.GetMenuHeader(context);
            }
        }

        public DependencyObject? GetMenuIcon()
        {
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformation, reader, true);
                return Widget.GetMenuIcon(context);
            }
        }

        private Window CreateWindowWidget(WidgetAddonCreateContext context)
        {
            Debug.Assert(Widget.ViewType == WidgetViewType.Window);

            var window = Widget.CreateWindowWidget(context);
            return window;
        }

        /// <summary>
        /// ウィジェット用にウィンドウを調節。
        /// </summary>
        /// <remarks>
        /// <para>生成時のスタイル変更は<see cref="WidgetViewModelBase.ReceiveViewInitialized(Window)"/>を参照。</para>
        /// </remarks>
        /// <param name="window"></param>
        private void AdjustWindow(Window window)
        {
            Debug.Assert(!window.IsVisible);

            // タイトルバーは強制的に変更
            var titleConvert = new TitleConverter();
            window.Title = (string)titleConvert.Convert($"{PluginInformation.PluginIdentifiers.PluginName}({PluginInformation.PluginIdentifiers.PluginId})", typeof(string), null!, CultureService.Culture);

            // 最前面強制不可・バインディング解除
            window.Topmost = false;
            // タスクバーにも表示しない
            window.ShowInTaskbar = false;
            // ウィンドウの透明度は Pe 側で制御
            window.Opacity = 0;
            // 最前面表示は Pe 側で制御
            window.Topmost = false;

            // ウィンドウ位置指定
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(PluginId)) {
                    var setting = pluginWidgetSettingsEntityDao.SelectPluginWidgetSetting(PluginId);
                    var isTopmost = pluginWidgetSettingsEntityDao.SelectPluginWidgetTopmost(PluginId);

                    window.WindowStartupLocation = WindowStartupLocation.Manual;
                    window.Topmost = isTopmost;

                    if(!double.IsNaN(setting.X) && !double.IsNaN(setting.Height)) {
                        window.Left = setting.X;
                        window.Top = setting.Y;
                    } else {
                        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    }
                    if(window.ResizeMode != ResizeMode.NoResize && !double.IsNaN(setting.Width) && !double.IsNaN(setting.Height)) {
                        window.Width = setting.Width;
                        window.Height = setting.Height;
                    }

                } else {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
        }

        public void ShowView(ViewModelBase callerViewModel)
        {
            if(WindowItem != null) {
                throw new InvalidOperationException(nameof(WindowItem));
            }

            Window window;
            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                using var context = WidgetAddonContextFactory.CreateCreateContext(PluginInformation, reader);
                window = Widget.ViewType switch {
                    WidgetViewType.Window => CreateWindowWidget(context),
                    _ => throw new NotImplementedException(),
                };
            }
            if(window.IsVisible) {
                Logger.LogError("ウィジェットの表示・非表示制御は Pe 側で処理するためウィジェット強制停止");
                window.Close();
                return;
            }
            AdjustWindow(window);
            WindowItem = new WindowItem(Manager.WindowKind.Widget, this, callerViewModel, window) {
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

        public void ToggleTopmost()
        {
            var newIsTopmost = !IsTopmost;
            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(PluginId)) {
                    pluginWidgetSettingsEntityDao.UpdatePluginWidgetTopmost(PluginId, newIsTopmost, DatabaseCommonStatus.CreatePluginAccount(PluginInformation.PluginIdentifiers, PluginInformation.PluginVersions));
                } else {
                    pluginWidgetSettingsEntityDao.InsertPluginWidgetTopmost(PluginId, newIsTopmost, DatabaseCommonStatus.CreatePluginAccount(PluginInformation.PluginIdentifiers, PluginInformation.PluginVersions));
                }
                context.Commit();
            }

            IsTopmost = newIsTopmost;
            if(WindowItem != null) {
                WindowItem.Window.Topmost = IsTopmost;
            }
        }

        public void SaveStatus(bool isVisible)
        {
            if(WindowItem == null) {
                return;
            }

            ThrowIfDisposed();

            var data = new PluginWidgetSettingData() {
                X = WindowItem.Window.Left,
                Y = WindowItem.Window.Top,
                IsVisible = isVisible, // こいつだけはユーザー操作であるか否かで変わってくる
                IsTopmost = WindowItem.Window.Topmost,
            };
            if(WindowItem.Window.ResizeMode == ResizeMode.NoResize) {
                data.Width = double.NaN;
                data.Height = double.NaN;
            } else {
                data.Width = WindowItem.Window.Width;
                data.Height = WindowItem.Window.Height;
            }


            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(PluginId)) {
                    pluginWidgetSettingsEntityDao.UpdatePluginWidgetSetting(PluginId, data, DatabaseCommonStatus.CreatePluginAccount(PluginInformation.PluginIdentifiers, PluginInformation.PluginVersions));
                } else {
                    pluginWidgetSettingsEntityDao.InsertPluginWidgetSetting(PluginId, data, DatabaseCommonStatus.CreatePluginAccount(PluginInformation.PluginIdentifiers, PluginInformation.PluginVersions));
                }
                context.Commit();
            }
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            IsTopmost = MainDatabaseBarrier.ReadData(c => {
                var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(PluginId)) {
                    return pluginWidgetSettingsEntityDao.SelectPluginWidgetTopmost(PluginId);
                }

                return false;
            });

            return Task.CompletedTask;
        }

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

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool, CancellationToken)"/>
        public Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            if(WindowItem == null) {
                throw new InvalidOperationException(nameof(WindowItem));
            }

            if(isUserOperation) {
                SaveStatus(false);
            }

            using(var writer = WidgetAddonContextFactory.BarrierWrite()) {
                using var context = WidgetAddonContextFactory.CreateClosedContext(PluginInformation, writer);
                Widget.ClosedWidget(context);
                WidgetAddonContextFactory.Save();
            }
            WindowItem.Window.Loaded -= Window_Loaded;
            WindowItem.Window.ContentRendered -= Window_ContentRendered;

            WindowItem = null;
            ViewCreated = false;

            return Task.CompletedTask;
        }


        #endregion

        #region IPluginId

        public PluginId PluginId => PluginInformation.PluginIdentifiers.PluginId;

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(WindowItem != null);

            WindowItem.Window.Loaded -= Window_Loaded;
            WindowItem.Window.ContentRendered += Window_ContentRendered;

            using(var reader = WidgetAddonContextFactory.BarrierRead()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformation, reader, true);
                Widget.OpeningWidget(context);
            }
        }

        private void Window_ContentRendered(object? sender, EventArgs e)
        {
            Debug.Assert(WindowItem != null);

            WindowItem.Window.Opacity = 1;

            // 書き込みは一応OKにしておく
            using(var writer = WidgetAddonContextFactory.BarrierWrite()) {
                var context = WidgetAddonContextFactory.CreateContext(PluginInformation, writer, false);
                Widget.OpenedWidget(context);
                WidgetAddonContextFactory.Save();
            }
        }
    }
}
