using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute
{
    public class ExtendsExecuteElement : ElementBase
    {
        #region variable

        bool _isVisible;

        #endregion

        public ExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            CaptionName = captionName;
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

        public string CaptionName { get; protected set; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            // 独立した何かなのでここでは何もしない
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
        public LauncherExtendsExecuteElement(Guid launcherItemId, Screen screen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(string.Empty, new LauncherFileData(), new List<LauncherEnvironmentVariableData>(), screen, orderManager, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                var launcherItem = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                var fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                var envItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId);

                LauncherFileData = fileData;
                EnvironmentVariables = envItems.ToList();

                CaptionName = launcherItem.Name ?? launcherItem.Code ?? Path.GetFileNameWithoutExtension(LauncherFileData.Path) ?? LauncherItemId.ToString("D");
            }
        }

        #endregion

    }
}
