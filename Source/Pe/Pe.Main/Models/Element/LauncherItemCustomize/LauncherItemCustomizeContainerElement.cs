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
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemCustomizeContainerElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;

        #endregion

        public LauncherItemCustomizeContainerElement(IScreen screen, LauncherItemCustomizeEditorElement editorElement, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = editorElement.LauncherItemId;
            Screen = screen;
            Editor = editorElement;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        public IScreen Screen { get; }
        public LauncherItemCustomizeEditorElement Editor { get; }

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        protected bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            protected set => SetProperty(ref this._isVisible, value);
        }

        public string CaptionName => Editor.Name;

        #endregion

        #region function

        public void Save()
        {
            Editor.Save();
            OrderManager.RefreshLauncherItemElement(LauncherItemId);
            NotifyManager.SendLauncherItemChanged(LauncherItemId);
        }


        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Editor.Dispose();
                }
            }

            base.Dispose(disposing);
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

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public virtual void ReceiveViewClosed(bool isUserOperation)
        {
            NotifyManager.SendCustomizeLauncherItemExited(LauncherItemId);

            ViewCreated = false;
        }


        #endregion

    }
}
