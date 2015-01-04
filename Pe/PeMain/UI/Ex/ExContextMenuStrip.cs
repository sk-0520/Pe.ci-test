using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public abstract class ExContextMenuStrip: ContextMenuStrip
	{ }
	
	/// <summary>
	/// コンテキストメニューで使用。
	/// </summary>
	public sealed class AppContextMenuStrip: ExContextMenuStrip
	{
		/// <summary>
		/// メニューは表示されているか。
		/// </summary>
		public bool ShowContextMenu { get; private set; }
		public bool IsExtension { get; private set; }
		
		protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
		{
			base.OnOpening(e);
			if(!e.Cancel) {
				ShowContextMenu = true;
				IsExtension = AppUtility.IsExtension();
			}
		}
		
		protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
		{
			base.OnClosing(e);
			if(!e.Cancel) {
				ShowContextMenu = false;
			}
		}
	}
}
