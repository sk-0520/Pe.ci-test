namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public abstract class ExTableLayoutPanel: TableLayoutPanel
	{ }

	public class ToolbarPositionClickEventArgs: EventArgs
	{
		public ToolbarPositionClickEventArgs(ToolbarPosition toolbarPosition)
		{
			ToolbarPosition = toolbarPosition;
		}

		public ToolbarPosition ToolbarPosition { get; private set; }
	}

	public class ToolbarPositionTableLayoutPanel: ExTableLayoutPanel, ISetCommonData, ICommonData
	{
		readonly Size imageSize = new Size(IconScale.Small.ToWidth() * 2, IconScale.Small.ToHeight());
		readonly SizeF drawSize = new SizeF(0.2f, 0.3f);

		#region variable

		ToolbarPosition _toolbarPosition;

		#endregion

		#region event

		public event EventHandler<ToolbarPositionClickEventArgs> ToolbarPositionClick = delegate { };

		#endregion

		public ToolbarPositionTableLayoutPanel()
		{
			Initialize();
		}

		#region property

		public ToolbarPositionRadioButton CommandDesktopFloat { get; private set; }
		public ToolbarPositionRadioButton CommandDesktopLeft { get; private set; }
		public ToolbarPositionRadioButton CommandDesktopRight { get; private set; }
		public ToolbarPositionRadioButton CommandDesktopTop { get; private set; }
		public ToolbarPositionRadioButton CommandDesktopBottom { get; private set; }

		public CommonData CommonData { get; private set; }

		public ToolbarPosition ToolbarPosition
		{
			get { return this._toolbarPosition; }
			set
			{
				this._toolbarPosition = value;
				var commands = GetCommands()
					.Single(c => c.ToolbarPosition == value)
					.Checked = true
				;
			}
		}

		#endregion

		#region ISetCommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplyLanguage();
			ApplySkin();
		}

		#endregion

		#region initialize

		void Initialize()
		{
			CommandDesktopFloat = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopFloat,
			};
			CommandDesktopLeft = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopLeft,
			};
			CommandDesktopRight = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopRight,
			};
			CommandDesktopTop = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopTop,
			};
			CommandDesktopBottom = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopBottom,
			};

			ToolbarPosition = ToolbarPosition.DesktopFloat;

			foreach(var command in GetCommands()) {
				command.TabStop = false;
				command.Appearance = Appearance.Button;
				command.Size = AppUtility.GetButtonSize(imageSize);
				command.Margin = new Padding(1);
				command.Click += command_Click;
			}

			Controls.Add(this.CommandDesktopTop, 1, 0);
			Controls.Add(this.CommandDesktopLeft, 0, 1);
			Controls.Add(this.CommandDesktopFloat, 1, 1);
			Controls.Add(this.CommandDesktopRight, 2, 1);
			Controls.Add(this.CommandDesktopBottom, 1, 2);

			AutoSize = true;
			Size = Size.Empty;
		}

		#endregion

		#region function

		IEnumerable<ToolbarPositionRadioButton> GetCommands()
		{
			return new[] {
				CommandDesktopFloat,
				CommandDesktopLeft,
				CommandDesktopRight,
				CommandDesktopTop,
				CommandDesktopBottom,
			};
		}

		void ApplySkin()
		{
			foreach(var command in GetCommands()) {
				command.Image = CreateToolbarImage(command.ToolbarPosition);
			}
		}

		void ApplyLanguage()
		{ }

		Image CreateToolbarImage(ToolbarPosition toolbarPosition)
		{
			using(var targetGraphics = CreateGraphics()) {
				var image = new Bitmap(imageSize.Width, imageSize.Height, targetGraphics);
				var drawArea = new Rectangle();
				switch(toolbarPosition) {
					case ToolbarPosition.DesktopFloat:
						drawArea.Width = (int)(imageSize.Width * 0.8);
						drawArea.Height = (int)(imageSize.Height * 0.4);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case ToolbarPosition.DesktopLeft:
						drawArea.Width = (int)(imageSize.Width * drawSize.Width);
						drawArea.Height = imageSize.Height;
						drawArea.X = 0;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case ToolbarPosition.DesktopRight:
						drawArea.Width = (int)(imageSize.Width * drawSize.Width);
						drawArea.Height = imageSize.Height;
						drawArea.X = imageSize.Width - drawArea.Width;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case ToolbarPosition.DesktopTop:
						drawArea.Width = imageSize.Width;
						drawArea.Height = (int)(imageSize.Height * drawSize.Height);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = 0;
						break;
					case ToolbarPosition.DesktopBottom:
						drawArea.Width = imageSize.Width;
						drawArea.Height = (int)(imageSize.Height * drawSize.Height);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = imageSize.Height - drawArea.Height;
						break;
					default:
						throw new NotImplementedException();
				}
				using(var g = Graphics.FromImage(image)) {
					using(var box = CommonData.Skin.CreateColorBoxImage(AppUtility.GetToolbarPositionColor(true, false), AppUtility.GetToolbarPositionColor(false, false), imageSize)) {
						g.DrawImage(box, Point.Empty);
					}
					using(var box = CommonData.Skin.CreateColorBoxImage(AppUtility.GetToolbarPositionColor(true, true), AppUtility.GetToolbarPositionColor(false, true), drawArea.Size)) {
						g.DrawImage(box, drawArea.Location);
					}
				}

				return image;
			}
		}

		#endregion

		void command_Click(object sender, EventArgs e)
		{
			var command = (ToolbarPositionRadioButton)sender;
			ToolbarPositionClick(this, new ToolbarPositionClickEventArgs(command.ToolbarPosition));
		}

	}
}
