using System;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// システム環境に対する操作設定。
	/// </summary>
	[Serializable]
	public class SystemEnvSetting: Item
	{
		public SystemEnvSetting()
		{
			HiddenFileShowHotKey = new HotKeySetting();
			ExtensionShowHotKey = new HotKeySetting();
		}
		/// <summary>
		/// 隠しファイルの表示非表示切り替えホットキー
		/// </summary>
		public HotKeySetting HiddenFileShowHotKey { get; set; }
		/// <summary>
		/// 拡張子の表示非表示切り替えホットキー
		/// </summary>
		public HotKeySetting ExtensionShowHotKey { get; set; }
	}
}
