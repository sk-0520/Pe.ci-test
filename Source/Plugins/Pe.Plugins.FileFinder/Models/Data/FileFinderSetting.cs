using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Models.Data
{
    public class FileFinderSetting
    {
        #region property

        /// <summary>
        /// 隠しファイルを列挙するか。
        /// </summary>
        public bool IncludeHiddenFile { get; set; } = true;

        /// <summary>
        /// PATHの通っている実行ファイルを列挙するか。
        /// </summary>
        public bool IncludePath { get; set; } = true;

        /// <summary>
        /// パスからの列挙において列挙する上限数。
        /// <para>0 で制限しない。</para>
        /// </summary>
        public int MaximumPathItem { get; set; } = 10;

        /// <summary>
        /// パス検索を有効にする入力文字数(以上)。
        /// </summary>
        public int PathEnabledInputCharCount { get; set; } = 2;

        #endregion
    }
}
