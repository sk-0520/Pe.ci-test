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
			using(var format = CreateTooltipFormat()) {
				using(var brush = new SolidBrush(ForeColor)) {
					var drawArea = new Rectangle(
						ToolTipPadding.Width,
						ToolTipPadding.Height,
						ClientSize.Width + ToolTipPadding.Width,
						ClientSize.Height + ToolTipPadding.Height
					);
					e.Graphics.DrawString(NoteItem.Body, font, brush, drawArea, format);
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

		StringFormat CreateTooltipFormat()
		{
			var sf = new StringFormat();

			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Near;
			sf.Trimming = StringTrimming.None;

			return sf;
		}

		public void ShowItem(Screen screen, ToolStripItem toolStripItem, NoteItem noteItem)
		{
			var parent = toolStripItem.Owner;

			var rect = toolStripItem.Bounds;
			var location = new Point(parent.Location.X + rect.X, parent.Location.Y + rect.Y);
			
			NoteItem = noteItem;

			using(var g = CreateGraphics())
			using(var format = CreateTooltipFormat()) {
				var messageSize = g.MeasureString(NoteItem.Body, NoteItem.Style.FontSetting.Font, NoteItem.Size, format);
				Size = new Size(
					(int)Math.Min(NoteItem.Size.Width, messageSize.Width) + ToolTipPadding.Width * 2,
					(int)Math.Min(NoteItem.Size.Height, messageSize.Height) + ToolTipPadding.Height * 2
				);
			}

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
