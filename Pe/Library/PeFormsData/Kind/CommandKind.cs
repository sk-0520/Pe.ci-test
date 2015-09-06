namespace ContentTypeTextNet.Pe.PeMain.Kind
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// コマンド種別。
	/// </summary>
	public enum CommandKind
	{
		/// <summary>
		/// なし。
		/// </summary>
		None,
		/// <summary>
		/// ランチャー名。
		/// </summary>
		LauncherItem_Name,
		/// <summary>
		/// ランチャータグ。
		/// </summary>
		LauncherItem_Tag,
		/// <summary>
		/// ファイルパス。
		/// </summary>
		FilePath,
		/// <summary>
		/// URI
		/// </summary>
		Uri,
	}
}
