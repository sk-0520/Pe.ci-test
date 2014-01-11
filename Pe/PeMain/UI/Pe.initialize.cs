/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_initialize.
	/// </summary>
	public partial class Pe
	{
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(string[] args, List<LogItem> initLog)
		{
			//var mainSettingPath = Literal.UserMainSettingPath;
			var mainSettingFilePath = @"Z:mainsetting.xml";
			initLog.Add(new LogItem(LogType.Information, "main-setting", mainSettingFilePath));
			this._mainSetting = Initializer.GetMainSetting(mainSettingFilePath);
			
			var languageFileName = "default.xml";
			var languageFilePath = Path.Combine(Literal.PeLanguageDirPath, languageFileName);
			initLog.Add(new LogItem(LogType.Information, "language", mainSettingFilePath));
			this._language = Initializer.GetLanguage(languageFilePath);
		}
		
		void InitializeMessage(string[] args, List<LogItem> initLog)
		{
			this._messageWindow = new MessageWindow(this);
		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem(this._language["main/menu/setting"], (object sender, EventArgs e) => {
				             	var f = new SettingForm(this._language, this._mainSetting);
				             	PauseOthers(() => {
				             	            	if(f.ShowDialog() == DialogResult.OK) {
				             	            		using(var stream = new FileStream(@"Z:mainsetting.xml", FileMode.Create)) {
				             	            			var serializer = new XmlSerializer(typeof(MainSetting));
				             	            			serializer.Serialize(stream, f.MainSetting);
				             	            		}
				             	            	}
				             	            }
				             	           );
				}),
				new MenuItem(this._language["common/menu/exit"], menuExitClick),
			};
			return menu;
		}
		
		/// <summary>
		/// 本体UI初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeMain(string[] args)
		{
			this._notifyIcon = new NotifyIcon();
			this._notificationMenu = new ContextMenu(InitializeMenu());
			
			this._notifyIcon.DoubleClick += IconDoubleClick;
			this._notifyIcon.Visible = true;
			
			this._notifyIcon.Icon = global::PeMain.Properties.Images.Pe;
			this._notifyIcon.ContextMenu = this._notificationMenu;			
		}
		
		void InitializeLogForm(string[] args, List<LogItem> initLog)
		{
			this._logForm = new LogForm(initLog);
			this._logForm.SetSettingData(this._language, this._mainSetting);
			
			this._logForm.Size = this._mainSetting.Log.Size;
			this._logForm.Location = this._mainSetting.Log.Point;
			this._logForm.Visible = this._mainSetting.Log.Visible;
		}
			
		void InitializeCommandForm(string[] args)
		{
			
		}
		
		void InitializeToolbarForm(string[] args)
		{
			Debug.Assert(this._mainSetting != null);
			
			this._toolbarForm = new ToolbarForm();
			this._toolbarForm.Logger = this._logForm;
			this._toolbarForm.SetSettingData(this._language, this._mainSetting);
		}

		void InitializeUI(string[] args, List<LogItem> initLog)
		{
			initLog.Add(new LogItem(LogType.Information, this._language["log/init/ui"], this._language["log/start"]));
			            
			InitializeMain(args);
			InitializeLogForm(args, initLog);
			InitializeCommandForm(args);
			InitializeToolbarForm(args);
			
			this._logForm.Puts(LogType.Information, this._language["log/init/ui"], this._language["log/start"]);
		}
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="args"></param>
		void Initialize(string[] args)
		{
			var initLog = new List<LogItem>(new []{ new LogItem(LogType.Information, "Initialize", args) });
			
			InitializeSetting(args, initLog);
			InitializeMessage(args, initLog);
			InitializeUI(args, initLog);
		}
	}
}
