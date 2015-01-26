namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using ContentTypeTextNet.Pe.Library.Skin;

	/// <summary>
	/// ISkinを受信可能とする。
	/// </summary>
	public interface ISetSkin
	{
		/// <summary>
		/// ISkinの受信。
		/// </summary>
		/// <param name="skin"></param>
		void SetSkin(ISkin skin);
	}
}
