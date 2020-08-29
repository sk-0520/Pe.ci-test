using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem
{
    public class LauncherItemElement: ElementBase, ILauncherItemId
    {
        #region variable

        bool _nowCustomize;

        #endregion

        public LauncherItemElement(Guid launcherItemId, ILauncherItemAddonFinder launcherItemAddonFinder, IWindowManager windowManager, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            LauncherItemAddonFinder = launcherItemAddonFinder;
            WindowManager = windowManager;
            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        IWindowManager WindowManager { get; }
        IOrderManager OrderManager { get; }
        IClipboardManager ClipboardManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
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

        void LoadLauncherItem()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;
            }
        }

        public LauncherFileDetailData LoadFileDetail()
        {
            ThrowIfDisposed();

            LauncherExecutePathData pathData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
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

        internal LauncherAddonDetailData LoadAddonDetail()
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
                result.Extension.Display(Bridge.Plugin.Addon.LauncherItemDisplayMode.LauncherItem);
            }

            return result;
        }

        List<LauncherEnvironmentVariableData> GetMergeEnvironmentVariableItems(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();

            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            return launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
        }

        ILauncherExecuteResult ExecuteFile(string? customArgument, IScreen screen)
        {
            ThrowIfDisposed();

            LauncherFileData fileData;
            List<LauncherEnvironmentVariableData> envItems;
            LauncherRedoData redoData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                if(customArgument != null) {
                    Logger.LogInformation("引数指定があるため上書き: [元] {0}, [優先] {1}", fileData.Option, customArgument);
                    fileData.Option = customArgument;
                }
                if(fileData.IsEnabledCustomEnvironmentVariable) {
                    envItems = GetMergeEnvironmentVariableItems(commander, commander.Implementation);
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
            var result = launcherExecutor.Execute(Kind, fileData, fileData, envItems, redoData, screen);

            return result;
        }

        public ILauncherExecuteResult Execute(IScreen screen)
        {
            ThrowIfDisposed();

            //#if DEBUG
            //            var id = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Topmost, "@テスト", new NotifyLogContent(Name)));
            //            Task.Run(() => {
            //                Thread.Sleep(TimeSpan.FromSeconds(5));
            //                NotifyManager.ReplaceLog(id, "@うんこー");
            //                Thread.Sleep(TimeSpan.FromSeconds(5));
            //                NotifyManager.ClearLog (id);
            //            });
            //            NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Normal, "@ランチャーアイテム起動", new NotifyLogContent(Name)));
            //            NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Command, "@Command", new NotifyLogContent(Name), () => { Logger.LogInformation("command"); }));
            //            NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Undo, "@Undo", new NotifyLogContent(Name), () => { Logger.LogInformation("undo"); }));
            //#endif

            try {
                ILauncherExecuteResult result;
                switch(Kind) {
                    case LauncherItemKind.File:
                        result = ExecuteFile(null, screen);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Debug.Assert(result != null);

                IncrementExecuteCount();

                return result;

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult DirectExecute(string argument, IScreen screen)
        {
            ThrowIfDisposed();

            try {
                ILauncherExecuteResult result;
                switch(Kind) {
                    case LauncherItemKind.File:
                        result = ExecuteFile(argument, screen);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Debug.Assert(result != null);

                IncrementExecuteCount();

                return result;

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

        private void IncrementExecuteCount()
        {
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                dao.UpdateExecuteCountIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());
                commander.Commit();
            }
        }

        public void OpenExtendsExecuteView(IScreen screen)
        {
            ThrowIfDisposed();

            DispatcherWrapper.Begin(() => {
                var element = OrderManager.CreateLauncherExtendsExecuteElement(LauncherItemId, screen);
                element.StartView();
            });
        }

        public void OpenExtendsExecuteViewWidthArgument(string argument, IScreen screen)
        {
            ThrowIfDisposed();

            var element = OrderManager.CreateLauncherExtendsExecuteElement(LauncherItemId, screen);
            element.SetOption(argument);
            element.StartView();
        }

        ILauncherExecutePathParameter GetExecutePath()
        {
            Debug.Assert(Kind == LauncherItemKind.File);
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
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

        public void OpenCustomizeView(IScreen screen)
        {
            ThrowIfDisposed();

            if(NowCustomizing) {
                Logger.LogWarning("現在編集中: {0}", LauncherItemId);
                //OrderManager.FlashCustomizeLauncherItem(LauncherItemId);
                //WindowManager.Flash()
                var items = WindowManager.GetWindowItems(WindowKind.LauncherCustomize);
                var item = items.FirstOrDefault(i => ((ILauncherItemId)i.ViewModel).LauncherItemId == LauncherItemId);
                if(item != null) {
                    WindowManager.Flash(item);
                }
                return;
            }

            //TODO: 確定時の処理
            NowCustomizing = true;
            NotifyManager.CustomizeLauncherItemExited += NotifyManager_CustomizeLauncherItemExited;
            var element = OrderManager.CreateCustomizeLauncherItemContainerElement(LauncherItemId, screen);
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
            return new LauncherIconFactory(LauncherItemId, Kind, LauncherItemAddonFinder, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
        }

        #endregion

        #region ElementBase

        override protected void InitializeImpl()
        {
            LoadLauncherItem();
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

        public Guid LauncherItemId { get; }

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
