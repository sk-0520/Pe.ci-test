namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ランチャーにファイルをD&amp;Dした際の挙動。
	/// </summary>
	public enum LauncherItemFileDropMode
	{
		/// <summary>
		/// 指定して実行ウィンドウの表示。
		/// </summary>
		ShowExecuteWindow,
		/// <summary>
		/// パラメータとして実行。
		/// </summary>
		ArgumentExecute,
	}
}
