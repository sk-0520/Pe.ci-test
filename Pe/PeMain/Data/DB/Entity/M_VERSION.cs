using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	[TargetName("M_VERSION")]
	public class MVersionEntity: Entity
	{
		[TargetName("NAME", true)]
		public string Name { get; set; }
		[TargetName("VERSION")]
		public int Version { get; set; }
	}
}
