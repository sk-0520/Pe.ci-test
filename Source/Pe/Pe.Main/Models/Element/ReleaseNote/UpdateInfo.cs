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

        #endregion
    }

    public class UpdateInfo : BindModelBase, IReadOnlyUpdateInfo
    {
        #region variable

        UpdateState _state;
        #endregion

        public UpdateInfo(ILoggerFactory loggerFactory) : base(loggerFactory)
        { }

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

        #endregion
    }
}
