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

	public abstract class ExTreeNode: TreeNode
	{ }

	/// <summary>
	/// 何かするわけじゃないけど予約用。
	/// </summary>
	public class GroupItemTreeNode: ExTreeNode
	{ }

	/// <summary>
	/// ランチャーアイテムを保持するツリーノード。
	/// </summary>
	public class LauncherItemTreeNode: ExTreeNode, ILauncherItem
	{
		public LauncherItem LauncherItem { get; set; }
	}
}
