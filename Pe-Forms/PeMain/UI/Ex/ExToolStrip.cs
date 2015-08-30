namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public abstract class ExToolStrip: ToolStrip
	{ }

	/// <summary>
	/// アクティブ・非アクティブで挙動の変わらないToolStrip。
	/// </summary>
	public class ActiveToolStrip: ExToolStrip
	{
		/// <summary>
		/// thanks: http://d.hatena.ne.jp/Kazzz/20061106/p1
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if ( m.Msg == (int)WM.WM_MOUSEACTIVATE && m.Result == (IntPtr)MA.MA_ACTIVATEANDEAT) {
				m.Result = (IntPtr)MA.MA_ACTIVATE;
			}
		}
	}
	
	public class ToolbarToolStrip: ActiveToolStrip
	{ }

}
