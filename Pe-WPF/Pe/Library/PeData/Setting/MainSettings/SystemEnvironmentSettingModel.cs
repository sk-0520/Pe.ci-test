namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class SystemEnvironmentSettingModel : SettingModelBase, IDeepClone
	{
		public SystemEnvironmentSettingModel()
			: base()
		{
			HideFileHotkey = new HotKeyModel();
			ExtensionHotkey = new HotKeyModel();
		}

		#region property

		/// <summary>
		/// 隠しファイル表示切り替えホットキー。
		/// </summary>
		[DataMember]
		public HotKeyModel HideFileHotkey { get; set; }
		/// <summary>
		/// 拡張子表示切替ホットキー。
		/// </summary>
		[DataMember]
		public HotKeyModel ExtensionHotkey { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (SystemEnvironmentSettingModel)target;

			HideFileHotkey.DeepCloneTo(obj.HideFileHotkey);
			ExtensionHotkey.DeepCloneTo(obj.ExtensionHotkey);
		}

		public IDeepClone DeepClone()
		{
			var result = new SystemEnvironmentSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
