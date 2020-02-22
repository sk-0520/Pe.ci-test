using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class HitValueItem : ViewModelBase
    {
        public HitValueItem(string value, bool isHit, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = value;
            IsHit = isHit;
        }

        #region MyRegion

        public string Value { get; }
        public bool IsHit { get; }

        #endregion
    }
}
