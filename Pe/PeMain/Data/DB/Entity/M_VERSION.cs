namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	[EntityMapping("M_VERSION")]
	public class MVersionEntity: Entity
	{
		[EntityMapping("NAME", true)]
		public string Name { get; set; }
		[EntityMapping("VERSION")]
		public int Version { get; set; }
	}
}
