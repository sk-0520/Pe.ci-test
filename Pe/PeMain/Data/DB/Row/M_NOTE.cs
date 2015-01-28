namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// Description of M_NOTE.
	/// </summary>
	[EntityMapping("M_NOTE")]
	public class MNoteEntity: CommonDataEnabledEntity
	{
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }
		[EntityMapping("NOTE_TITLE")]
		public string Title { get; set; }
		[EntityMapping("NOTE_TYPE")]
		public int RawType { get; set; }
	}
}
