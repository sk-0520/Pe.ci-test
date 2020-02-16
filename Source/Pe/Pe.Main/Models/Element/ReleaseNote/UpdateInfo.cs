using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public interface IReadOnlyUpdateInfo
    {
        #region property

        /// <summary>
        /// アップデート準備完了。
        /// </summary>
        bool IsReady { get; }
        UpdateState State { get; }

        IReadOnlyUpdateItemData? UpdateItem { get; }

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

    public class UpdateInfo : BindModelBase, IReadOnlyUpdateInfo
    {
        #region variable

        UpdateState _state;

        string _currentLog = string.Empty;

        double _checksumValue;
        double _downloadedValue;
        double _extractedValue;

        #endregion

        public UpdateInfo(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            CurrentLogProgress = new Progress<string>(v => CurrentLog = v);
            ChecksumProgress = new Progress<double>(v => ChecksumValue = v);
            DownloadProgress = new Progress<double>(v => DownloadedValue = v);
            ExtractProgress = new Progress<double>(v => ExtractedValue = v);
        }

        #region property

        /// <summary>
        /// アップデート処理実施ファイル（*.bat とか *.exe とか）。
        /// <para>batかなぁ。</para>
        /// </summary>
        public ILauncherExecutePathParameter? Path { get; set; }

        #endregion

        #region IReadOnlyUpdateInfo

        /// <summary>
        /// アップデート準備完了。
        /// </summary>
        public bool IsReady => State == UpdateState.Ready;
        public UpdateState State
        {
            get => this._state;
            set
            {
                SetProperty(ref this._state, value);
                RaisePropertyChanged(nameof(IsReady));
            }
        }

        public UpdateItemData? UpdateItem { get; set; }
        IReadOnlyUpdateItemData? IReadOnlyUpdateInfo.UpdateItem => UpdateItem;

        public Progress<string> CurrentLogProgress { get; }
        IProgress<string> IReadOnlyUpdateInfo.CurrentLogProgress => CurrentLogProgress;
        public string CurrentLog
        {
            get => this._currentLog;
            private set => SetProperty(ref this._currentLog, value);
        }


        public Progress<double> ChecksumProgress { get; }
        IProgress<double> IReadOnlyUpdateInfo.ChecksumProgress => ChecksumProgress;
        public Progress<double> DownloadProgress { get; }
        IProgress<double> IReadOnlyUpdateInfo.DownloadProgress => DownloadProgress;
        public IProgress<double> ExtractProgress { get; }
        IProgress<double> IReadOnlyUpdateInfo.ExtractProgress => ExtractProgress;


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
            State = UpdateState.Error;
            Logging(message);
        }

        #endregion
    }
}
