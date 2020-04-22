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
    public class LauncherItemCustomizeRedoViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        string _successExitCodes = string.Empty;

        #endregion

        public LauncherItemCustomizeRedoViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            Redo = Model.Redo ?? throw new InvalidOperationException(nameof(Model.Redo));
        }

        #region property

        LauncherRedoData Redo { get; }

        public RedoWait RedoWait
        {
            get => Redo.RedoWait;
            set => SetPropertyValue(Redo, value);
        }
        public int WaitTimeSeconds
        {
            get => (int)Redo.WaitTime.TotalSeconds;
            set => SetPropertyValue(Redo, TimeSpan.FromSeconds(value), nameof(Redo.WaitTime));
        }
        public int RetryCount
        {
            get => Redo.RetryCount;
            set => SetPropertyValue(Redo, value);
        }
        public string SuccessExitCodes
        {
            get => this._successExitCodes;
            set => SetProperty(ref this._successExitCodes, value);
        }

        public int MinimumWaitTimeSeconds => 1;
        public int MaximumWaitTimeSeconds => 300;
        public int MinimumRetryCount => 1;
        public int MaximumRetryCount => 100;

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void InitializeImpl()
        {
            var numericRange = new NumericRange();
            SuccessExitCodes = numericRange.ToString(Redo.SuccessExitCodes);
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            var numericRange = new NumericRange();
            if(numericRange.TryParse(SuccessExitCodes, out var values)) {
                Redo.SuccessExitCodes.SetRange(values);
            }
        }

        #endregion
    }
}
