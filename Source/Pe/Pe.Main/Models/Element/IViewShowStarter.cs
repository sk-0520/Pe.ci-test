namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    public interface IViewShowStarter
    {
        #region property

        bool CanStartShowView { get; }

        #endregion

        #region function

        void StartView();

        #endregion
    }
}
