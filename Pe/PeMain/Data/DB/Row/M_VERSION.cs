namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// バージョン行。
	/// </summary>
	[EntityMapping("M_VERSION")]
	public class MVersionRow: Row
	{
		/// <summary>
		/// テーブル名。
		/// </summary>
		[EntityMapping("NAME", true)]
		public string Name { get; set; }
		/// <summary>
		/// バージョン。
		/// </summary>
		[EntityMapping("VERSION")]
		public int Version { get; set; }
	}
}
