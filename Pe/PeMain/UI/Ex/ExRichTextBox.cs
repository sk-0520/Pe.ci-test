namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public abstract class ExRichTextBox: RichTextBox
	{ }

	public class TransparentRichTextBox: ExRichTextBox
	{
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

		#endregion //////////////////////////////////////
	}

	public class NoteTextBox: TransparentRichTextBox
	{
		public NoteTextBox(): base()
		{
		}
	}
}
