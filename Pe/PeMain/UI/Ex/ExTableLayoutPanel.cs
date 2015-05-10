namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

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
				//command.FlatStyle = FlatStyle.Popup;
				command.Click += command_Click;
			}

			Controls.Add(this.CommandDesktopTop, 1, 0);
			Controls.Add(this.CommandDesktopLeft, 0, 1);
			Controls.Add(this.CommandDesktopFloat, 1, 1);
			Controls.Add(this.CommandDesktopRight, 2, 1);
			Controls.Add(this.CommandDesktopBottom, 1, 2);
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
		{ }

		void ApplyLanguage()
		{ }

		#endregion

		void command_Click(object sender, EventArgs e)
		{
			var command = (ToolbarPositionRadioButton)sender;
			ToolbarPositionClick(this, new ToolbarPositionClickEventArgs(command.ToolbarPosition));
		}

	}
}
