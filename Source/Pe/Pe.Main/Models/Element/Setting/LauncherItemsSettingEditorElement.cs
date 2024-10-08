using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemsSettingEditorElement: SettingEditorElementBase
    {
        public LauncherItemsSettingEditorElement(ObservableCollection<LauncherItemSettingEditorElement> allLauncherItems, PluginContainer pluginContainer, LauncherItemAddonContextFactory launcherItemAddonContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            AllLauncherItems = allLauncherItems;
            PluginContainer = pluginContainer;

            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;

            var addons = new List<IAddon>();
            var addonIds = pluginContainer.Addon.GetLauncherItemAddonIds();
            var addonPlugins = pluginContainer.Plugins.OfType<IAddon>().ToList();
            foreach(var addonId in addonIds) {
                var addon = addonPlugins.FirstOrDefault(i => i.PluginInformation.PluginIdentifiers.PluginId == addonId);
                if(addon != null) {
                    addons.Add(addon);
                } else {
                    Logger.LogWarning("アドオンプラグインが存在しない: {0}", addonId);
                }
            }
            Addons = addons;
        }

        #region property

        public ObservableCollection<LauncherItemSettingEditorElement> AllLauncherItems { get; }
        PluginContainer PluginContainer { get; }

        public IReadOnlyList<IAddon> Addons { get; }
        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        #endregion

        #region function

        public void RemoveItem(LauncherItemId launcherItemId)
        {
            ThrowIfDisposed();

            var item = AllLauncherItems.First(i => i.LauncherItemId == launcherItemId);
            var launcherItemKind = item.Kind;
            AllLauncherItems.Remove(item);
            item.Dispose();

            // DBから物理削除
            using(var mainDatabaseContext = MainDatabaseBarrier.WaitWrite())
            using(var largeDatabaseContext = LargeDatabaseBarrier.WaitWrite())
            using(var temporaryDatabaseContext = TemporaryDatabaseBarrier.WaitWrite()) {
                var launcherEntityEraser = new LauncherEntityEraser(launcherItemId, launcherItemKind, mainDatabaseContext, largeDatabaseContext, temporaryDatabaseContext, DatabaseStatementLoader, LoggerFactory);
                launcherEntityEraser.Execute();

                temporaryDatabaseContext.Commit();
                largeDatabaseContext.Commit();
                mainDatabaseContext.Commit();
            }

            SettingNotifyManager.SendLauncherItemRemove(launcherItemId);
        }

        public async Task<LauncherItemId> AddNewItemAsync(LauncherItemKind kind, LauncherSeparatorKind launcherSeparatorKind, PluginId pluginId, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var newLauncherItemId = IdFactory.CreateLauncherItemId();
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var launcherItemManager = new LauncherItemManager();

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                var item = new LauncherItemData() {
                    LauncherItemId = newLauncherItemId,
                    Kind = kind,
                    Name = launcherItemManager.CreateNewName(kind, AllLauncherItems.Select(i => i.Name).ToList()),
                };

                item.IsEnabledCommandLauncher = true;
                switch(kind) {
                    case LauncherItemKind.File:
                    case LauncherItemKind.Addon:
                        break;

                    case LauncherItemKind.Separator:
                        item.IsEnabledCommandLauncher = false;
                        break;

                    default:
                        throw new NotImplementedException();
                }
                launcherItemsDao.InsertLauncherItem(item, DatabaseCommonStatus.CreateCurrentAccount());

                switch(kind) {
                    case LauncherItemKind.File: {
                            Debug.Assert(pluginId == PluginId.Empty);
                            Debug.Assert(launcherSeparatorKind == LauncherSeparatorKind.None);

                            var launcherFilesDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            var file = new LauncherFileData();
                            launcherFilesDao.InsertFile(item.LauncherItemId, file, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherRedoItemsEntityDao.InsertRedoItem(item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.StoreApp: {
                            Debug.Assert(pluginId == PluginId.Empty);
                            Debug.Assert(launcherSeparatorKind == LauncherSeparatorKind.None);

                            var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            var store = new LauncherStoreAppData();
                            launcherStoreAppsEntityDao.InsertStoreApp(item.LauncherItemId, store, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.Addon: {
                            Debug.Assert(pluginId != PluginId.Empty);
                            Debug.Assert(launcherSeparatorKind == LauncherSeparatorKind.None);

                            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            launcherAddonsEntityDao.InsertAddonPluginId(item.LauncherItemId, pluginId, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.Separator: {
                            Debug.Assert(pluginId == PluginId.Empty);

                            var launcherSeparatorsEntityDao = new LauncherSeparatorsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            var data = new LauncherSeparatorData() {
                                Kind = launcherSeparatorKind,
                                Width = 1,
                            };
                            launcherSeparatorsEntityDao.InsertSeparator(item.LauncherItemId, data, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }

                context.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(newLauncherItemId, new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory), LauncherItemAddonContextFactory, ClipboardManager, MainDatabaseBarrier, LargeDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await customizeEditor.InitializeAsync(cancellationToken);

            AllLauncherItems.Add(customizeEditor);

            return newLauncherItemId;
        }

        /// <summary>
        /// ファイルを登録する。
        /// </summary>
        /// <remarks>
        /// <para>TODO: <see cref="LauncherToolbar.LauncherToolbarElement.RegisterFile"/>と重複</para>
        /// </remarks>
        /// <param name="filePath">対象ファイルパス。</param>
        /// <param name="expandShortcut"><paramref name="filePath"/>がショートカットの場合にショートカットの内容を登録するか</param>
        public async Task<LauncherItemId> RegisterFileAsync(string filePath, bool expandShortcut, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var file = new FileInfo(filePath);
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var data = launcherFactory.FromFile(file, expandShortcut);
            var tags = launcherFactory.GetTags(file).ToList();

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherTagsDao = new LauncherTagsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                launcherItemsDao.InsertLauncherItem(data.Item, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesDao.InsertFile(data.Item.LauncherItemId, data.File, DatabaseCommonStatus.CreateCurrentAccount());
                launcherTagsDao.InsertTags(data.Item.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());
                launcherRedoItemsEntityDao.InsertRedoItem(data.Item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(data.Item.LauncherItemId, new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory), LauncherItemAddonContextFactory, ClipboardManager, MainDatabaseBarrier, LargeDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await customizeEditor.InitializeAsync(cancellationToken);

            AllLauncherItems.Add(customizeEditor);

            return data.Item.LauncherItemId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override Task LoadCoreAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            /* なにしたかったんだこれ
            IReadOnlyList<Guid> launcherItemIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }
            */

            //Items.Clear();
            //foreach(var launcherItemId in launcherItemIds) {
            //    var customizeEditor = new LauncherItemCustomizeEditorElement(launcherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            //    customizeEditor.Initialize();

            //    var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
            //    var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
            //    launcherIconElement.Initialize();

            //    var item = LauncherItemWithIconElement.Create(customizeEditor, launcherIconElement, LoggerFactory);
            //    Items.Add(item);
            //}

            return Task.CompletedTask;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            foreach(var item in AllLauncherItems.Where(i => !i.IsLazyLoad)) {
                var needIconClear = item.SaveItem(contextsPack);
                if(needIconClear) {
                    item.ClearIcon(contextsPack.Large.Context, contextsPack.Large.Implementation);
                }
            }
        }

        #endregion
    }
}
