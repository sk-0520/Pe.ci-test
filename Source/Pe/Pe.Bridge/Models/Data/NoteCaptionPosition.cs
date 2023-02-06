namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// ノート タイトルバー位置。
    /// </summary>
    public enum NoteCaptionPosition
    {
        /// <summary>
        /// 上。
        /// </summary>
        Top,
        /// <summary>
        /// 下。
        /// </summary>
        Bottom,
        /// <summary>
        /// 左。
        /// </summary>
        Left,
        /// <summary>
        /// 右。
        /// </summary>
        Right
    }

    public static class NoteCaptionPositionExtensions
    {
        #region function

        /// <summary>
        /// 上下指定。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsVertical(this NoteCaptionPosition position)
        {
            return position == NoteCaptionPosition.Top || position == NoteCaptionPosition.Bottom;
        }

        /// <summary>
        /// 左右指定。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsHorizontal(this NoteCaptionPosition position)
        {
            return position == NoteCaptionPosition.Left || position == NoteCaptionPosition.Right;
        }

        #endregion
    }
}
