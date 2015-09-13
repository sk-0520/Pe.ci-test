namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using System;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// 共通行。
	/// </summary>
	public abstract class CommonDataRow: Row
	{
		/// <summary>
		/// 作成日時。
		/// </summary>
		[EntityMapping("CMN_CREATE")]
		public DateTime CommonCreate { get; set; }
		/// <summary>
		/// 更新日時。
		/// </summary>
		[EntityMapping("CMN_UPDATE")]
		public DateTime CommonUpdate { get; set; }
	}

	/// <summary>
	/// 有効フラグ付共通行。
	/// </summary>
	public abstract class CommonDataEnabledRow: CommonDataRow
	{
		/// <summary>
		/// 有効か。
		/// </summary>
		[EntityMapping("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
	}

}
