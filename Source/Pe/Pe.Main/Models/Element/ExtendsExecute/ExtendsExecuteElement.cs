using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute
{
    public class ExtendsExecuteElement : ElementBase
    {
        #region variable

        bool _isVisible;

        #endregion

        public ExtendsExecuteElement(LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherFileData = launcherFileData;
            EnvironmentVariables = launcherEnvironmentVariables;
            Screen = screen;

            OrderManager = orderManager;
        }

        #region property

        protected LauncherFileData LauncherFileData { get; set; }
        protected IReadOnlyList<LauncherEnvironmentVariableData> EnvironmentVariables { get; set; }
        protected Screen Screen { get; }

        IOrderManager OrderManager { get; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

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
            var windowItem = OrderManager.CreateExtendsExecuteWindow(this);
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

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }

    public sealed class LauncherExtendsExecuteElement : ExtendsExecuteElement, ILauncherItemId
    {
        public LauncherExtendsExecuteElement(Guid launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, Screen screen, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(new LauncherFileData(), new List<LauncherEnvironmentVariableData>(), screen, orderManager, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
