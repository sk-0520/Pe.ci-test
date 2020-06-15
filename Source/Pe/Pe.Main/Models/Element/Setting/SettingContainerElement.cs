using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Views.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingContainerElement: ContextElementBase
    {
        #region variable

        bool _isVisible;

        #endregion

        public SettingContainerElement(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            GeneralsSettingEditor = ServiceLocator.Build<GeneralsSettingEditorElement>();
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorElement(AllLauncherItems, ServiceLocator.Build<PluginContainer>(), ServiceLocator.Build<ISettingNotifyManager>(), ServiceLocator.Build<IClipboardManager>(), ServiceLocator.Build<IMainDatabaseBarrier>(), ServiceLocator.Build<IFileDatabaseBarrier>(), ServiceLocator.Build<IDatabaseStatementLoader>(), ServiceLocator.Build<IIdFactory>(), ServiceLocator.Build<IDispatcherWrapper>(), ServiceLocator.Build<ILoggerFactory>());
            LauncherGroupsSettingEditor = ServiceLocator.Build<LauncherGroupsSettingEditorElement>(AllLauncherGroups);
            LauncherToobarsSettingEditor = ServiceLocator.Build<LauncherToobarsSettingEditorElement>(AllLauncherGroups);
            KeyboardSettingEditor = ServiceLocator.Build<KeyboardSettingEditorElement>();
            PluginsSettingEditor = new PluginsSettingEditorElement(ServiceLocator.Build<PluginContainer>(), ServiceLocator.Build<PreferencesContextFactory>(), ServiceLocator.Build<ISettingNotifyManager>(), ServiceLocator.Build<IClipboardManager>(), ServiceLocator.Build<IMainDatabaseBarrier>(), ServiceLocator.Build<IFileDatabaseBarrier>(), ServiceLocator.Build<IDatabaseStatementLoader>(), ServiceLocator.Build<IIdFactory>(), ServiceLocator.Build<EnvironmentParameters>(), ServiceLocator.Build<IUserAgentManager>(), ServiceLocator.Build<IPlatformTheme>(), ServiceLocator.Build<IDispatcherWrapper>(), ServiceLocator.Build<ILoggerFactory>());

            PluginContainer = ServiceLocator.Build<PluginContainer>();

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

        PluginContainer PluginContainer { get; }

        #endregion

        #region function

        public void Save()
        {
            var mainDatabaseBarrier = ServiceLocator.Get<IMainDatabaseBarrier>();
            var mainDatabaseCommander = mainDatabaseBarrier.WaitWrite();

            var fileDatabaseBarrier = ServiceLocator.Get<IFileDatabaseBarrier>();
            var fileDatabaseCommander = fileDatabaseBarrier.WaitWrite();

            var tempDatabaseBarrier = ServiceLocator.Get<ITemporaryDatabaseBarrier>();
            var tempDatabaseCommander = tempDatabaseBarrier.WaitWrite();

            var pack = new ApplicationDatabaseCommandsPack(
                new DatabaseCommands(mainDatabaseCommander, mainDatabaseCommander.Implementation),
                new DatabaseCommands(fileDatabaseCommander, fileDatabaseCommander.Implementation),
                new DatabaseCommands(tempDatabaseCommander, tempDatabaseCommander.Implementation),
                DatabaseCommonStatus.CreateCurrentAccount()
            );

            using(mainDatabaseCommander)
            using(fileDatabaseCommander)
            using(tempDatabaseCommander) {
                foreach(var editor in Editors) {
                    if(editor.IsLoaded) {
                        editor.Save(pack);
                    }
                }

                tempDatabaseCommander.Commit();
                fileDatabaseCommander.Commit();
                mainDatabaseCommander.Commit();
            }

            IsSubmit = true;
        }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            IReadOnlyList<Guid> launcherItemIds;
            IReadOnlyList<Guid> groupIds;
            using(var commander = ServiceLocator.Get<IMainDatabaseBarrier>().WaitRead()) {
                var launcherItemsEntityDao = ServiceLocator.Build<LauncherItemsEntityDao>(commander, commander.Implementation);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();

                var launcherGroupsEntityDao = ServiceLocator.Build<LauncherGroupsEntityDao>(commander, commander.Implementation);
                groupIds = launcherGroupsEntityDao.SelectAllLauncherGroupIds().ToList();
            }


            var launcherItemElements = new List<LauncherItemSettingEditorElement>(launcherItemIds.Count);
            foreach(var launcherItemId in launcherItemIds) {
                var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, ServiceLocator.Get<IMainDatabaseBarrier>(), ServiceLocator.Get<IFileDatabaseBarrier>(), ServiceLocator.Get<IDatabaseStatementLoader>(), ServiceLocator.Get<IDispatcherWrapper>(), LoggerFactory);
                var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
                launcherIconElement.Initialize();
                var element = ServiceLocator.Build<LauncherItemSettingEditorElement>(launcherItemId, launcherIconElement);
                launcherItemElements.Add(element);
            }
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
