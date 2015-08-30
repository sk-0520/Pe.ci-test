namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// T_NOTE行。
	/// </summary>
	[EntityMapping("T_NOTE")]
	public class TNoteRow: CommonDataRow, INoteId
	{
		/// <summary>
		/// ノートID。
		/// </summary>
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }
		/// <summary>
		/// 本文。
		/// </summary>
		[EntityMapping("NOTE_BODY")]
		public string Body { get; set ;}
	}
}
