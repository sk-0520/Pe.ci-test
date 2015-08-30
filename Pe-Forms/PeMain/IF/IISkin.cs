namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using ContentTypeTextNet.Pe.Library.Skin;

	/// <summary>
	/// ISkinを設定可能とする。
	/// </summary>
	public interface ISetSkin
	{
		/// <summary>
		/// ISkinの設定。
		/// </summary>
		/// <param name="skin"></param>
		void SetSkin(ISkin skin);
	}

	/// <summary>
	/// ISkinの参照。
	/// </summary>
	public interface IISkin
	{
		/// <summary>
		/// 参照するISkin。
		/// </summary>
		ISkin Skin { get; }
	}
}
