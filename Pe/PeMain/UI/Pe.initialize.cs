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
using System.Globalization;
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
		void InitializeLanguage(string[] args, List<LogItem> initLog)
		{
			// 言語
			var langName = this._mainSetting.LanguageFileName;
			var languageFileName = "default.xml";
			if(!string.IsNullOrEmpty(langName)) {
				if(!Path.HasExtension(langName)) {
					languageFileName = Path.ChangeExtension(langName, "xml");
				} else {
					languageFileName = langName;
				}
			} else {
				var a = CultureInfo.CurrentCulture;
				languageFileName = Path.ChangeExtension(CultureInfo.CurrentCulture.Name, "xml");
			}
			var languageFilePath = Path.Combine(Literal.PeLanguageDirPath, languageFileName);
			if(initLog != null) {
				initLog.Add(new LogItem(LogType.Information, "language", languageFilePath));
			}
			this._language = Initializer.GetLanguage(languageFilePath);
			if(this._language == null) {
				if(initLog != null) {
					initLog.Add(new LogItem(LogType.Warning, "not found language", languageFilePath));
				}
				this._language = new Language();
			}
		}
		
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(string[] args, List<LogItem> initLog)
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			initLog.Add(new LogItem(LogType.Information, "main-setting", mainSettingFilePath));
			this._mainSetting = Initializer.GetMainSetting(mainSettingFilePath);
			
			InitializeLanguage(args, initLog);
		}
		
		void InitializeMessage(string[] args, List<LogItem> initLog)
		{
			this._messageWindow = new MessageWindow(this);
		}
		
		MenuItem[] CreateWindowMenu()
		{
			var menuList = new List<MenuItem>();
			var itemToolbar = new MenuItem();
			var itemLogger = new MenuItem();
			
			menuList.Add(itemToolbar);
			menuList.Add(itemLogger);
			
			itemToolbar.Text = this._language["main/menu/window/toolbar"];
			itemToolbar.Name = menuNameWindowToolbar;
			itemToolbar.Click += (object sender, EventArgs e) => {
				this._toolbarForm.Visible = !this._toolbarForm.Visible;
				this._mainSetting.Toolbar.Visible = this._toolbarForm.Visible;
			};
			
			itemLogger.Text = this._language["main/menu/window/logger"];
			itemLogger.Name = menuNameWindowLogger;
			itemLogger.Click += (object sender, EventArgs e) => {
				this._logForm.Visible = !this._logForm.Visible; 
				this._mainSetting.Log.Visible = this._logForm.Visible;
			};
			
			return menuList.ToArray();
		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private MenuItem[] InitializeMenu()
		{
			var menuList = new List<MenuItem>();
			var itemWindow = new MenuItem();
			var itemSetting = new MenuItem();
			var itemExit = new MenuItem();
			
			menuList.Add(itemWindow);
			menuList.Add(itemSetting);
			menuList.Add(itemExit);
			
			// ウィンドウ
			itemWindow.Text = this._language["main/menu/window"];
			itemWindow.Name = menuNameWindow;
			itemWindow.MenuItems.AddRange(CreateWindowMenu());
			itemWindow.Popup += (object sender, EventArgs e) => {
				itemWindow.MenuItems[menuNameWindowToolbar].Checked = this._toolbarForm.Visible;
				itemWindow.MenuItems[menuNameWindowLogger].Checked = this._logForm.Visible;
			};
			
			// 設定
			itemSetting.Text = this._language["main/menu/setting"];
			itemSetting.Name = menuNameSetting;
			itemSetting.Click += (object sender, EventArgs e) => {
				PauseOthers(() => OpenSetting());
			};
			
			// 終了
			itemExit.Text = this._language["common/menu/exit"];
			itemExit.Name = menuNameExit;
			itemExit.Click += (object sender, EventArgs e) => {
				CloseApplication(true);
			};

			return menuList.ToArray();;
		}
		
		/// <summary>
		/// 本体UI初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeMain(string[] args, List<LogItem> initLog)
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
			this._logForm = new LogForm();
			this._logForm.SetSettingData(this._language, this._mainSetting);
		}
			
		void InitializeCommandForm(string[] args, List<LogItem> initLog)
		{
			
		}
		
		void InitializeToolbarForm(string[] args, List<LogItem> initLog)
		{
			Debug.Assert(this._mainSetting != null);
			
			this._toolbarForm = new ToolbarForm();
			this._toolbarForm.Logger = this._logForm;
			this._toolbarForm.SetSettingData(this._language, this._mainSetting);
		}

		void InitializeUI(string[] args, List<LogItem> initLog)
		{
			initLog.Add(new LogItem(LogType.Information, this._language["log/init/ui"], this._language["log/start"]));
			            
			InitializeMain(args, initLog);
			InitializeLogForm(args, initLog);
			InitializeCommandForm(args, initLog);
			InitializeToolbarForm(args, initLog);
			
			initLog.Add(new LogItem(LogType.Information, this._language["log/init/ui"], this._language["log/start"]));
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
			
			this._logForm.PutsList(initLog, false);
		}
	}
}
