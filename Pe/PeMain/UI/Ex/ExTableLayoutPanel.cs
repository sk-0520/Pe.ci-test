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
		#region event

		public event EventHandler<ToolbarPositionClickEventArgs> ToolbarPositionClick = delegate { };

		#endregion

		public ToolbarPositionTableLayoutPanel()
		{
			Initialize();
		}

		#region property

		public ToolbarPositionButton CommandDesktopFloat { get; private set; }
		public ToolbarPositionButton CommandDesktopLeft { get; private set; }
		public ToolbarPositionButton CommandDesktopRight { get; private set; }
		public ToolbarPositionButton CommandDesktopTop { get; private set; }
		public ToolbarPositionButton CommandDesktopBottom { get; private set; }

		public CommonData CommonData { get; private set; }

		#endregion

		#region ISetCommonData

		public void SetCommonData(CommonData commonData)
		{

		}

		#endregion

		#region initialize

		void Initialize()
		{
			CommandDesktopFloat = new ToolbarPositionButton() {
				ToolbarPosition = ToolbarPosition.DesktopFloat,
			};
			CommandDesktopLeft = new ToolbarPositionButton() {
				ToolbarPosition = ToolbarPosition.DesktopLeft,
			};
			CommandDesktopRight = new ToolbarPositionButton() {
				ToolbarPosition = ToolbarPosition.DesktopRight,
			};
			CommandDesktopTop = new ToolbarPositionButton() {
				ToolbarPosition = ToolbarPosition.DesktopTop,
			};
			CommandDesktopBottom = new ToolbarPositionButton() {
				ToolbarPosition = ToolbarPosition.DesktopBottom,
			};

			foreach(var command in GetCommands()) {
				command.TabStop = false;
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

		IEnumerable<ToolbarPositionButton> GetCommands()
		{
			return new[] {
				CommandDesktopFloat,
				CommandDesktopLeft,
				CommandDesktopRight,
				CommandDesktopTop,
				CommandDesktopBottom,
			};
		}

		#endregion

		void command_Click(object sender, EventArgs e)
		{
			var command = (ToolbarPositionButton)sender;
			ToolbarPositionClick(this, new ToolbarPositionClickEventArgs(command.ToolbarPosition));
		}

	}
}
