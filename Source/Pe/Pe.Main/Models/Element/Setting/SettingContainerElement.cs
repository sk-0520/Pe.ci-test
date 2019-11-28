using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
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
            LauncherItemsSettingEditor = ServiceLocator.Build<LauncherItemsSettingEditorElement>();
            LauncherGroupsSettingEditor = ServiceLocator.Build<LauncherGroupsSettingEditorElement>();
        }

        #region property

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool IsSubmit { get; private set; }

        public LauncherItemsSettingEditorElement LauncherItemsSettingEditor { get; }
        public LauncherGroupsSettingEditorElement LauncherGroupsSettingEditor { get; }
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
            LauncherItemsSettingEditor.Initialize();
            LauncherGroupsSettingEditor.Initialize();
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
