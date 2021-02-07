namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// 該当状態文字列。
    /// </summary>
    public readonly struct HitValue
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value">対象文字列。</param>
        /// <param name="isHit"><param name="value" /> は該当しているか。</param>
        public HitValue(string value, bool isHit)
        {
            Value = value;
            IsHit = isHit;
        }

        #region property

        /// <summary>
        /// 対象文字列。
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// <see cref="Value"/> は該当しているか。
        /// </summary>
        public bool IsHit { get; }

        #endregion
    }
}
