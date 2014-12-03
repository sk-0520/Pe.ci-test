﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/10/2014
 * 時刻: 23:49
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// ログ。
	/// </summary>
	public partial class LogForm : Form, ILogger, ISetCommonData
	{
		public LogForm(FileLogger fileLogger)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this._fileLogger = fileLogger;
			
			Initialize();
		}
		
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
				return createParams;
			}
		}
		static int LogTypeToImageIndex(LogType logType) {
			switch(logType) {
					case LogType.Information: return 0;
					case LogType.Warning: return 1;
					case LogType.Error: return 2;
				default:
					Debug.Assert(false, logType.ToString());
					return -1;
			}
		}
		
		
		void ListLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			if (e.ItemIndex < this._logs.Count || this._refresh) {
				if(e.Item == null) {
					e.Item = new ListViewItem();
					e.Item.SubItems.Add(new ListViewItem.ListViewSubItem());
				} else if(e.Item.Index == -1) {
					return;
				}
				if(this._logs.Count == 0) {
					return;
				}
				var logItem = this._logs[e.ItemIndex];
				e.Item.ImageIndex = LogTypeToImageIndex(logItem.LogType);
				
				var dateItem = e.Item;
				var titleItem = e.Item.SubItems[1];
				
				dateItem.Text = logItem.DateTime.ToString();
				//dateItem.ImageKey = logItem.LogType.ToString();
				titleItem.Text = logItem.Title;
			}
		}
		
		void LogForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}
		
		void ListLog_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearDetail();
			if(this.listLog.FocusedItem != null && 0 <= this._logs.Count && this.listLog.FocusedItem.Index <= this._logs.Count) {
				var listItem = this.listLog.FocusedItem;
				var logItem = this._logs[listItem.Index];
				SetDetail(logItem);
			}
		}
		
		void ToolLog_clear_Click(object sender, EventArgs e)
		{
			this._logs.Clear();
			this.listLog.VirtualListSize = 0;
			this._refresh = true;
			this.listLog.Refresh();
		}
		
		void ToolLog_save_Click(object sender, EventArgs e)
		{
			using(var dialog = new SaveFileDialog()) {
				dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				dialog.FileName = Literal.NowTimestampFileName + ".log";
				dialog.Filter = "*.log|*.log";
				if(dialog.ShowDialog() == DialogResult.OK) {
					var path = dialog.FileName;
					//Debug.WriteLine(path);
					try {
						using(var stream = new StreamWriter(new FileStream(path, FileMode.Create))) {
							stream.WriteLine(new PeMain.Logic.AppInformation().ToString());
							foreach(var logItem in this._logs) {
								stream.WriteLine(logItem.ToString());
							}
						}
					} catch(Exception ex) {
						CommonData.Logger.Puts(LogType.Error, CommonData.Language["log/output/error"], ex);
					}
				}
			}
		}
		
		void StatusLog_itemDetail_Click(object sender, EventArgs e)
		{
			SwitchDetail();
		}

		private void LogForm_Shown(object sender, EventArgs e)
		{
			ShowLast();
		}
	}
}
