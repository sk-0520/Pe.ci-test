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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Navigation;
using PeMain.Data;
using PeUtility;

namespace PeMain.Logic
{
	public static class DialogUtility
	{
		public static void OpenDialogFilePath(Control input, bool whitespaceIsQuotation = true)
		{
			var path = input.Text.Trim();
			using(var dialog = new OpenFileDialog()) {
				if(path.Length > 0 && File.Exists(path)) {
					dialog.InitialDirectory = Path.GetDirectoryName(path);
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					var filePath = dialog.FileName;
					if(whitespaceIsQuotation) {
						filePath = (new []{filePath}).WhitespaceToQuotation().First();
					}
					input.Text = filePath;
				}
			}
		}
		
		public static void OpenDialogDirPath(Control input)
		{
			var path = input.Text.Trim();
			using(var dialog = new FolderBrowserDialog()) {
				dialog.ShowNewFolderButton = true;
				
				if(path.Length > 0 && Directory.Exists(path)) {
					dialog.SelectedPath = path;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.SelectedPath;
				}
			}
		}
		
		public static void SetDefaultText(Form target, Language language, string title)
		{
			target.Text = title;
			
			var acceptButton = target.AcceptButton as Button;
			if(acceptButton != null) {
				acceptButton.Text = language["common/command/ok"];
			}
			
			var cancelButton = target.CancelButton as Button;
			if(cancelButton != null) {
				cancelButton.Text = language["common/command/cancel"];
			}
		}
	}
	
	public static class TreeViewUtility
	{
		public static IList<TreeNode> GetChildrenNodes(this TreeView treeView)
		{
			var result = new List<TreeNode>();
			foreach(TreeNode node in treeView.Nodes) {
				result.Add(node);
				var nodes = node.GetChildrenNodes();
				if(nodes.Count > 0) {
					nodes.AddRange(nodes);
				}
			}
			
			return result;
		}
	}
	
	public static class TreeNoteUtility
	{
		/// <summary>
		/// 対象の子ノードをすべて取得
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
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
			Debug.Assert(fromNode != null);
			Debug.Assert(toParent != null);
			
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
			if(parentNode == null) {
				return;
			}
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
