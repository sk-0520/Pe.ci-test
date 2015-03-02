namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;

	/// <summary>
	/// システム環境に対する操作設定。
	/// </summary>
	[Serializable]
	public class SystemEnvironmentSetting: Item
	{
		public SystemEnvironmentSetting()
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
