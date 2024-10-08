using System;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeRedoViewModel: LauncherItemCustomizeDetailViewModelBase, IFlushable
    {
        #region variable

        private string _successExitCodes = string.Empty;

        #endregion

        public LauncherItemCustomizeRedoViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            if(!Model.IsLazyLoad) {
                if(Model.Redo == null) {
                    throw new ArgumentNullException(nameof(model));
                }
            }
        }

        #region property

        private LauncherRedoData Redo => Model.Redo!;

        public RedoMode RedoMode
        {
            get => Redo.RedoMode;
            set => SetPropertyValue(Redo!, value);
        }
        public int WaitTimeSeconds
        {
            get => (int)Redo.WaitTime.TotalSeconds;
            set => SetPropertyValue(Redo!, TimeSpan.FromSeconds(value), nameof(Redo.WaitTime));
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
            if(Model.IsLazyLoad) {
                return;
            }

            var numericRange = new NumericRange();
            if(Redo!.SuccessExitCodes.Any()) {
                SuccessExitCodes = numericRange.ToString(Redo.SuccessExitCodes);
            } else {
                SuccessExitCodes = "0";
            }
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            if(Model.IsLazyLoad) {
                return;
            }

            var numericRange = new NumericRange();
            if(numericRange.TryParse(SuccessExitCodes, out var values)) {
                Redo!.SuccessExitCodes.SetRange(values);
            } else {
                Redo!.SuccessExitCodes.SetRange(new[] { 0 });
            }
        }

        #endregion
    }
}
