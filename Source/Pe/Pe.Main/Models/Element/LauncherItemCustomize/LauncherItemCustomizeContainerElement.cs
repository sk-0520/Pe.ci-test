using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    /// <summary>
    /// 独立したランチャーアイテム編集処理。
    /// </summary>
    /// <remarks>
    /// <para>アイテムを単体編集する要素。</para>
    /// </remarks>
    sealed public class LauncherItemCustomizeContainerElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        private bool _isVisible;

        #endregion

        public LauncherItemCustomizeContainerElement(IScreen screen, LauncherItemCustomizeEditorElement editorElement, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = editorElement.LauncherItemId;
            Screen = screen;
            Editor = editorElement;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        public IScreen Screen { get; }
        public LauncherItemCustomizeEditorElement Editor { get; }

        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        private bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
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

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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

        public LauncherItemId LauncherItemId { get; }

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

        public Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            NotifyManager.SendCustomizeLauncherItemExited(LauncherItemId);

            ViewCreated = false;

            return Task.CompletedTask;
        }


        #endregion

    }
}
