using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public interface ICommandItem
    {
        #region property

        /// <summary>
        /// メイン表示文字列。
        /// </summary>
        string Header { get; }
        /// <summary>
        /// 追記文言。
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 小さく表示する種別文言。
        /// </summary>
        string Kind { get; }

        IReadOnlyList<Range> HeaderMatches { get; }
        IReadOnlyList<Range> DescriptionMatches { get; }

        #endregion

        #region function

        object GetIcon(IconBox iconBox);
        void Execute();

        #endregion
    }
}
