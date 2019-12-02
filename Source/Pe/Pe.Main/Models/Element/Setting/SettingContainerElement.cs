using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Views.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingContainerElement : ContextElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region event

        public event EventHandler? Closed;

        #endregion

        #region variable

        bool _isVisible;

        #endregion

        public SettingContainerElement(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            LauncherItemsSettingEditor = ServiceLocator.Build<LauncherItemsSettingEditorElement>(AllLauncherItems);
            LauncherGroupsSettingEditor = ServiceLocator.Build<LauncherGroupsSettingEditorElement>();
            KeyboardSettingEditor = ServiceLocator.Build<KeyboardSettingEditorElement>();
        }

        #region property

        /// <summary>
        ///編集中のランチャーアイテム一覧。
        ///<para>みんなで共有する。</para>
        /// </summary>
        public ObservableCollection<LauncherItemSettingEditorElement> AllLauncherItems { get; } = new ObservableCollection<LauncherItemSettingEditorElement>();

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool IsSubmit { get; private set; }

        public LauncherItemsSettingEditorElement LauncherItemsSettingEditor { get; }
        public LauncherGroupsSettingEditorElement LauncherGroupsSettingEditor { get; }
        public KeyboardSettingEditorElement KeyboardSettingEditor { get; }
        #endregion

        #region function

        public void SetSubmit(bool value)
        {
            IsSubmit = value;
        }


        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            IReadOnlyList<Guid> launcherItemIds;
            using(var commander = ServiceLocator.Get<IMainDatabaseBarrier>().WaitRead()) {
                var launcherItemsEntityDao = ServiceLocator.Build<LauncherItemsEntityDao>(commander, commander.Implementation);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }
            var elements = new List<LauncherItemSettingEditorElement>(launcherItemIds.Count);
            foreach(var launcherItemId in launcherItemIds) {
                var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, ServiceLocator.Get<IMainDatabaseBarrier>(), ServiceLocator.Get<IFileDatabaseBarrier>(), ServiceLocator.Get<IDatabaseStatementLoader>(), ServiceLocator.Get<IDispatcherWrapper>(), LoggerFactory);
                var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
                launcherIconElement.Initialize();
                var element = ServiceLocator.Build<LauncherItemSettingEditorElement>(launcherItemId, launcherIconElement);
                elements.Add(element);
            }
            foreach(var element in elements) {
                element.Initialize();
            }
            AllLauncherItems.SetRange(elements);

            LauncherItemsSettingEditor.Initialize();
            LauncherGroupsSettingEditor.Initialize();
            KeyboardSettingEditor.Initialize();
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return IsVisible;
            }
        }

        public void StartView()
        {
            var windowItem = ServiceLocator.Get<IOrderManager>().CreateSettingWindow(this);
            windowItem.Window.Show();

            ViewCreated = true;
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

        public void ReceiveViewClosed()
        {
            ViewCreated = false;

            Closed?.Invoke(this, new EventArgs());
        }


        #endregion

    }
}
