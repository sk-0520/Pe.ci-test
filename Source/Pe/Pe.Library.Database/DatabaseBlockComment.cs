namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBMS依存DBブロックコメントとコメント文中で特殊処理する起点・終点。
    /// </summary>
    /// <remarks>
    /// <para>あえてなんかすることはないはず。</para>
    /// </remarks>
    public readonly struct DatabaseBlockComment
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="begin">開始。</param>
        /// <param name="end">終了。</param>
        public DatabaseBlockComment(string begin, string end)
        {
            Begin = begin;
            End = end;
        }

        #region property

        /// <summary>
        /// 開始。
        /// </summary>
        public string Begin { get; }
        /// <summary>
        /// 終了。
        /// </summary>
        public string End { get; }

        #endregion
    }
}
