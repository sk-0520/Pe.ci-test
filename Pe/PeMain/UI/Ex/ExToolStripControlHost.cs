/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/19
 * 時刻: 21:42
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of ExToolStripControlHost.
	/// </summary>
	public class ExToolStripControlHost: ToolStripControlHost
	{
		/// <summary>
		/// 指定したコントロールをホストする ToolStripControlHost クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="Control"></param>
		public ExToolStripControlHost(Control c): base(c)
		{ }
		/// <summary>
		/// 指定したコントロールをホストし、指定した名前を持つ ToolStripControlHost クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="Control"></param>
		/// <param name="String"></param>
		public ExToolStripControlHost(Control c, String s): base(c, s)
		{ }
	}
}
