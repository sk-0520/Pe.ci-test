namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 使用許諾結果。
    /// </summary>
    public record AcceptResult
    {
        public AcceptResult(bool accepted, UpdateKind updateKind, bool isEnabledTelemetry)
        {
            Accepted = accepted;
            UpdateKind = updateKind;
            IsEnabledTelemetry = isEnabledTelemetry;
        }

        #region property

        /// <summary>
        /// 許可されたか。
        /// </summary>
        public bool Accepted { get; }
        /// <summary>
        /// アップデート方法。
        /// </summary>
        public UpdateKind UpdateKind { get; }
        /// <summary>
        /// テレメトリー許可。
        /// </summary>
        public bool IsEnabledTelemetry { get; }

        #endregion
    }
}
