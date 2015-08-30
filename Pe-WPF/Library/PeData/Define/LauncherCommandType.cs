namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ランチャーアイテムの詳細呼び出し対象。
	/// </summary>
	public enum LauncherCommandType
	{
		/// <summary>
		/// コマンド。
		/// </summary>
		Command,
		/// <summary>
		/// 親ディレクトリ。
		/// </summary>
		ParentDirectory,
		/// <summary>
		/// 作業ディレクトリ。
		/// </summary>
		WorkDirectory,
	}
}
