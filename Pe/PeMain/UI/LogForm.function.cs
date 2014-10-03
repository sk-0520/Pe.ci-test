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
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ObjectDumper;
using PeMain.Data;
using PeMain.Logic;
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
			if(InvokeRequired && Created) {
				BeginInvoke((MethodInvoker)delegate() { Puts(logType, title, detail, frame); });
				return;
			}
			var logItem = new LogItem(logType, title, detail, frame);
			this._fileLogger.WiteItem(logItem);
			if(this._logs.Count >= Literal.logListLimit) {
				this._logs.RemoveAt(0);
			}
			this._logs.Add(logItem);
			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			
			if(!Visible && CommonData.MainSetting.Log.AddShow && ((CommonData.MainSetting.Log.AddShowTrigger & logType) == logType)) {
				Visible = true;
				this._refresh = true;
			}
			
			this.listLog.Items[this.listLog.Items.Count - 1].Focused = true;
			this.listLog.Items[this.listLog.Items.Count - 1].EnsureVisible();
			
			this.listLog.Refresh();
		}
		
		public void PutsList(IEnumerable<LogItem> logs, bool show)
		{
			if(logs.Count() >= Literal.logListLimit) {
				logs = logs.Skip(logs.Count() - Literal.logListLimit);
			}
			this._refresh = true;
			this._logs.AddRange(logs);
			
			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

			if(CommonData.MainSetting.Log.AddShow && !Visible) {
				Visible = show;
			}
		}
		
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplyUI()
		{
			Size = CommonData.MainSetting.Log.Size;
			Location = CommonData.MainSetting.Log.Point;
			Visible = CommonData.MainSetting.Log.Visible;
			
			ChangeDetail(CommonData.MainSetting.Log.FullDetail);
		}
		
		void ApplySetting()
		{
			Debug.Assert(CommonData.MainSetting != null);
			
			ApplyLanguage();
			ApplyUI();
		}
		
		void ClearDetail()
		{
			this.viewDetail.Clear();
			this.listStack.Items.Clear();
		}
		
		
		ListViewItem StackToListItem(StackFrame sf)
		{
			var listItem = new ListViewItem();
			var funcItem = listItem.SubItems[0];
			var lineItem = new ListViewItem.ListViewSubItem();
			var fileItem = new ListViewItem.ListViewSubItem();
			listItem.SubItems.Add(lineItem);
			listItem.SubItems.Add(fileItem);
			
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
		
		/*
		IEnumerable<string> ObjectToStringList(object obj)
		{
			var result = new List<string>();
			var type = obj.GetType();
			
			if(type == typeof(string)) {
				return new [] { (string)obj };
			}

			var members = type.GetMembers(
				BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance
			)
				.Select(m => new { Name = m.Name, Type = m.GetType(), MemberType = m.MemberType})
				.Where(m => m.MemberType.IsIn(MemberTypes.Property, MemberTypes.Field))
				.OrderBy(m => m.Name)
				;
			
			foreach(var member in members)  {
				object value;
				if(member.MemberType == MemberTypes.Field) {
					var field = type.GetField(member.Name);
					if(field == null) {
						continue;
					}
					value = field.GetValue(obj);
				} else {
					Debug.Assert(member.MemberType == MemberTypes.Property);
					var prop = type.GetProperty(member.Name);
					if(prop == null) {
						continue;
					}
					value = prop.GetValue(obj);
				}
				var message = string.Format("{0}: {1}", member.Name, value);
				result.Add(message);
			}
			return result;
		}
		*/
		
		void SetDetail(LogItem logItem)
		{
			Debug.Assert(logItem != null);
			
			// 
			//this.viewDetail.Text = string.Join(Environment.NewLine, ObjectToStringList(logItem.Detail));
			if(logItem.Detail is Exception || logItem.Detail is string) {
				this.viewDetail.Text = logItem.Detail.ToString();
			} else {
				this.viewDetail.Text = logItem.Detail.DumpToString(logItem.Title);
			}
			
			//
			var listitemList = new List<ListViewItem>(logItem.StackTrace.FrameCount);
			var st = logItem.StackTrace;
			for(var i = 0; i < st.FrameCount; i++) {
				listitemList.Add(StackToListItem(st.GetFrame(i)));
			}
			
			this.listStack.Items.AddRange(listitemList.ToArray());
		}
		
		void ChangeDetail(bool fullDetail)
		{
			if(fullDetail) {
				statusLog_itemDetail.Image = PeMain.Properties.Images.SideExpand;
				statusLog_itemDetail.Text  = CommonData.Language["log/label/detail-full"];
			} else {
				statusLog_itemDetail.Image = PeMain.Properties.Images.SideContract;
				statusLog_itemDetail.Text  = CommonData.Language["log/label/detail-split"];
			}
			
			CommonData.MainSetting.Log.FullDetail = fullDetail;
			this.panelDetail.Panel2Collapsed = fullDetail;
		}

		void SwitchDetail()
		{
			ChangeDetail(!CommonData.MainSetting.Log.FullDetail);
		}
		
	}
}
