using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	/// <summary>
	/// Description of M_NOTE.
	/// </summary>
	[TargetName("M_NOTE")]
	public class MNoteEntity: CommonDataEnabledEntity
	{
		[TargetName("NOTE_ID", true)]
		public long Id { get; set; }
		[TargetName("NOTE_TITLE")]
		public string Title { get; set; }
		[TargetName("NOTE_TYPE")]
		public int RawType { get; set; }
	}
}
