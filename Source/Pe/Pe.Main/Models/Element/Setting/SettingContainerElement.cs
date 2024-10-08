using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
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
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting.Factory;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingContainerElement: ServiceLocatorElementBase
    {
        #region variable

        private bool _isVisible;

        #endregion

        public SettingContainerElement(IDiContainer diContainer, PauseReceiveLogDelegate pauseReceiveLog, ISettingElementFactory settingElementFactory, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            PluginContainer = ServiceLocator.Build<PluginContainer>();

            GeneralsSettingEditor = settingElementFactory.CreateGeneralsSettingEditorElement(PluginContainer.Theme.Plugins);
            LauncherItemsSettingEditor = settingElementFactory.CreateLauncherItemsSettingEditorElement(AllLauncherItems, PluginContainer);
            LauncherGroupsSettingEditor = settingElementFactory.CreateLauncherGroupsSettingEditorElement(AllLauncherGroups);
            LauncherToolbarsSettingEditor = settingElementFactory.CreateLauncherToolbarsSettingEditorElement(AllLauncherGroups);
            KeyboardSettingEditor = settingElementFactory.CreateKeyboardSettingEditorElement();
            PluginsSettingEditor = settingElementFactory.CreatePluginsSettingEditorElement(PluginContainer, pauseReceiveLog);

            Editors = new SettingEditorElementBase[] {
                GeneralsSettingEditor,
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
                LauncherToolbarsSettingEditor,
                KeyboardSettingEditor,
                PluginsSettingEditor
            };
        }

        #region property

        /// <summary>
        ///編集中のランチャーアイテム一覧。
        /// </summary>
        /// <remarks>
        /// <para>みんなで共有する。</para>
        /// </remarks>
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
        public LauncherToolbarsSettingEditorElement LauncherToolbarsSettingEditor { get; }
        public KeyboardSettingEditorElement KeyboardSettingEditor { get; }
        public PluginsSettingEditorElement PluginsSettingEditor { get; }
        public IReadOnlyList<SettingEditorElementBase> Editors { get; }

        private PluginContainer PluginContainer { get; }

        #endregion

        #region function

        public void Save()
        {
            var pack = PersistenceHelper.WaitWritePack(
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

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
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
            //launcherItemElements.Sort((a,b) => {
            //    if(a.Kind == LauncherItemKind.Separator
            //})


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
                await element.InitializeAsync(cancellationToken);
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
                await element.InitializeAsync(cancellationToken);
            }
            AllLauncherGroups.SetRange(launcherGroupElements);

            foreach(var editor in Editors) {
                await editor.InitializeAsync(cancellationToken);
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
