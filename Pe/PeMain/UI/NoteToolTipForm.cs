namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	public partial class NoteToolTipForm: CommonToolTipForm
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		#endregion ////////////////////////////////////

		public NoteToolTipForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region property

		public NoteItem NoteItem { get; private set; }

		#endregion ////////////////////////////////////


		#region override

		protected override void OnPaint(PaintEventArgs e)
		{
			if(NoteItem == null) {
				base.OnPaint(e);
				return;
			}
			var font = NoteItem.Style.FontSetting.Font;
			using(var format = new StringFormat()) {
				format.Alignment = StringAlignment.Near;
				format.LineAlignment = StringAlignment.Near;
				using(var brush = new SolidBrush(ForeColor)) {
					e.Graphics.DrawString(NoteItem.Body, font, brush, ClientRectangle, format);
				}
			}
		}
		#endregion ////////////////////////////////////

		#region initialize

		void Initialize()
		{ }

		#endregion ////////////////////////////////////

		#region language
		#endregion ////////////////////////////////////

		#region function

		public void ShowItem(Screen screen, ToolStripItem toolStripItem, NoteItem noteItem)
		{
			var parent = toolStripItem.Owner;

			var rect = toolStripItem.Bounds;
			var location = new Point(parent.Location.X + rect.X, parent.Location.Y + rect.Y);
			
			NoteItem = noteItem;

			Size = NoteItem.Size;

			var showPoint = new Point(location.X + toolStripItem.Width, location.Y);
			// 横方向補正
			if(screen.Bounds.Right < showPoint.X + Size.Width) {
				showPoint.X -= Size.Width + toolStripItem.Width;
			}
			// 下方向補正
			if(screen.Bounds.Bottom < showPoint.Y + Size.Height) {
				showPoint.Y -= (showPoint.Y + Size.Height) - screen.Bounds.Bottom;
			}

			Location = showPoint;

			ToNoActiveShow();
		}

		#endregion ////////////////////////////////////
	}
}
