using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public readonly struct HitValue
    {
        public HitValue(string value, bool isHit)
        {
            Value = value;
            IsHit = isHit;
        }

        #region property

        public string Value { get; }
        public bool IsHit { get; }

        #endregion
    }
}
