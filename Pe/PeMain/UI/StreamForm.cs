namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// 標準出力取得。
	/// </summary>
	public partial class StreamForm : CommonForm
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		//StreamWriter _inputStream;
		#endregion ////////////////////////////////////

		public StreamForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}


		#region property
		//CommonData CommonData { get; set; }
		Process Process { get; set; }
		public LauncherItem LauncherItem { get; private set; }
		public bool ProcessRunning { get { return Process != null && !Process.HasExited; } }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplySetting();
		//}

		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{ }

		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			var map = new Dictionary<string, string>() {
				{ AppLanguageName.itemName, LauncherItem.Name },
			};

			UIUtility.SetDefaultText(this, CommonData.Language, map);

			this.tabStream_pageStream.SetLanguage(CommonData.Language);
			this.tabStream_pageProcess.SetLanguage(CommonData.Language);
			this.tabStream_pageProperty.SetLanguage(CommonData.Language);

			this.toolStream_itemSave.SetLanguage(CommonData.Language);
			this.toolStream_itemClear.SetLanguage(CommonData.Language);
			this.toolStream_itemRefresh.SetLanguage(CommonData.Language);
			this.toolStream_itemTopmost.SetLanguage(CommonData.Language);
			this.toolStream_itemKill.SetLanguage(CommonData.Language);
		}
		#endregion ////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			base.ApplySkin();

			toolStream_itemTopmost.Image = CommonData.Skin.GetImage(SkinImage.Pin);
			toolStream_itemSave.Image = CommonData.Skin.GetImage(SkinImage.Save);
			toolStream_itemClear.Image = CommonData.Skin.GetImage(SkinImage.Clear);
			toolStream_itemRefresh.Image = CommonData.Skin.GetImage(SkinImage.Refresh);
			toolStream_itemKill.Image = CommonData.Skin.GetImage(SkinImage.Kill);
		}
		#endregion ////////////////////////////////////

		#region function
		public void SetParameter(Process process, LauncherItem launcherItem)
		{
			Process = process;
			LauncherItem = launcherItem;

			Process.EnableRaisingEvents = true;
			Process.Exited += new EventHandler(Process_Exited);

			// アイコン設定、アイテムに設定されているアイコンとは別に実行プロセスのアイコンを指定する
			try {
				var iconPath = Environment.ExpandEnvironmentVariables(LauncherItem.Command);
				if(PathUtility.HasIconPath(iconPath)) {
					Icon = IconUtility.Load(iconPath, IconScale.Normal, 0);
				} else {
					Icon = new Icon(iconPath);
				}
			} catch(Exception ex) {
				Debug.WriteLine(ex);
				Icon = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_App;
			}
		}

		protected override void ApplySetting()
		{
			base.ApplySetting();

			this.propertyProcess.SelectedObject = Process;
			this.propertyProperty.SelectedObject = Process.StartInfo;

			Process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
			Process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);

			//this._inputStream = Process.StandardInput;

			this.inputOutput.Font = CommonData.MainSetting.Launcher.StreamFontSetting.Font;
		}

		void OutputStreamReceived(string line, bool stdOutput)
		{
			/* //#20 retry
			if(IsDisposed) {
				// #20
				return;
			}
			 */

			this.inputOutput.BeginInvoke(
				(MethodInvoker)delegate() {
				this.inputOutput.Text += line + Environment.NewLine;
				this.inputOutput.SelectionStart = this.inputOutput.TextLength;
				this.inputOutput.ScrollToCaret();
			}
			);
		}

		void RefreshProperty()
		{
			// #21
			Process.Refresh();
		}

		void ExitedProcess()
		{
			/* //#20 retry
			if(IsDisposed) {
				// #20
				return;
			}
			 */

			this.toolStream_itemKill.Enabled = false;
			this.toolStream_itemClear.Enabled = false;
			this.toolStream_itemRefresh.Enabled = false;
			this.inputOutput.ReadOnly = true;
			RefreshProperty();

			Text += String.Format(": {0}", Process.ExitCode);
		}

		void KillProcess()
		{
			try {
				if(Process.HasExited) {
					return;
				}
				Process.Kill();
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
			}
		}

		/// <summary>
		/// #22
		/// </summary>
		/// <param name="path"></param>
		void SaveStream(string path)
		{
			using(var stream = new StreamWriter(new FileStream(path, FileMode.Create))) {
				stream.Write(this.inputOutput.Text);
			}
		}

		void SwitchTopmost()
		{
			this.toolStream_itemTopmost.Checked = !this.toolStream_itemTopmost.Checked;
			TopMost = this.toolStream_itemTopmost.Checked;
		}
		#endregion ////////////////////////////////////

		void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, true);
		}

		void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, false);
		}
		
		
		void ToolStream_refresh_Click(object sender, EventArgs e)
		{
			RefreshProperty();
		}
		
		void Process_Exited(object sender, EventArgs e)
		{
			if(InvokeRequired) {
				Invoke((MethodInvoker)ExitedProcess);
			} else {
				ExitedProcess();
			}
		}
		
		void ToolStream_kill_Click(object sender, EventArgs e)
		{
			KillProcess();
		}
		
		void ViewOutput_TextChanged(object sender, EventArgs e)
		{
			var hasText = this.inputOutput.TextLength > 0;
			this.toolStream_itemSave.Enabled = hasText;
			this.toolStream_itemClear.Enabled = hasText;
		}
		
		void ToolStream_clear_Click(object sender, EventArgs e)
		{
			// #22
			this.inputOutput.Clear();
		}
		
		void ToolStream_save_Click(object sender, EventArgs e)
		{
			using(var dialog = new SaveFileDialog()) {
				dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				dialog.FileName = Literal.NowTimestampFileName + ".output.log";
				dialog.Filter = "*.output.log|*.output.log";
				if(dialog.ShowDialog() == DialogResult.OK) {
					var path = dialog.FileName;
					SaveStream(path);
				}
			}
		}
		
		void ToolStream_itemTopmost_Click(object sender, EventArgs e)
		{
			SwitchTopmost();
		}

		void ViewOutput_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Debug.WriteLine((int)e.KeyChar);
			if(e.KeyChar == 0x0a || e.KeyChar == 0x0d) {
				Process.StandardInput.WriteLine();
			} else {
				Process.StandardInput.Write(e.KeyChar);
			}
		}
		
		void StreamForm_Shown(object sender, EventArgs e)
		{
			//this.toolStream_itemTopmost.Checked = TopMost;
			this.inputOutput.Focus();
		}
		
		void StreamForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(!Process.HasExited) {
				e.Cancel = true;
				CommonData.Logger.Puts(LogType.Warning, CommonData.Language["stream/running-close"], Process.ToString());
			}
		}
	}
}
