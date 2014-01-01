/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/01
 * 時刻: 16:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace PeMain.Logic
{
	public static class TreeViewUtility
	{
		
	}
	
	public static class TreeNoteUtility
	{
		public static List<TreeNode> GetChildrenNodes(this TreeNode parent)
		{
			var result = new List<TreeNode>();
			foreach(TreeNode node in parent.Nodes) {
				result.Add(node);
				if(node.Nodes.Count > 0) {
					var list = GetChildrenNodes(node);
					if(list.Count > 0) {
						result.AddRange(list);
					}
				}
			}
			return result;
		}
		
		/// <summary>
		/// ノード選択。
		/// 
		/// ユーザーコードでは基本的に使用しない
		/// </summary>
		/// <param name="node"></param>
		/// <param name="toSelect"></param>
		public static void ToSelect(this TreeNode node, bool toSelect)
		{
			if(toSelect) {
				var view = node.TreeView;
				view.SelectedNode = node;
			}
		}
		
		/// <summary>
		/// 対象ノードを指定ノードの子にする
		/// </summary>
		/// <param name="fromNode"></param>
		/// <param name="toParent">親となるノード</param>
		/// <param name="toSelect"></param>
		public static void MoveToChild(this TreeNode fromNode, TreeNode toParent, bool toSelect)
		{
			var tree = fromNode.TreeView;
			fromNode.Remove();
			toParent.Nodes.Add(fromNode);
			toParent.Expand();
			ToSelect(fromNode, toSelect);
		}
		
		/// <summary>
		/// 対象ノードを親ノード位置へ移動する
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToOut(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var superParentNode = parentNode.Parent;
			targetNode.Remove();
			superParentNode.Nodes.Insert(parentNode.Index + 1, targetNode);
			superParentNode.Expand();
			parentNode.Expand();
			ToSelect(targetNode, toSelect);
		}
		
		/// <summary>
		/// 上の兄弟を親ノードとする
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToIn(this TreeNode targetNode, bool toSelect)
		{
			var prevNode = targetNode.PrevNode;
			targetNode.Remove();
			prevNode.Nodes.Add(targetNode);
			prevNode.Expand();
			ToSelect(targetNode, toSelect);
		}

		/// <summary>
		/// 兄弟要素間で上に移動する。
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToUp(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var prevNode = targetNode.PrevNode;
			var nodes = parentNode != null ? parentNode.Nodes: targetNode.TreeView.Nodes; 
			if(prevNode == null) {
				return;
			}
			targetNode.Remove();
			nodes.Insert(prevNode.Index, targetNode);
			var tree = targetNode.TreeView;
			ToSelect(targetNode, toSelect);
		}
		
		/// <summary>
		/// 兄弟要素間で下に移動する。
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToDown(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var nextNode = targetNode.NextNode;
			var nodes = parentNode != null ? parentNode.Nodes: targetNode.TreeView.Nodes; 
			if(nextNode == null) {
				return;
			}
			targetNode.Remove();
			nodes.Insert(nextNode.Index + 1, targetNode);
			var tree = targetNode.TreeView;
			ToSelect(targetNode, toSelect);
		}
	}
}
