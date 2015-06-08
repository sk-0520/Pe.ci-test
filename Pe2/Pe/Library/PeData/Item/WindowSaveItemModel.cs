namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ウィンドウ状態復元設定。
	/// </summary>
	public class WindowSaveItemModel : ItemModelBase
	{
		/// <summary>
		/// 有効。
		/// </summary>
		public bool Enabled { get; set; }
		/// <summary>
		/// 保存数。
		/// </summary>
		public int SaveCount { get; set; }
		/// <summary>
		/// 保存間隔。
		/// </summary>
		public TimeSpan SaveSpan { get; set; }
	}
}
