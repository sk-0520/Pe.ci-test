namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class SystemEnvironmentSettingModel : SettingModelBase
	{
		public SystemEnvironmentSettingModel()
			: base()
		{
			HideFileHotkey = new HotkeyModel();
			ExtensionHotkey = new HotkeyModel();
		}

		/// <summary>
		/// 隠しファイル表示切り替えホットキー。
		/// </summary>
		[DataMember]
		public HotkeyModel HideFileHotkey { get; set; }
		/// <summary>
		/// 拡張子表示切替ホットキー。
		/// </summary>
		[DataMember]
		public HotkeyModel ExtensionHotkey { get; set; }
	}
}
