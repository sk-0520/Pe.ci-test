using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeRedoViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        RedoWait _redoWait;
        int _waitTimeSeconds;
        int _retryCount;
        string _successExitCodes = string.Empty;

        #endregion

        public LauncherItemCustomizeRedoViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public RedoWait RedoWait
        {
            get => this._redoWait;
            set => SetProperty(ref this._redoWait, value);
        }
        public int WaitTimeSeconds
        {
            get => this._waitTimeSeconds;
            set => SetProperty(ref this._waitTimeSeconds, value);
        }
        public int RetryCount
        {
            get => this._retryCount;
            set => SetProperty(ref this._retryCount, value);
        }
        public string SuccessExitCodes
        {
            get => this._successExitCodes;
            set => SetProperty(ref this._successExitCodes, value);
        }

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void InitializeImpl()
        {
            Debug.Assert(Model.Redo != null);

            var numericRange = new NumericRange();

            RedoWait = Model.Redo.RedoWait;
            WaitTimeSeconds = (int)Model.Redo.WaitTime.TotalSeconds;
            RetryCount = Model.Redo.RetryCount;
            SuccessExitCodes = numericRange.ToString(Model.Redo.SuccessExitCodes);
        }

        #endregion
    }
}
