namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;

	public enum ClipboardListType
	{
		/// <summary>
		/// クリップボード履歴。
		/// </summary>
		History,
		/// <summary>
		/// テンプレート。
		/// </summary>
		Template,
	}

	/// <summary>
	/// 認識可能とするクリップボード形式。
	/// </summary>
	[Flags]
	public enum ClipboardType
	{
		/// <summary>
		/// 無効。
		/// </summary>
		None = 0,
		/// <summary>
		/// プレーンテキスト。
		/// </summary>
		Text = 0x01,
		/// <summary>
		/// RTF。
		/// </summary>
		Rtf = 0x02,
		/// <summary>
		/// HTML。
		/// </summary>
		Html = 0x04,
		/// <summary>
		/// 画像。
		/// </summary>
		Image = 0x08,
		/// <summary>
		/// ファイル。
		/// </summary>
		File = 0x10,
		/// <summary>
		/// 全部
		/// </summary>
		All = Text | Rtf | Html | Image | File
	}
}
