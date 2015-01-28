namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	public class CountDto: Dto
	{
		[TargetName("NUM")]
		public long Count { get; set; }
		
		public bool Has { get { return Count > 0; } }
	}
	
	public class SingleIdDto: Dto
	{
		[TargetName("MAX_ID")]
		public long MaxId { get; set; }
		[TargetName("MIN_ID")]
		public long MinId { get; set; }
	}
}
