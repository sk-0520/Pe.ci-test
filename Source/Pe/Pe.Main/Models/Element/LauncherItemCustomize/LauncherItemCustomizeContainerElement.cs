using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemCustomizeContainerElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;

        #endregion

        public LauncherItemCustomizeContainerElement(IScreen screen, LauncherItemCustomizeEditorElement editorElement, LauncherIconElement launcherIconElement, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = editorElement.LauncherItemId;
            Screen = screen;
            Editor = editorElement;
            Icon = launcherIconElement;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public IScreen Screen { get; }
        public LauncherItemCustomizeEditorElement Editor { get; }
        public LauncherIconElement Icon { get; }

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        protected bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            protected set => SetProperty(ref this._isVisible, value);
        }

        #endregion

        #region function

        public void Save()
        {
            Editor.Save();
            NotifyManager.SendLauncherItemChanged(LauncherItemId);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
        }

        #endregion

        #region ILauncherItemId
        public Guid LauncherItemId { get; }
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

        public virtual void StartView()
        {
            var windowItem = OrderManager.CreateCustomizeLauncherItemWindow(this);
            windowItem.Window.Show();

            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            IsVisible = false;
            return true;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        public virtual void ReceiveViewClosed()
        {
            NotifyManager.SendCustomizeLauncherItemExited(LauncherItemId);

            ViewCreated = false;
        }


        #endregion

    }
}
