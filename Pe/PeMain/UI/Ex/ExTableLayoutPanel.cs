namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExTableLayoutPanel: TableLayoutPanel
	{ }

	public class ToolbarPositionTableLayoutPanel: ExTableLayoutPanel, ISetCommonData, ICommonData
	{
		public ToolbarPositionTableLayoutPanel()
		{
			Initialize();
		}

		#region property

		public Button CommandDesktopLeft { get; private set; }
		public Button CommandDesktopRight { get; private set; }
		public Button CommandDesktopTop { get; private set; }
		public Button CommandDesktopBottom { get; private set; }
		public Button CommandFloat { get; private set; }

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
			CommandFloat = new Button() { };
			CommandDesktopLeft = new Button() { };
			CommandDesktopRight = new Button() { };
			CommandDesktopTop = new Button() { };
			CommandDesktopBottom = new Button() { };

			Controls.Add(this.CommandDesktopTop, 1, 0);
			Controls.Add(this.CommandDesktopLeft, 0, 1);
			Controls.Add(this.CommandDesktopRight, 1, 1);
			Controls.Add(this.CommandFloat, 2, 1);
			Controls.Add(this.CommandDesktopBottom, 1, 2);
		}

		#endregion

		#region function
		#endregion
	}
}
