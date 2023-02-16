using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingContainerElement: ServiceLocatorElementBase
    {
        #region variable

        private bool _isVisible;

        #endregion

        public SettingContainerElement(IDiContainer diContainer, PauseReceiveLogDelegate pauseReceiveLog, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            PluginContainer = ServiceLocator.Build<PluginContainer>();

            GeneralsSettingEditor = ServiceLocator.Build<GeneralsSettingEditorElement>(PluginContainer.Theme.Plugins.ToList());
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorElement(AllLauncherItems, PluginContainer, ServiceLocator.Build<LauncherItemAddonContextFactory>(), ServiceLocator.Build<ISettingNotifyManager>(), ServiceLocator.Build<IClipboardManager>(), ServiceLocator.Build<IMainDatabaseBarrier>(), ServiceLocator.Build<ILargeDatabaseBarrier>(), ServiceLocator.Build<ITemporaryDatabaseBarrier>(), ServiceLocator.Build<IDatabaseStatementLoader>(), ServiceLocator.Build<IIdFactory>(), ServiceLocator.Build<IImageLoader>(), ServiceLocator.Build<IMediaConverter>(), ServiceLocator.Build<IPolicy>(), ServiceLocator.Build<IDispatcherWrapper>(), ServiceLocator.Build<ILoggerFactory>());
            LauncherGroupsSettingEditor = ServiceLocator.Build<LauncherGroupsSettingEditorElement>(AllLauncherGroups);
            LauncherToobarsSettingEditor = ServiceLocator.Build<LauncherToobarsSettingEditorElement>(AllLauncherGroups);
            KeyboardSettingEditor = ServiceLocator.Build<KeyboardSettingEditorElement>();
            PluginsSettingEditor = new PluginsSettingEditorElement(PluginContainer, ServiceLocator.Build<NewVersionChecker>(), ServiceLocator.Build<NewVersionDownloader>(), ServiceLocator.Build<PluginConstructorContext>(), pauseReceiveLog, ServiceLocator.Build<PreferencesContextFactory>(), ServiceLocator.Build<IWindowManager>(), ServiceLocator.Build<IUserTracker>(), ServiceLocator.Build<ISettingNotifyManager>(), ServiceLocator.Build<IClipboardManager>(), ServiceLocator.Build<IMainDatabaseBarrier>(), ServiceLocator.Build<ILargeDatabaseBarrier>(), ServiceLocator.Build<ITemporaryDatabaseBarrier>(), ServiceLocator.Build<IDatabaseStatementLoader>(), ServiceLocator.Build<IIdFactory>(), ServiceLocator.Build<EnvironmentParameters>(), ServiceLocator.Build<IUserAgentManager>(), ServiceLocator.Build<IPlatformTheme>(), ServiceLocator.Build<IImageLoader>(), ServiceLocator.Build<IMediaConverter>(), ServiceLocator.Build<IPolicy>(), ServiceLocator.Build<IDispatcherWrapper>(), ServiceLocator.Build<ILoggerFactory>());

            Editors = new SettingEditorElementBase[] {
                GeneralsSettingEditor,
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
                LauncherToobarsSettingEditor,
                KeyboardSettingEditor,
                PluginsSettingEditor
            };
        }

        #region property

        /// <summary>
        ///編集中のランチャーアイテム一覧。
        ///<para>みんなで共有する。</para>
        /// </summary>
        public ObservableCollection<LauncherItemSettingEditorElement> AllLauncherItems { get; } = new ObservableCollection<LauncherItemSettingEditorElement>();
        public ObservableCollection<LauncherGroupSettingEditorElement> AllLauncherGroups { get; } = new ObservableCollection<LauncherGroupSettingEditorElement>();

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool IsSubmit { get; private set; }

        public GeneralsSettingEditorElement GeneralsSettingEditor { get; }
        public LauncherItemsSettingEditorElement LauncherItemsSettingEditor { get; }
        public LauncherGroupsSettingEditorElement LauncherGroupsSettingEditor { get; }
        public LauncherToobarsSettingEditorElement LauncherToobarsSettingEditor { get; }
        public KeyboardSettingEditorElement KeyboardSettingEditor { get; }
        public PluginsSettingEditorElement PluginsSettingEditor { get; }
        public IReadOnlyList<SettingEditorElementBase> Editors { get; }

        private PluginContainer PluginContainer { get; }

        #endregion

        #region function

        public void Save()
        {
            var pack = PersistentHelper.WaitWritePack(
                ServiceLocator.Get<IMainDatabaseBarrier>(),
                ServiceLocator.Get<ILargeDatabaseBarrier>(),
                ServiceLocator.Get<ITemporaryDatabaseBarrier>(),
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            using(pack) {
                foreach(var editor in Editors) {
                    if(editor.IsLoaded) {
                        editor.Save(pack);
                    }
                }

                pack.Commit();
            }

            IsSubmit = true;
        }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            IReadOnlyList<LauncherItemId> launcherItemIds;
            IReadOnlyList<LauncherGroupId> groupIds;

            var appLauncherItemsMap = new Dictionary<LauncherItemId, LauncherSettingCommonData>();

            using(var context = ServiceLocator.Get<IMainDatabaseBarrier>().WaitRead()) {
                var launcherItemsEntityDao = ServiceLocator.Build<LauncherItemsEntityDao>(context, context.Implementation);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();

                var launcherGroupsEntityDao = ServiceLocator.Build<LauncherGroupsEntityDao>(context, context.Implementation);
                groupIds = launcherGroupsEntityDao.SelectAllLauncherGroupIds().ToList();

                var launcherTagsEntityDao = ServiceLocator.Build<LauncherTagsEntityDao>(context, context.Implementation);
                var allTagMap = launcherTagsEntityDao.SelectAllTags();

                foreach(var data in launcherItemsEntityDao.SelectApplicationLauncherItems()) {
                    var setting = new LauncherSettingCommonData() {
                        LauncherItemId = data.LauncherItemId,
                        Kind = data.Kind,
                        Code = data.Code,
                        Name = data.Name,
                        Icon = data.Icon,
                        IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                        Comment = data.Comment,
                    };
                    if(allTagMap.TryGetValue(setting.LauncherItemId, out var tags)) {
                        setting.Tags.SetRange(tags);
                    }
                    appLauncherItemsMap[data.LauncherItemId] = setting;
                }

                //var launcherItemDomainDao = ServiceLocator.Build<LauncherItemDomainDao>(commander, commander.Implementation);
            }

            var launcherItemElements = new List<LauncherItemSettingEditorElement>(launcherItemIds.Count);
            foreach(var launcherItemId in launcherItemIds) {
                var element = appLauncherItemsMap.TryGetValue(launcherItemId, out var setting)
                    ? ServiceLocator.Build<LauncherItemSettingEditorElement>(launcherItemId, setting)
                    : ServiceLocator.Build<LauncherItemSettingEditorElement>(launcherItemId)
                ;
                launcherItemElements.Add(element);
            }


            /* パラで回しても1秒くらいしか変わらんかった
            if(launcherItemElements.Count < 100) {
                foreach(var element in launcherItemElements) {
                    element.Initialize();
                }
            } else {
                Parallel.ForEach(launcherItemElements, element => element.Initialize());
            }
            */
            foreach(var element in launcherItemElements) {
                element.Initialize();
            }
            AllLauncherItems.SetRange(launcherItemElements);

            var launcherItemAddonIds = PluginContainer.Addon.GetLauncherItemAddonIds();
            foreach(var launcherItemAddonId in launcherItemAddonIds) {

            }

            var launcherGroupElements = new List<LauncherGroupSettingEditorElement>(groupIds.Count);
            foreach(var groupId in groupIds) {
                var element = ServiceLocator.Build<LauncherGroupSettingEditorElement>(groupId);
                launcherGroupElements.Add(element);
            }
            foreach(var element in launcherGroupElements) {
                element.Initialize();
            }
            AllLauncherGroups.SetRange(launcherGroupElements);

            foreach(var editor in Editors) {
                editor.Initialize();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in Editors) {
                        editor.Dispose();
                    }
                    foreach(var item in AllLauncherGroups) {
                        item.Dispose();
                    }
                    AllLauncherGroups.Clear();

                    foreach(var item in AllLauncherItems) {
                        item.Dispose();
                    }
                    AllLauncherItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
