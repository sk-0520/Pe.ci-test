namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ObjectDumper;

	/// <summary>
	/// ログ。
	/// </summary>
	public partial class LogForm : Form, ILogger, ISetCommonData
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		static int LogTypeToImageIndex(LogType logType)
		{
			switch(logType) {
				case LogType.Information: return 0;
				case LogType.Warning: return 1;
				case LogType.Error: return 2;
				default:
					Debug.Assert(false, logType.ToString());
					return -1;
			}
		}
		#endregion ////////////////////////////////////

		#region variable
		//List<LogItem> _logs = new List<LogItem>();
		FixedSizedList<LogItem> _logs = new FixedSizedList<LogItem>(Literal.logListLimit);
		ImageList _imageLogType = null;
		FileLogger _fileLogger = null;
		bool _refresh = false;
		#endregion ////////////////////////////////////

		public LogForm(FileLogger fileLogger)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this._fileLogger = fileLogger;
			
			Initialize();
		}

		#region property
		CommonData CommonData { get; set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}
		#endregion ////////////////////////////////////

		#region ILogger
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			if(InvokeRequired) {
				BeginInvoke((MethodInvoker)delegate() { Puts(logType, title, detail, frame); });
				return;
			}
			var logItem = new LogItem(logType, title, detail, frame);
			this._fileLogger.WiteItem(logItem);
			/*
			if(this._logs.Count >= Literal.logListLimit) {
				this._logs.RemoveAt(0);
			}
			*/
			this._logs.Add(logItem);
			var isCreated = Created;

			if(!Visible && CommonData.MainSetting.Log.AddShow && ((CommonData.MainSetting.Log.AddShowTrigger & logType) == logType)) {
				WindowsUtility.ShowNoActive(this);
				Visible = true;
				this._refresh = true;
			}

			ShowLast();
		}
		#endregion ////////////////////////////////////

		#region override
		//TODO: なんでこれ指定してんだろう
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
				return createParams;
			}
		}
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			// イメージリストをリソースから構築
			this._imageLogType = new ImageList();
			this._imageLogType.ColorDepth = ColorDepth.Depth32Bit;
			//this.listLog.LargeImageList = this._imageLogType;

		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);

			//Text = CommonData.Language["window/log"];
			/*
			this.toolLog_save.Text = CommonData.Language["log/command/save"];
			this.toolLog_save.ToolTipText = CommonData.Language["log/tips/save"];
			this.toolLog_clear.Text = CommonData.Language["log/command/clear"];
			this.toolLog_clear.ToolTipText = CommonData.Language["log/tips/save"];
			
			this.listLog_columnTimestamp.Text = CommonData.Language["log/header/timestamp"];
			this.listLog_columnTitle.Text = CommonData.Language["log/header/title"];
			this.listStack_columnFile.Text = CommonData.Language["log/header/file"];
			this.listStack_columnLine.Text = CommonData.Language["log/header/line"];
			this.listStack_columnFunction.Text = CommonData.Language["log/header/method"];
			*/
			//UIUtility.SetDefaultText(this, CommonData.Language);
			Text = CommonData.Language["window/log"];
#if DEBUG
			Text = "(DEBUG) " + Text;
#endif

			this.toolLog_save.Text = CommonData.Language["log/command/save"];
			this.toolLog_save.ToolTipText = CommonData.Language["log/tips/save"];
			this.toolLog_clear.Text = CommonData.Language["log/command/clear"];
			this.toolLog_clear.ToolTipText = CommonData.Language["log/tips/clear"];

			this.listLog_columnTimestamp.Text = CommonData.Language["log/header/timestamp"];
			this.listLog_columnTitle.Text = CommonData.Language["log/header/title"];

			this.listStack_columnFile.Text = CommonData.Language["log/header/method"];
			this.listStack_columnLine.Text = CommonData.Language["log/header/file"];
			this.listStack_columnFunction.Text = CommonData.Language["log/header/title"];
		}
		#endregion ////////////////////////////////////

		#region skin
		void ApplySkin()
		{
			this.toolLog_save.Image = CommonData.Skin.GetImage(SkinImage.Save);
			this.toolLog_clear.Image = CommonData.Skin.GetImage(SkinImage.Clear);

			this._imageLogType.Images.Clear();
			this._imageLogType.ImageSize = IconScale.Small.ToSize();
			this._imageLogType.Images.Add(LogType.Information.ToString(), CommonData.Skin.GetImage(SkinImage.Information));
			this._imageLogType.Images.Add(LogType.Warning.ToString(), CommonData.Skin.GetImage(SkinImage.Warning));
			this._imageLogType.Images.Add(LogType.Error.ToString(), CommonData.Skin.GetImage(SkinImage.Error));
			this.listLog.SmallImageList = this._imageLogType;
		}
		#endregion ////////////////////////////////////

		#region function
		public void PutsList(IEnumerable<LogItem> logs, bool show)
		{
			if(logs.Count() >= Literal.logListLimit) {
				logs = logs.Skip(logs.Count() - Literal.logListLimit);
			}
			this._refresh = true;
			this._logs.AddRange(logs);

			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			this.listLog.EnsureVisible(this._logs.Count - 1);

			if(CommonData.MainSetting.Log.AddShow && !Visible) {
				Visible = show;
			}
		}

		void ShowLast()
		{

			if(Created) {
				this.listLog.VirtualListSize = this._logs.Count;
				this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

				this.listLog.Items[this.listLog.Items.Count - 1].Focused = true;
				this.listLog.Items[this.listLog.Items.Count - 1].EnsureVisible();

				this.listLog.Refresh();
			}
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
			ApplySkin();
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
				statusLog_itemDetail.Image = CommonData.Skin.GetImage(SkinImage.SideExpand);
				statusLog_itemDetail.Text = CommonData.Language["log/label/detail-full"];
			} else {
				statusLog_itemDetail.Image = CommonData.Skin.GetImage(SkinImage.SideContract);
				statusLog_itemDetail.Text = CommonData.Language["log/label/detail-split"];
			}

			CommonData.MainSetting.Log.FullDetail = fullDetail;
			this.panelDetail.Panel2Collapsed = fullDetail;
		}

		void SwitchDetail()
		{
			ChangeDetail(!CommonData.MainSetting.Log.FullDetail);
		}

		#endregion ////////////////////////////////////
		
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
				CommonData.MainSetting.Log.Visible = false;
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
							stream.WriteLine(new ContentTypeTextNet.Pe.PeMain.Logic.AppInformation().ToString());
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

		private void LogForm_SizeChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Log.Size = Size;
		}

		private void LogForm_LocationChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Log.Point = Location;
		}
	}
}
