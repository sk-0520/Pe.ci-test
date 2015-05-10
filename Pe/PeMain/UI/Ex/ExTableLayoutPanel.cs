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

		Label _labelToolbar;
		ToolbarPositionRadioButton _selectDesktopFloat;
		ToolbarPositionRadioButton _selectDesktopLeft;
		ToolbarPositionRadioButton _selectDesktopRight;
		ToolbarPositionRadioButton _selectDesktopTop;
		ToolbarPositionRadioButton _selectDesktopBottom;

		#endregion

		#region event

		public event EventHandler<ToolbarPositionClickEventArgs> ToolbarPositionClick = delegate { };

		#endregion

		public ToolbarPositionTableLayoutPanel()
		{
			Initialize();
		}

		#region property

		public CommonData CommonData { get; private set; }

		public ToolbarPosition ToolbarPosition
		{
			get { return this._toolbarPosition; }
			set
			{
				this._toolbarPosition = value;
				GetToolbarPositionControl()
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
			this._labelToolbar = new Label() {
				Text = ":enum/toolbar-position",
				AutoSize = true,
				Margin = Padding.Empty,
			};
			this._selectDesktopFloat = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopFloat,
			};
			this._selectDesktopLeft = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopLeft,
			};
			this._selectDesktopRight = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopRight,
			};
			this._selectDesktopTop = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopTop,
			};
			this._selectDesktopBottom = new ToolbarPositionRadioButton() {
				ToolbarPosition = ToolbarPosition.DesktopBottom,
			};

			ToolbarPosition = ToolbarPosition.DesktopFloat;

			foreach(var control in GetToolbarPositionControl()) {
				control.TabStop = false;
				control.Appearance = Appearance.Button;
				control.Size = AppUtility.GetButtonSize(imageSize);
				control.Margin = new Padding(1);
				control.Click += command_Click;
			}

			
			Controls.Add(this._labelToolbar, 0, 0);
			Controls.Add(this._selectDesktopTop, 1, 1);
			Controls.Add(this._selectDesktopLeft, 0, 2);
			Controls.Add(this._selectDesktopFloat, 1, 2);
			Controls.Add(this._selectDesktopRight, 2, 2);
			Controls.Add(this._selectDesktopBottom, 1, 3);

			SetColumnSpan(this._labelToolbar, 3);

			AutoSize = true;
			Size = Size.Empty;
		}

		#endregion

		#region function

		IEnumerable<ToolbarPositionRadioButton> GetToolbarPositionControl()
		{
			return new[] {
				this._selectDesktopFloat,
				this._selectDesktopLeft,
				this._selectDesktopRight,
				this._selectDesktopTop,
				this._selectDesktopBottom,
			};
		}

		void ApplySkin()
		{
			foreach(var command in GetToolbarPositionControl()) {
				command.Image = CreateToolbarImage(command.ToolbarPosition);
			}
		}

		void ApplyLanguage()
		{
			this._labelToolbar.SetLanguage(CommonData.Language);
		}

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
			var control = (ToolbarPositionRadioButton)sender;
			ToolbarPositionClick(this, new ToolbarPositionClickEventArgs(control.ToolbarPosition));
		}

	}
}
