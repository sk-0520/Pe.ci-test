namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public abstract class ExRichTextBox: RichTextBox
	{ }

	/// <summary>
	/// https://social.msdn.microsoft.com/Forums/en-US/6f48c005-2ddf-4669-9c0a-6fe11c0b235f/how-to-make-a-textbox-transparent
	/// </summary>
	public class TransparentRichTextBox: ExRichTextBox
	{
		public TransparentRichTextBox()
			: base()
		{
			DetectUrls = false;
		}

		#region override

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams result = base.CreateParams;

				result.ExStyle |= (int)WS_EX.WS_EX_TRANSPARENT;

				return result;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//do not allow the background to be painted 
		}

		protected override void OnGotFocus(System.EventArgs e)
		{
			base.OnGotFocus(e);
			InvalidateEx();
		}

		protected override void OnLostFocus(System.EventArgs e)
		{
			base.OnLostFocus(e);
			InvalidateEx();
		}

		protected override void OnParentChanged(System.EventArgs e)
		{
			base.OnParentChanged(e);
			InvalidateEx();
		}

		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
			InvalidateEx();
		}

		#endregion //////////////////////////////////////

		protected void InvalidateEx()
		{
			if(Parent == null) {
				return;
			}

			var rc = new Rectangle(this.Location, this.Size);
			Parent.Invalidate(rc, true);
		}
	}

	public class NoteTextBox: TransparentRichTextBox
	{
		public NoteTextBox(): base()
		{
			ScrollBars = RichTextBoxScrollBars.None;
			LanguageOption = RichTextBoxLanguageOptions.UIFonts;
		}

		#region override

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			var dir = e.Delta > 0 ? SB.SB_LINEUP : SB.SB_LINEDOWN;
			NativeMethods.SendMessage(Handle, WM.WM_VSCROLL, new IntPtr((int)dir), IntPtr.Zero);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if(e.Control && e.KeyCode == Keys.V || e.Shift && e.KeyCode == Keys.Insert) {
				if(ReadOnly) {
					e.Handled = true;
					return;
				}

				if(Clipboard.ContainsText(TextDataFormat.Text) || Clipboard.ContainsText(TextDataFormat.UnicodeText)) {
					SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
				}
				e.Handled = true;
			}
		}

		#endregion
	}
}
