namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// 何らかの数(大抵は行数)を取得。
	/// </summary>
	public class CountDto: Dto
	{
		/// <summary>
		/// 数。
		/// </summary>
		[EntityMapping("NUM")]
		public long Count { get; set; }
		
		/// <summary>
		/// 0件以上取得できたか。
		/// </summary>
		public bool Has { get { return Count > 0; } }
	}
	
	/// <summary>
	/// 各IDの最大最小値を取得する。
	/// 
	/// IDが何を指すかは実装により異なる。
	/// </summary>
	public class SingleIdDto: Dto
	{
		/// <summary>
		/// 最大値。
		/// </summary>
		[EntityMapping("MAX_ID")]
		public long MaxId { get; set; }
		/// <summary>
		/// 最小値。
		/// </summary>
		[EntityMapping("MIN_ID")]
		public long MinId { get; set; }
	}
}
