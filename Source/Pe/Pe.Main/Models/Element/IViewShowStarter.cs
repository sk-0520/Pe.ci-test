namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    /// <summary>
    /// ビューの表示開始を行う。
    /// </summary>
    public interface IViewShowStarter
    {
        #region property

        /// <summary>
        /// ビューの表示開始が可能か。
        /// </summary>
        bool CanStartShowView { get; }

        #endregion

        #region function

        /// <summary>
        /// ビューを開始する。
        /// </summary>
        void StartView();

        #endregion
    }
}
