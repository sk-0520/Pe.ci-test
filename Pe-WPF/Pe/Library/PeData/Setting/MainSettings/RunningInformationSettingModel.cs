namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class RunningInformationSettingModel : SettingModelBase
	{
		public RunningInformationSettingModel()
			: base()
		{ }

		/// <summary>
		/// 実行が許可されているか。
		/// </summary>
		[DataMember]
		public bool Accept { get; set; }
		/// <summary>
		/// 前回終了時のバージョン。
		/// </summary>
		[DataMember]
		public Version LastExecuteVersion { get; set; }
		/// <summary>
		/// アップデートチェックを行うか。
		/// </summary>
		[DataMember]
		public bool CheckUpdateRelease { get; set; }
		/// <summary>
		/// RCアップデートチェックを行うか。
		/// </summary>
		[DataMember]
		public bool CheckUpdateRC { get; set; }
		/// <summary>
		/// アップデートチェックで無視するバージョン。
		/// </summary>
		[DataMember]
		public Version IgnoreUpdateVersion { get; set; }
		/// <summary>
		/// プログラム実行回数。
		/// </summary>
		[DataMember]
		public int ExecuteCount { get; set; }
	}
}
