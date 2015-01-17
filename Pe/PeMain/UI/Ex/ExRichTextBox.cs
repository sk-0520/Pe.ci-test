namespace ContentTypeTextNet.Pe.PeMain.UI
{
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
		{ }

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

		#endregion //////////////////////////////////////

		protected void InvalidateEx()
		{
			if(Parent == null)
				return;

			var rc = new Rectangle(this.Location, this.Size);
			Parent.Invalidate(rc, true);
		}
	}

	public class NoteTextBox: TransparentRichTextBox
	{
		public NoteTextBox(): base()
		{
			ScrollBars = RichTextBoxScrollBars.None;
		}

	}
}
