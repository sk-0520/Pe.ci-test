/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm_functions.
	/// </summary>
	public partial class LogForm
	{
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			this._logs.Add(logItem);
			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			
			if(LogSetting.AddShow && !Visible) {
				Visible = true;
			}
			
			this.listLog.Items[this.listLog.Items.Count - 1].Focused = true;
			this.listLog.Items[this.listLog.Items.Count - 1].EnsureVisible();
		}
		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			this._mainSetting = mainSetting;
			//LogSetting = mainSetting.Log;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(LogSetting != null);
			
			ApplyLanguage();
		}
		
		void ClearDetail()
		{
			this.treeDetail.Nodes.Clear();
			this.listStack.Items.Clear();
		}
		
		void ObjectToNode(object obj)
		{
			if(obj.GetType() == typeof(string)){}
			
		}
		
		ListViewItem StackToListItem(StackFrame sf)
		{
			var listItem = new ListViewItem();
			var fileItem = listItem.SubItems[0];
			var lineItem = new ListViewItem.ListViewSubItem();
			var funcItem = new ListViewItem.ListViewSubItem();
			listItem.SubItems.Add(lineItem);
			listItem.SubItems.Add(funcItem);
			
			var filePath = sf.GetFileName();
			var head = string.Join(Path.DirectorySeparatorChar.ToString(), "", "Pe", "Pe", "").ToUpper();
			if(!string.IsNullOrEmpty(filePath)) {
				var index = filePath.ToUpper().LastIndexOf(head);
				Debug.Assert(0 <= index);
				filePath = filePath.Substring(index);
			}
			
			fileItem.Text = filePath;
			lineItem.Text = string.Format("{0}:{1}", sf.GetFileLineNumber(), sf.GetFileColumnNumber());
			var method = sf.GetMethod();
			funcItem.Text = string.Format("{0}:{1}", method.ReflectedType, method.ToString());
						
			return listItem;
		}
		
		void SetDetail(LogItem logItem)
		{
			Debug.Assert(logItem != null);
			
			// 
			this.treeDetail.Nodes.Add(logItem.Detail.ToString());
			
			
			//
			var listitemList = new List<ListViewItem>(logItem.StackTrace.FrameCount);
			var st = logItem.StackTrace;
			for(var i = 0; i < st.FrameCount; i++) {
				listitemList.Add(StackToListItem(st.GetFrame(i)));
			}
			
			this.listStack.Items.AddRange(listitemList.ToArray());
		}
	}
}
