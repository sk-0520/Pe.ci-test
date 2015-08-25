namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ノートの通常タイトル文字列。
	/// </summary>
	public enum NoteTitle
	{
		/// <summary>
		/// タイムスタンプを使用。
		/// </summary>
		Timestamp,
		/// <summary>
		/// デフォルトタイトルを使用。
		/// <para>言語設定に依存。</para>
		/// </summary>
		DefaultCaption
	}
}
