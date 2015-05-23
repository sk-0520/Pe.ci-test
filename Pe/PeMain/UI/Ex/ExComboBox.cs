namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public class ExComboBox: ComboBox
	{ }

	public class TextCutEventArgs: EventArgs
	{
		public bool Cancel {get;set;}
	}

	public class CommandComboBox: ExComboBox
	{
		#region event

		public event EventHandler<TextCutEventArgs> TextCutting = delegate { };

		#endregion

		#region function

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == (int)WM.WM_CUT) {
				var e = new TextCutEventArgs();
				OnTextCutting(e);
				if(e.Cancel) {
					return;
				}
			}
			base.WndProc(ref m);
		}

		#endregion

		#region function

		protected void OnTextCutting(TextCutEventArgs e)
		{
			TextCutting(this, e);
		}

		#endregion
	}
}
