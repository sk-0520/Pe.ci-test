namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Flags]
	public enum ClipboardType: int
	{
		/// <summary>
		/// 無し。
		/// <para>比較時にのみ使用する感じ。</para>
		/// </summary>
		None = 0x00,
		/// <summary>
		/// プレーンテキスト。
		/// </summary>
		Text = 0x01,
		/// <summary>
		/// 書式付文字列。
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
		Files = 0x10,
		/// <summary>
		/// 全て。
		/// </summary>
		All = Text | Rtf | Html | Image | Files,
	}
}
