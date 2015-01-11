using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	[TargetName("T_NOTE")]
	public class TNoteEntity: CommonDataEntity
	{
		[TargetName("NOTE_ID", true)]
		public long Id { get; set; }
		[TargetName("NOTE_BODY")]
		public string Body { get; set ;}
	}
}
