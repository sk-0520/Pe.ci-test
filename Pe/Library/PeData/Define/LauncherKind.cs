namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ランチャー種別。
	/// </summary>
	public enum LauncherKind
	{
		/// <summary>
		/// 何かのファイル。
		/// </summary>
		File,
		/// <summary>
		/// ディレクトリ。
		/// <para>作っとくだけで使わない。</para>
		/// TODO: どうしようかねー。
		/// </summary>
		Directory,
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
