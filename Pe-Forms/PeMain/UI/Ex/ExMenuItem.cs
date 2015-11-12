namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExMenuItem: MenuItem
	{ }

	/// <summary>
	/// 共通データを保持するメニューアイテム。
	/// </summary>
	/// <param name="commonData"></param>
	public abstract class CommonDataMenuItem: ExMenuItem, ICommonData
	{
		public CommonDataMenuItem(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}


	public class ToolbarGroupItemMenuItem: CommonDataMenuItem, IToolbarGroupItem
	{
		public ToolbarGroupItemMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public ToolbarGroupItem ToolbarGroupItem { get; set; }
	}
}
