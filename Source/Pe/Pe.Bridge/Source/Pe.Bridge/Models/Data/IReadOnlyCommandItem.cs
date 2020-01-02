using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public interface IReadOnlyCommandItem
    {
        #region property

        string DisplayText { get; }

        IReadOnlyList<Range> MatchRanges { get; }

        #endregion

        #region function

        object GetIcon(IconBox iconBox);

        #endregion
    }
}
