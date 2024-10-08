using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem
{
    public class LauncherItemElement: ElementBase, ILauncherItemId
    {
        #region variable

        private bool _nowCustomize;

        #endregion

        public LauncherItemElement(LauncherItemId launcherItemId, LauncherItemAddonContextFactory launcherItemAddonContextFactory, ILauncherItemAddonFinder launcherItemAddonFinder, LauncherItemAddonViewSupporterCollection launcherItemAddonViewSupporterCollection, IWindowManager windowManager, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
            LauncherItemAddonFinder = launcherItemAddonFinder;
            LauncherItemAddonViewSupporterCollection = launcherItemAddonViewSupporterCollection;

            WindowManager = windowManager;
            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        private ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        private LauncherItemAddonViewSupporterCollection LauncherItemAddonViewSupporterCollection { get; }
        private IWindowManager WindowManager { get; }
        private IOrderManager OrderManager { get; }
        private IClipboardManager ClipboardManager { get; }
        private INotifyManager NotifyManager { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;
        public string Name { get; private set; } = string.Empty;
        public LauncherItemKind Kind { get; private set; }
        public bool IsEnabledCommandLauncher { get; private set; }
        public string? Comment { get; private set; }

        public virtual bool NowCustomizing
        {
            get => this._nowCustomize;
            private set => SetProperty(ref this._nowCustomize, value);
        }

        #endregion

        #region function

        private void LoadLauncherItem()
        {
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Kind = launcherItemData.Kind;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;
            }
        }

        public LauncherFileDetailData LoadFileDetail()
        {
            ThrowIfDisposed();

            LauncherExecutePathData pathData;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                pathData = launcherFilesEntityDao.SelectPath(LauncherItemId);
            }

            var expandedPath = Environment.ExpandEnvironmentVariables(pathData.Path ?? string.Empty);
            var fullPath = EnvironmentPathExecuteFileCache.ToFullPathIfExistsCommand(expandedPath, LoggerFactory);
            var result = new LauncherFileDetailData() {
                PathData = pathData,
                FullPath = fullPath,
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="callerObject">このアイテムを生成するにあたり呼び出し元となったViewModelオブジェクト。これで何とか一意制約をををををを。</param>
        /// <returns></returns>
        internal LauncherAddonDetailData LoadAddonDetail(object callerObject)
        {
            ThrowIfDisposed();

            var result = new LauncherAddonDetailData();

            var pluginId = MainDatabaseBarrier.ReadData(c => {
                var launcherAddonsEntityDao = new LauncherAddonsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);
            });

            result.IsEnabled = LauncherItemAddonFinder.Exists(pluginId);
            if(result.IsEnabled) {
                result.Extension = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
                result.Extension.ChangeDisplay(Bridge.Plugin.Addon.LauncherItemIconMode.Toolbar, true, callerObject);
            }

            return result;
        }

        internal LauncherSeparatorData LoadSeparator()
        {
            LauncherSeparatorData launcherSeparatorData;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherSeparatorsEntityDao = new LauncherSeparatorsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherSeparatorData = launcherSeparatorsEntityDao.SelectSeparator(LauncherItemId);
            }

            return launcherSeparatorData;
        }

        private List<LauncherEnvironmentVariableData> GetMergeEnvironmentVariableItems(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();

            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            return launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
        }

        private async Task<ILauncherExecuteResult> ExecuteFileAsync(string? customArgument, IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            LauncherFileData fileData;
            List<LauncherEnvironmentVariableData> envItems;
            LauncherRedoData redoData;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                if(customArgument != null) {
                    Logger.LogInformation("引数指定があるため上書き: [元] {0}, [優先] {1}", fileData.Option, customArgument);
                    fileData.Option = customArgument;
                }
                if(fileData.IsEnabledCustomEnvironmentVariable) {
                    envItems = GetMergeEnvironmentVariableItems(context, context.Implementation);
                } else {
                    envItems = new List<LauncherEnvironmentVariableData>();
                }

                redoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                var exitCodes = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                redoData.SuccessExitCodes.SetRange(exitCodes);
            }
            if(!redoData.SuccessExitCodes.Any()) {
                redoData.SuccessExitCodes.Add(0);
            }
            fileData.Caption = Name;

            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache, OrderManager, NotifyManager, DispatcherWrapper, LoggerFactory);
            var result = await launcherExecutor.ExecuteAsync(Kind, fileData, fileData, envItems, redoData, screen, cancellationToken);

            return result;
        }

        private Task<LauncherAddonExecuteResult> ExecuteAddonAsync(string? customArgument, IScreen screen, CancellationToken cancellationToken)
        {
            if(LauncherItemAddonViewSupporterCollection.ExistsInformation(LauncherItemId)) {
                Logger.LogInformation("ランチャーアイテムはすでに起動している: {0}", LauncherItemId);
                LauncherItemAddonViewSupporterCollection.Foreground(LauncherItemId);

                return Task.FromResult(new LauncherAddonExecuteResult() {
                    Kind = LauncherItemKind.Addon,
                    Data = LauncherAddonExecuteKind.Duplicate,
                    Success = false,
                });
            }

            var pluginId = MainDatabaseBarrier.ReadData(c => {
                var launcherAddonsEntityDao = new LauncherAddonsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);
            });

            var addon = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
            var plugin = LauncherItemAddonFinder.GetPlugin(pluginId);
            var commandExecuteParameter = new CommandExecuteParameter(screen, false);
            var launcherItemAddonViewSupporter = LauncherItemAddonViewSupporterCollection.Create(plugin.PluginInformation, LauncherItemId);
            var launcherItemExtensionExecuteParameter = LauncherItemAddonContextFactory.CreateExtensionExecuteParameter(plugin.PluginInformation, LauncherItemId, launcherItemAddonViewSupporter);
            DispatcherWrapper.BeginAsync(() => {
                using var databaseContextsPack = PersistenceHelper.WaitWritePack(MainDatabaseBarrier, LargeDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreatePluginAccount(plugin.PluginInformation));
                using(var context = LauncherItemAddonContextFactory.CreateContext(plugin.PluginInformation, LauncherItemId, databaseContextsPack, false)) {
                    addon.Execute(customArgument, commandExecuteParameter, launcherItemExtensionExecuteParameter, context);
                }
            });

            return Task.FromResult(new LauncherAddonExecuteResult() {
                Kind = LauncherItemKind.Addon,
                Data = LauncherAddonExecuteKind.Execute,
                Success = true,
            });
        }

        private async Task<ILauncherExecuteResult> ExecuteCoreAsync(string? argument, IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try {
                ILauncherExecuteResult result;
                switch(Kind) {
                    case LauncherItemKind.File:
                        result = await ExecuteFileAsync(argument, screen, cancellationToken);
                        break;

                    case LauncherItemKind.Addon:
                        result = await ExecuteAddonAsync(argument, screen, cancellationToken);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Debug.Assert(result != null);

                IncrementExecuteCount();

                return result;

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return new LauncherExecuteErrorResult(Kind, ex);
            }
        }

        public Task<ILauncherExecuteResult> ExecuteAsync(IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return ExecuteCoreAsync(null, screen, cancellationToken);
        }

        public async Task<ILauncherExecuteResult> DirectExecuteAsync(string argument, IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if(argument == null) {
                return new LauncherExecuteErrorResult(Kind, new ArgumentNullException(nameof(argument)));
            }

            return await ExecuteCoreAsync(argument, screen, cancellationToken);
        }

        private void IncrementExecuteCount()
        {
            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var dao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                dao.UpdateExecuteCountIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());
                context.Commit();
            }
        }

        public Task OpenExtendsExecuteViewAsync(IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return DispatcherWrapper.BeginAsync(async () => {
                var element = await OrderManager.CreateLauncherExtendsExecuteElementAsync(LauncherItemId, screen, cancellationToken);
                element.StartView();
            });
        }

        public async Task OpenExtendsExecuteViewWidthArgumentAsync(string argument, IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var element = await OrderManager.CreateLauncherExtendsExecuteElementAsync(LauncherItemId, screen, cancellationToken);
            element.SetOption(argument);
            element.StartView();
        }

        private ILauncherExecutePathParameter GetExecutePath()
        {
            Debug.Assert(Kind == LauncherItemKind.File);
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return launcherFilesEntityDao.SelectPath(LauncherItemId);
            }
        }

        public ILauncherExecuteResult OpenParentDirectory()
        {
#pragma warning disable S1940 // 増える可能性があるので抑制
            if(!(Kind == LauncherItemKind.File)) {
#pragma warning restore S1940
                throw new InvalidOperationException($"{Kind}");
            }
            ThrowIfDisposed();

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache, OrderManager, NotifyManager, DispatcherWrapper, LoggerFactory);
            var result = launcherExecutor.OpenParentDirectory(Kind, pathData);

            return result;

        }

        public ILauncherExecuteResult OpenWorkingDirectory()
        {
#pragma warning disable S1940 // 増える可能性があるので抑制
            if(!(Kind == LauncherItemKind.File)) {
#pragma warning restore S1940
                throw new InvalidOperationException($"{Kind}");
            }
            ThrowIfDisposed();

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache, OrderManager, NotifyManager, DispatcherWrapper, LoggerFactory);
            var result = launcherExecutor.OpenWorkingDirectory(Kind, pathData);

            return result;

        }

        public void CopyExecutePath()
        {
            ThrowIfDisposed();

            var pathData = GetExecutePath();
            ClipboardManager.CopyText(pathData.Path, ClipboardNotify.None);
        }

        public void CopyParentDirectory()
        {
            ThrowIfDisposed();

            var pathData = GetExecutePath();
            var path = Environment.ExpandEnvironmentVariables(pathData.Path ?? string.Empty);

            var value = Path.GetDirectoryName(path);
            if(string.IsNullOrEmpty(value)) {
                if(!PathUtility.IsNetworkDirectoryPath(path)) {
                    var fullPath = EnvironmentPathExecuteFileCache.ToFullPathIfExistsCommand(path, LoggerFactory);
                    if(ReferenceEquals(fullPath, path) || fullPath == null) {
                        Logger.LogWarning("親ディレクトリ不明: {0}, {1}", path, LauncherItemId);
                        return;
                    }
                    value = Path.GetDirectoryName(fullPath);
                    if(string.IsNullOrEmpty(value)) {
                        Logger.LogWarning("親ディレクトリ不明: {0}, {1}", fullPath, LauncherItemId);
                        return;
                    }
                } else {
                    var owner = PathUtility.GetNetworkOwnerName(path);
                    if(string.IsNullOrEmpty(owner)) {
                        Logger.LogWarning("親ディレクトリ不明: {0}, {1}", path, LauncherItemId);
                        return;
                    }
                    value = owner;
                }
            }
            ClipboardManager.CopyText(value, ClipboardNotify.None);
        }

        public void CopyOption()
        {
            ThrowIfDisposed();

            var pathData = GetExecutePath();
            ClipboardManager.CopyText(pathData.Option, ClipboardNotify.None);
        }

        public void CopyWorkingDirectory()
        {
            ThrowIfDisposed();

            var pathData = GetExecutePath();
            ClipboardManager.CopyText(pathData.WorkDirectoryPath, ClipboardNotify.None);
        }

        public async Task OpenCustomizeViewAsync(IScreen screen, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if(NowCustomizing) {
                Logger.LogWarning("現在編集中: {0}", LauncherItemId);
                //OrderManager.FlashCustomizeLauncherItem(LauncherItemId);
                //WindowManager.Flash()
                var items = WindowManager.GetWindowItems(Manager.WindowKind.LauncherCustomize);
                var item = items.FirstOrDefault(i => ((ILauncherItemId)i.ViewModel).LauncherItemId == LauncherItemId);
                if(item != null) {
                    WindowManager.Flash(item);
                }
                return;
            }

            //TODO: 確定時の処理
            NowCustomizing = true;
            NotifyManager.CustomizeLauncherItemExited += NotifyManager_CustomizeLauncherItemExited;
            var element = await OrderManager.CreateCustomizeLauncherItemContainerElementAsync(LauncherItemId, screen, cancellationToken);
            element.StartView();
        }

        public void ShowProperty()
        {
#pragma warning disable S1940 // 増える可能性があるので抑制
            if(!(Kind == LauncherItemKind.File)) {
#pragma warning restore S1940 //
                throw new InvalidOperationException($"{Kind}");
            }
            ThrowIfDisposed();

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(EnvironmentPathExecuteFileCache, OrderManager, NotifyManager, DispatcherWrapper, LoggerFactory);
            launcherExecutor.ShowProperty(pathData);
        }

        internal void Refresh()
        {
            LoadLauncherItem();
        }


        public LauncherIconFactory CreateLauncherIconFactory()
        {
            return new LauncherIconFactory(LauncherItemId, Kind, LauncherItemAddonFinder, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
        }

        #endregion

        #region ElementBase

        override protected Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            LoadLauncherItem();

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.CustomizeLauncherItemExited -= NotifyManager_CustomizeLauncherItemExited;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion

        private void NotifyManager_CustomizeLauncherItemExited(object? sender, CustomizeLauncherItemExitedEventArgs e)
        {
            ThrowIfDisposed();

            if(e.LauncherItemId == LauncherItemId) {
                NotifyManager.CustomizeLauncherItemExited -= NotifyManager_CustomizeLauncherItemExited;
                NowCustomizing = false;
            }
        }
    }
}
