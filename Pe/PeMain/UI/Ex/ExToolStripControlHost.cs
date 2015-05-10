namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
using System.Windows.Forms;
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

	public class ToolbarPositionToolStripControl: ExToolStripControlHost, ISetCommonData, ICommonData
	{
		public ToolbarPositionToolStripControl(): base(new ToolbarPositionTableLayoutPanel())
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

		#endregion

		#region initialize

		void Initialize()
		{
			ToolbarPositionTableLayoutPanel = (ToolbarPositionTableLayoutPanel)Control;
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

	}
}
