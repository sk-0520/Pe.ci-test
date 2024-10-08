using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput
{
    public class StandardInputOutputElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        private bool _isVisible;
        private bool _preparedReceive;
        private bool _processExited;

        #endregion

        public StandardInputOutputElement(string captionName, Process process, IScreen screen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if(!process.EnableRaisingEvents) {
                throw new ArgumentException(null, $"{nameof(process)}.{process.EnableRaisingEvents}");
            }

            CaptionName = captionName;
            Process = process;
            Screen = screen;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;

            Process.Exited += Process_Exited;
        }

        #region property

        public string CaptionName { get; }
        public Process Process { get; }
        private IScreen Screen { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IOrderManager OrderManager { get; }

        public StreamReceiver? OutputStreamReceiver { get; private set; }
        public StreamReceiver? ErrorStreamReceiver { get; private set; }

        bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        public bool PreparedReceive
        {
            get => this._preparedReceive;
            private set => SetProperty(ref this._preparedReceive, value);
        }

        public bool ProcessExited
        {
            get => this._processExited;
            private set => SetProperty(ref this._processExited, value);
        }

        public FontElement? Font { get; private set; }
        public Color OutputForegroundColor { get; private set; }
        public Color OutputBackgroundColor { get; private set; }
        public Color ErrorForegroundColor { get; private set; }
        public Color ErrorBackgroundColor { get; private set; }

        public bool IsTopmost { get; set; }

        #endregion

        #region function

        public void PreparateReceiver()
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, exit code = {1}, exit time = {2}", Process.Id, Process.ExitCode, Process.ExitTime);
                return;
            }

            OutputStreamReceiver = new StreamReceiver(Process.StandardOutput, LoggerFactory);
            ErrorStreamReceiver = new StreamReceiver(Process.StandardError, LoggerFactory);
            //ProcessStandardOutputReceiver = new ProcessStandardOutputReceiver(Process, LoggerFactory);

            PreparedReceive = true;
        }

        public void RunReceiver()
        {
            Debug.Assert(PreparedReceive);
            ThrowIfDisposed();

            OutputStreamReceiver!.StartReceive();
            ErrorStreamReceiver!.StartReceive();
            //ProcessStandardOutputReceiver!.StartReceive();
        }

        public void Kill()
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, name = {1}, exit coe = {2}, exit time = {3}", Process.Id, Process.ProcessName, Process.ExitCode, Process.ExitTime);
                return;
            }

            Process.Kill();
        }

        public void SendInputValue(string value)
        {
            ThrowIfDisposed();

            if(Process.HasExited) {
                Logger.LogWarning("既に終了したプロセス: id = {0}, name = {1}, exit coe = {2}, exit time = {3}", Process.Id, Process.ProcessName, Process.ExitCode, Process.ExitTime);
                return;
            }

            Process.StandardInput.Write(value);
        }

        #endregion

        #region ElementBase

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            SettingAppStandardInputOutputSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appStandardInputOutputSettingEntityDao.SelectSettingStandardInputOutputSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await Font.InitializeAsync(cancellationToken);

            OutputForegroundColor = setting.OutputForegroundColor;
            OutputBackgroundColor = setting.OutputBackgroundColor;
            ErrorForegroundColor = setting.ErrorForegroundColor;
            ErrorBackgroundColor = setting.ErrorBackgroundColor;

            IsTopmost = setting.IsTopmost;
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
            var windowItem = OrderManager.CreateStandardInputOutputWindow(this);
            windowItem.Window.Show();
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            if(!ProcessExited) {
                Logger.LogWarning("実行中のプロセス: id = {0}, name = {1}", Process.Id, Process.ProcessName);
                return false;
            }

            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool, CancellationToken)"/>
        public Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            ViewCreated = false;
            return Task.CompletedTask;
        }

        #endregion

        private void Process_Exited(object? sender, EventArgs e)
        {
            ProcessExited = true;
            OutputStreamReceiver?.Dispose();
            ErrorStreamReceiver?.Dispose();
        }
    }
}
