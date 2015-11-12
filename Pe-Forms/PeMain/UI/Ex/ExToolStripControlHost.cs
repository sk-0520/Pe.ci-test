namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExToolStripControlHost: ToolStripControlHost
	{
		/// <summary>
		/// 指定したコントロールをホストする ToolStripControlHost クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="Control"></param>
		public ExToolStripControlHost(Control c): base(c)
		{ }
		/// <summary>
		/// 指定したコントロールをホストし、指定した名前を持つ ToolStripControlHost クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="Control"></param>
		/// <param name="String"></param>
		public ExToolStripControlHost(Control c, String s): base(c, s)
		{ }
	}

	public class ToolbarPositionChangedEventArgs: EventArgs
	{
		public ToolbarPositionChangedEventArgs(ToolbarPosition toolbarPosition)
		{
			ToolbarPosition = toolbarPosition;
		}

		public ToolbarPosition ToolbarPosition { get; private set; }
		public bool Cancel { get; set; }
	}

	public class ToolbarPositionToolStripControl: ExToolStripControlHost, ISetCommonData, ICommonData
	{
		#region event

		public event EventHandler<ToolbarPositionChangedEventArgs> ToolbarPositionChanged = delegate { };

		#endregion

		public ToolbarPositionToolStripControl()
			: base(new ToolbarPositionTableLayoutPanel())
		{
			Initialize();
		}
		public ToolbarPositionToolStripControl(String s): base(new ToolbarPositionTableLayoutPanel(), s)
		{
			Initialize();
		}

		#region property

		public CommonData CommonData { get; private set; }
		protected ToolbarPositionTableLayoutPanel ToolbarPositionTableLayoutPanel { get; private set; }

		public ToolbarPosition ToolbarPosition
		{
			get { return ToolbarPositionTableLayoutPanel.ToolbarPosition; }
			set { ToolbarPositionTableLayoutPanel.ToolbarPosition = value; }
		}

		#endregion

		#region initialize

		void Initialize()
		{
			ToolbarPositionTableLayoutPanel = (ToolbarPositionTableLayoutPanel)Control;
			ToolbarPositionTableLayoutPanel.ToolbarPositionClick += ToolbarPositionTableLayoutPanel_ToolbarPositionChanged;
		}

		#endregion

		#region ISetCommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			ToolbarPositionTableLayoutPanel.SetCommonData(CommonData);
		}

		#endregion

		#region function
		#endregion

		void ToolbarPositionTableLayoutPanel_ToolbarPositionChanged(object sender, ToolbarPositionClickEventArgs e)
		{
			var mainEvent = new ToolbarPositionChangedEventArgs(e.ToolbarPosition);

			var menu = Owner as ToolStripDropDown;
			if(menu != null) {
				menu.Close();
			}

			ToolbarPositionChanged(this, mainEvent);
		}

	}
}
