namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ウィンドウ状態復元設定。
	/// </summary>
	[Serializable]
	public class WindowSaveSettingModel : SettingModelBase
	{
		/// <summary>
		/// 有効。
		/// </summary>
		[DataMember]
		public bool Enabled { get; set; }
		/// <summary>
		/// 保存数。
		/// </summary>
		[DataMember]
		public int SaveCount { get; set; }
		/// <summary>
		/// 保存間隔。
		/// </summary>
		[DataMember]
		public TimeSpan SaveSpan { get; set; }
	}
}
