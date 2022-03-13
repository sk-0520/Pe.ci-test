using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class HitValueItemViewModel: ViewModelBase
    {
        public HitValueItemViewModel(HitValue hitValue, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = hitValue.Value;
            IsHit = hitValue.IsHit;
        }

        #region property

        public string Value { get; }
        public bool IsHit { get; }

        #endregion
    }
}
