namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using System;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	public abstract class CommonDataEntity: Entity
	{
		[EntityMapping("CMN_CREATE")]
		public DateTime CommonCreate { get; set; }
		[EntityMapping("CMN_UPDATE")]
		public DateTime CommonUpdate { get; set; }
	}
	public abstract class CommonDataEnabledEntity: CommonDataEntity
	{
		[EntityMapping("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
	}

}
