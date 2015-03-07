namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;

	/// <summary>
	/// 作成・更新日時保持。
	/// </summary>
	[Serializable]
	public class DateHistory: Item, ICloneable
	{
		/// <summary>
		/// 
		/// </summary>
		public DateHistory()
		{
			Create = DateTime.Now;
			Update = DateTime.Now;
		}
		/// <summary>
		/// 作成日。
		/// </summary>
		public DateTime Create { get; set; }
		/// <summary>
		/// 更新日。
		/// </summary>
		public DateTime Update { get; set; }
		
		/// <summary>
		/// コピー。
		/// </summary>
		/// <returns>コピーされたオブジェクト</returns>
		public object Clone()
		{
			var result = new DateHistory();
			
			result.Create = Create;
			result.Update = Update;
			
			return result;
		}
	}
}
