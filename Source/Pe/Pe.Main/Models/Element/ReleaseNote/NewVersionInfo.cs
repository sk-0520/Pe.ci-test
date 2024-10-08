using System;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public interface IReadOnlyNewVersionInfo
    {
        #region property

        /// <summary>
        /// アップデート準備完了。
        /// </summary>
        bool IsReady { get; }
        bool Updating { get; }
        NewVersionState State { get; }

        IReadOnlyNewVersionItemData? NewVersion { get; }

        IProgress<string> CurrentLogProgress { get; }
        string CurrentLog { get; }

        IProgress<double> ChecksumProgress { get; }
        IProgress<double> DownloadProgress { get; }
        IProgress<double> ExtractProgress { get; }

        double ChecksumValue { get; }
        double DownloadedValue { get; }
        double ExtractedValue { get; }

        #endregion
    }

    public class NewVersionInfo: BindModelBase, IReadOnlyNewVersionInfo
    {
        #region variable

        private NewVersionState _state;

        private string _currentLog = string.Empty;

        private double _checksumValue;
        private double _downloadedValue;
        private double _extractedValue;

        #endregion

        public NewVersionInfo(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            CurrentLogProgress = new Progress<string>(v => CurrentLog = v);
            ChecksumProgress = new Progress<double>(v => ChecksumValue = v);
            DownloadProgress = new Progress<double>(v => DownloadedValue = v);
            ExtractProgress = new Progress<double>(v => ExtractedValue = v);
        }

        #region property

        /// <summary>
        /// アップデート処理実施ファイル（*.bat とか *.exe とか）。
        /// </summary>
        /// <remarks>
        /// <para>batかなぁ。</para>
        /// </remarks>
        public ILauncherExecutePathParameter? Path { get; set; }

        #endregion

        #region IReadOnlyUpdateInfo

        /// <summary>
        /// アップデート準備完了。
        /// </summary>
        public bool IsReady => State == NewVersionState.Ready;
        public bool Updating
        {
            get
            {
                var items = new[] {
                    NewVersionState.Checking,
                    NewVersionState.Downloading,
                    NewVersionState.Checksumming,
                    NewVersionState.Extracting,
                };
                return items.Any(i => i == State);
            }
        }

        public NewVersionState State
        {
            get => this._state;
            set
            {
                SetProperty(ref this._state, value);
                RaisePropertyChanged(nameof(IsReady));
                RaisePropertyChanged(nameof(Updating));
            }
        }

        public NewVersionItemData? NewVersionItem { get; set; }
        IReadOnlyNewVersionItemData? IReadOnlyNewVersionInfo.NewVersion => NewVersionItem;

        public Progress<string> CurrentLogProgress { get; }
        IProgress<string> IReadOnlyNewVersionInfo.CurrentLogProgress => CurrentLogProgress;
        public string CurrentLog
        {
            get => this._currentLog;
            [Unused(UnusedKinds.TwoWayBinding)]
            set => SetProperty(ref this._currentLog, value);
        }


        public Progress<double> ChecksumProgress { get; }
        IProgress<double> IReadOnlyNewVersionInfo.ChecksumProgress => ChecksumProgress;
        public Progress<double> DownloadProgress { get; }
        IProgress<double> IReadOnlyNewVersionInfo.DownloadProgress => DownloadProgress;
        public IProgress<double> ExtractProgress { get; }
        IProgress<double> IReadOnlyNewVersionInfo.ExtractProgress => ExtractProgress;


        public double ChecksumValue
        {
            get => this._checksumValue;
            private set => SetProperty(ref this._checksumValue, value);
        }

        public double DownloadedValue
        {
            get => this._downloadedValue;
            private set => SetProperty(ref this._downloadedValue, value);
        }

        public double ExtractedValue
        {
            get => this._extractedValue;
            private set => SetProperty(ref this._extractedValue, value);
        }

        #endregion

        #region function

        public void Logging(string message)
        {
            ((IProgress<string>)CurrentLogProgress).Report(message);
        }

        public void SetError(string message)
        {
            State = NewVersionState.Error;
            Logging(message);
        }

        #endregion
    }
}
