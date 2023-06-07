using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemsSettingEditorElement: SettingEditorElementBase
    {
        internal LauncherItemsSettingEditorElement(ObservableCollection<LauncherItemSettingEditorElement> allLauncherItems, PluginContainer pluginContainer, LauncherItemAddonContextFactory launcherItemAddonContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
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

        public LauncherItemId AddNewItem(LauncherItemKind kind, PluginId pluginId)
        {
            ThrowIfDisposed();

            var newLauncherItemId = IdFactory.CreateLauncherItemId();
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                //TODO: 名前の自動設定
                var item = new LauncherItemData() {
                    LauncherItemId = newLauncherItemId,
                    Kind = kind,
                    Name = TextUtility.ToUnique(Properties.Resources.String_LauncherItem_NewItem_Name, AllLauncherItems.Select(i => i.Name).ToList(), StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}({n})"),
                };

                var newCode = kind.ToString().ToLowerInvariant() + "-item-code";
                var codes = launcherItemsDao.SelectFuzzyCodes(newCode).ToList();
                item.Code = launcherFactory.GetUniqueCode(newCode, codes);
                item.IsEnabledCommandLauncher = true;

                launcherItemsDao.InsertLauncherItem(item, DatabaseCommonStatus.CreateCurrentAccount());

                switch(kind) {
                    case LauncherItemKind.File: {
                            Debug.Assert(pluginId == PluginId.Empty);

                            var launcherFilesDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            var file = new LauncherFileData();
                            launcherFilesDao.InsertFile(item.LauncherItemId, file, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherRedoItemsEntityDao.InsertRedoItem(item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.StoreApp: {
                            Debug.Assert(pluginId == PluginId.Empty);

                            var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            var store = new LauncherStoreAppData();
                            launcherStoreAppsEntityDao.InsertStoreApp(item.LauncherItemId, store, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.Addon: {
                            Debug.Assert(pluginId != PluginId.Empty);

                            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                            launcherAddonsEntityDao.InsertAddonPluginId(item.LauncherItemId, pluginId, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }

                context.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(newLauncherItemId, new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory), LauncherItemAddonContextFactory, ClipboardManager, MainDatabaseBarrier, LargeDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            AllLauncherItems.Add(customizeEditor);

            return newLauncherItemId;
        }

        /// <summary>
        /// ファイルを登録する。
        /// <para>TODO: <see cref="LauncherToolbar.LauncherToolbarElement.RegisterFile"/>と重複</para>
        /// </summary>
        /// <param name="filePath">対象ファイルパス。</param>
        /// <param name="expandShortcut"><paramref name="filePath"/>がショートカットの場合にショートカットの内容を登録するか</param>
        public LauncherItemId RegisterFile(string filePath, bool expandShortcut)
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

                var codes = launcherItemsDao.SelectFuzzyCodes(data.Item.Code).ToList();
                data.Item.Code = launcherFactory.GetUniqueCode(data.Item.Code, codes);

                launcherItemsDao.InsertLauncherItem(data.Item, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesDao.InsertFile(data.Item.LauncherItemId, data.File, DatabaseCommonStatus.CreateCurrentAccount());
                launcherTagsDao.InsertTags(data.Item.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());
                launcherRedoItemsEntityDao.InsertRedoItem(data.Item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(data.Item.LauncherItemId, new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory), LauncherItemAddonContextFactory, ClipboardManager, MainDatabaseBarrier, LargeDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            AllLauncherItems.Add(customizeEditor);

            return data.Item.LauncherItemId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
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
