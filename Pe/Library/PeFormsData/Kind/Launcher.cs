namespace ContentTypeTextNet.Pe.PeMain.Kind
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ランチャー種別。
	/// </summary>
	public enum LauncherType
	{
		None,
		/// <summary>
		/// 何かのファイル。
		/// </summary>
		File,
		/// <summary>
		/// ディレクトリ。
		/// </summary>
		Directory,
		/// <summary>
		/// URI。
		/// 
		/// Commandへ置き換える。
		/// </summary>
		URI,
		/// <summary>
		/// コマンド。
		/// </summary>
		Command,
		/// <summary>
		/// 組み込み
		/// </summary>
		Embedded
	}

}
