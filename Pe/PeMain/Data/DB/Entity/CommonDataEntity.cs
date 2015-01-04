using System;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	public abstract class CommonDataEntity: Entity
	{
		[TargetName("CMN_CREATE")]
		public DateTime CommonCreate { get; set; }
		[TargetName("CMN_UPDATE")]
		public DateTime CommonUpdate { get; set; }
	}
	public abstract class CommonDataEnabledEntity: CommonDataEntity
	{
		[TargetName("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
	}

}
