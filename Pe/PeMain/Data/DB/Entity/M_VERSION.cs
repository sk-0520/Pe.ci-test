namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	[TargetName("M_VERSION")]
	public class MVersionEntity: Entity
	{
		[TargetName("NAME", true)]
		public string Name { get; set; }
		[TargetName("VERSION")]
		public int Version { get; set; }
	}
}
