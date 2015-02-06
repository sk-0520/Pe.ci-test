namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// M_NOTEの行。
	/// </summary>
	[EntityMapping("M_NOTE")]
	public class MNoteRow: CommonDataEnabledRow
	{
		/// <summary>
		/// ノートID。
		/// </summary>
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }
		/// <summary>
		/// タイトル。
		/// </summary>
		[EntityMapping("NOTE_TITLE")]
		public string Title { get; set; }
		/// <summary>
		/// ノート種別。
		/// </summary>
		[EntityMapping("NOTE_TYPE")]
		public int RawType { get; set; }
	}
}
