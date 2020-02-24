using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class HitValueItemViewModel : ViewModelBase
    {
        public HitValueItemViewModel(HitValue hitValue, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = hitValue.Value;
            IsHit = hitValue.IsHit;
        }

        #region MyRegion

        public string Value { get; }
        public bool IsHit { get; }

        #endregion
    }
}
