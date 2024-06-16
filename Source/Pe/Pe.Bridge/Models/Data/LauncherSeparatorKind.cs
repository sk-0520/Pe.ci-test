using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public enum LauncherSeparatorKind
    {
        /// <summary>
        /// ユーザー設定では原則使わない。
        /// </summary>
        /// <remarks>未指定を示す要素。</remarks>
        None,
        /// <summary>
        /// 単純線。
        /// </summary>
        Line,
        /// <summary>
        /// 空白。
        /// </summary>
        Space,
    }
}
