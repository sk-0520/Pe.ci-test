using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum FeedbackKind
    {
        /// <summary>
        /// 不具合報告。
        /// </summary>
        [EnumResource]
        Bug,
        /// <summary>
        /// 提案。
        /// </summary>
        [EnumResource]
        Proposal,
        /// <summary>
        /// その他。
        /// </summary>
        [EnumResource]
        Others,
    }
}
