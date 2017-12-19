using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
    public struct IndexValue<TValue>
    {
        public IndexValue(TValue value, int index)
        {
            Value = value;
            Index = index;
        }

        #region property

        public int Index { get; }
        public TValue Value { get; }

        #endregion
    }
}
