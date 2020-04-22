using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute
{
    public class ExtendsExecuteElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;

        #endregion

        public ExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, LauncherRedoData launcherRedoData, IScreen screen, IOrderManager orderManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            CaptionName = captionName;
            LauncherFileData = launcherFileData;
            EnvironmentVariables = launcherEnvironmentVariables;
            LauncherRedoData = launcherRedoData;
            Screen = screen;

            OrderManager = orderManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        public LauncherFileData LauncherFileData { get; protected set; }
        public IReadOnlyList<LauncherEnvironmentVariableData> EnvironmentVariables { get; protected set; }
        public IReadOnlyList<LauncherHistoryData> HistoryOptions { get; protected set; } = new List<LauncherHistoryData>();
        public IReadOnlyList<LauncherHistoryData> HistoryWorkDirectories { get; protected set; } = new List<LauncherHistoryData>();

        public LauncherRedoData LauncherRedoData { get; protected set; }

        public IScreen Screen { get; }

        IOrderManager OrderManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public string CaptionName { get; protected set; }

        #endregion

        #region function

        public virtual ILauncherExecuteResult Execute(LauncherFileData fileData, IReadOnlyList<LauncherEnvironmentVariableData> environmentVariables, IScreen screen)
        {
            ThrowIfDisposed();

            try {
                var launcherExecutor = new LauncherExecutor(OrderManager, DispatcherWrapper, LoggerFactory);
                var result = launcherExecutor.Execute(LauncherItemKind.File, fileData, fileData, environmentVariables, screen);
                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

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
        public LauncherExtendsExecuteElement(Guid launcherItemId, IScreen screen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IOrderManager orderManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(string.Empty, new LauncherFileData(), new List<LauncherEnvironmentVariableData>(), new LauncherRedoData(), screen, orderManager, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        /// <summary>
        /// 後付けオプション設定。
        /// <para>null を許容しているけど設定未設定の判別のみに使用するため null は設定不可。</para>
        /// <para>設定には <see cref="SetOption(string)"/> を使用する。</para>
        /// </summary>
        public string? CustomOption { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="CustomOption"/>の設定処理。
        /// </summary>
        /// <param name="option"></param>
        public void SetOption(string option)
        {
            ThrowIfDisposed();

            CustomOption = option ?? throw new ArgumentNullException(nameof(option));
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ExtendsExecuteElement

        protected override void InitializeImpl()
        {
            LauncherItemData launcherItem;
            LauncherFileData fileData;
            IEnumerable<LauncherEnvironmentVariableData> envItems;
            IEnumerable<LauncherHistoryData> histories;
            LauncherRedoData launcherRedoData;

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                launcherItem = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                envItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId);
                histories = launcherItemHistoriesEntityDao.SelectHistories(LauncherItemId);

                if(launcherRedoItemsEntityDao.SelectExistsLauncherRedoItem(LauncherItemId)) {
                    launcherRedoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                    var values = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                    launcherRedoData.SuccessExitCodes.SetRange(values);
                } else {
                    launcherRedoData = LauncherRedoData.GetDisable();
                }
            }

            LauncherFileData = fileData;
            EnvironmentVariables = envItems.ToList();
            LauncherRedoData = launcherRedoData;
            CaptionName = launcherItem.Name; // ?? launcherItem.Code ?? Path.GetFileNameWithoutExtension(LauncherFileData.Path) ?? LauncherItemId.ToString("D");

            var histories2 = histories.ToList();
            HistoryOptions = histories2.Where(i => i.Kind == LauncherHistoryKind.Option).ToList();
            HistoryWorkDirectories = histories2.Where(i => i.Kind == LauncherHistoryKind.WorkDirectory).ToList();
        }

        public override ILauncherExecuteResult Execute(LauncherFileData fileData, IReadOnlyList<LauncherEnvironmentVariableData> environmentVariables, IScreen screen)
        {
            var result = base.Execute(fileData, environmentVariables, screen);

            if(result.Success) {
                using(var commander = MainDatabaseBarrier.WaitWrite()) {
                    var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                    launcherItemsEntityDao.UpdateExecuteCountIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());

                    var item = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                    if(item.Kind == LauncherItemKind.File) {
                        var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                        var launcherFileData = launcherFilesEntityDao.SelectFile(LauncherItemId);

                        launcherItemHistoriesEntityDao.DeleteHistory(LauncherItemId, LauncherHistoryKind.Option, fileData.Option);
                        if(launcherFileData.Option != fileData.Option) {
                            launcherItemHistoriesEntityDao.InsertHistory(LauncherItemId, LauncherHistoryKind.Option, fileData.Option, DatabaseCommonStatus.CreateCurrentAccount());
                        }

                        launcherItemHistoriesEntityDao.DeleteHistory(LauncherItemId, LauncherHistoryKind.WorkDirectory, fileData.WorkDirectoryPath);
                        if(launcherFileData.WorkDirectoryPath != fileData.WorkDirectoryPath) {
                            launcherItemHistoriesEntityDao.InsertHistory(LauncherItemId, LauncherHistoryKind.WorkDirectory, fileData.WorkDirectoryPath, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                    }

                    commander.Commit();
                }
            }

            return result;
        }

        #endregion

    }
}
