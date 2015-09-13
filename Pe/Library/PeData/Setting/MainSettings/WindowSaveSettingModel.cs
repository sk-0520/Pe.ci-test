namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ウィンドウ状態復元設定。
	/// </summary>
	[Serializable]
	public class WindowSaveSettingModel : SettingModelBase, IDeepClone
	{
		#region property

		/// <summary>
		/// 有効。
		/// </summary>
		[DataMember]
		public bool IsEnabled { get; set; }
		/// <summary>
		/// 保存数。
		/// </summary>
		[DataMember]
		public int SaveCount { get; set; }
		/// <summary>
		/// 保存間隔。
		/// </summary>
		[DataMember]
		public TimeSpan SaveIntervalTime { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (WindowSaveSettingModel)target;

			obj.IsEnabled = IsEnabled;
			obj.SaveCount = SaveCount;
			obj.SaveIntervalTime = SaveIntervalTime;
		}

		public IDeepClone DeepClone()
		{
			var result = new WindowSaveSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
