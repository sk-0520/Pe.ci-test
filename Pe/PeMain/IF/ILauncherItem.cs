namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using ContentTypeTextNet.Pe.PeMain.Data;

	/// <summary>
	/// LauncherItemの参照。
	/// </summary>
	public interface ILauncherItem
	{
		/// <summary>
		/// 参照するLauncherItem。
		/// </summary>
		LauncherItem LauncherItem { get; set; }
	}
}
