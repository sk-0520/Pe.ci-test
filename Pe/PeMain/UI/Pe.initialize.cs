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
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;
using PeMain.Logic;
using PeUtility;
using PI.Windows;

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
			this._messageWindow.SetSettingData(this._language, this._mainSetting);
		}
		
		void AttachmentToolbarSubMenu(MenuItem parentMenu)
		{
			var menuList = new List<MenuItem>();
			foreach(var screen in Screen.AllScreens) {
				var menuItem = new MenuItem();
				menuItem.Name = screen.DeviceName;
				menuItem.Text = ScreenUtility.ToScreenName(screen);
				menuItem.Click += (object sender, EventArgs e) => {
					var toolbar = this._toolbarForms[screen];
					toolbar.Visible = !toolbar.Visible;
					toolbar.UseToolbarItem.Visible = toolbar.Visible;
				};
				menuList.Add(menuItem);
			}
			
			// サブメニュー設定
			parentMenu.MenuItems.AddRange(menuList.ToArray());
			
			parentMenu.Popup += (object sender, EventArgs e) => {
				foreach(var screen in Screen.AllScreens) {
					if(parentMenu.MenuItems.ContainsKey(screen.DeviceName)) {
						var menuItem = parentMenu.MenuItems[screen.DeviceName];
						menuItem.Checked = this._toolbarForms[screen].Visible;
					}
				}
			};

		}
		
		void AttachmentWindowSubMenu(MenuItem parentMenu)
		{
			var menuList = new List<MenuItem>();
			var itemToolbar = new MenuItem();
			var itemLogger = new MenuItem();
			
			menuList.Add(itemToolbar);
			menuList.Add(itemLogger);
			
			itemToolbar.Name = menuNameWindowToolbar;
			AttachmentToolbarSubMenu(itemToolbar);
			/*
			itemToolbar.Click += (object sender, EventArgs e) => {
				this._toolbarForms.Visible = !this._toolbarForms.Visible;
				this._mainSetting.Toolbar.Visible = this._toolbarForms.Visible;
			};
			*/
			
			itemLogger.Name = menuNameWindowLogger;
			itemLogger.Click += (object sender, EventArgs e) => {
				this._logForm.Visible = !this._logForm.Visible; 
				this._mainSetting.Log.Visible = this._logForm.Visible;
			};
			
			// サブメニュー設定
			parentMenu.MenuItems.AddRange(menuList.ToArray());
			
			// ログ
			parentMenu.Popup += (object sender, EventArgs e) => {
				itemLogger.Checked = this._logForm.Visible;
			};
			
		}
		
		void AttachmentSystemEnvSubMenu(MenuItem parentMenu)
		{
			var menuList = new List<MenuItem>();
			var itemHiddenFile = new MenuItem();
			var itemExtension = new MenuItem();
			menuList.Add(itemHiddenFile);
			menuList.Add(itemExtension);
			
			// 隠しファイル
			itemHiddenFile.Name = menuNameSystemEnvHiddenFile;
			itemHiddenFile.Click += (object sender, EventArgs e) => {
				SystemEnv.SetHiddenfFileShow(!SystemEnv.IsHiddenfFileShow());
				SystemEnv.RefreshShell();
			};
			
			// 拡張子
			itemExtension.Name  = menuNameSystemEnvExtension;
			itemExtension.Click += (object sender, EventArgs e) => {
				SystemEnv.SetExtensionShow(!SystemEnv.IsExtensionShow());
				SystemEnv.RefreshShell();
			};
			
			// サブメニュー設定
			parentMenu.MenuItems.AddRange(menuList.ToArray());
			
			parentMenu.Popup += (object sender, EventArgs e) => {
				itemHiddenFile.Checked = SystemEnv.IsHiddenfFileShow();
				itemExtension.Checked = SystemEnv.IsExtensionShow();
			};

		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private MenuItem[] InitializeMenu()
		{
			var menuList = new List<MenuItem>();
			var itemWindow = new MenuItem();
			var itemSystemEnv = new MenuItem();
			var itemSetting = new MenuItem();
			var itemAbout = new MenuItem();
			var itemExit = new MenuItem();
			
			menuList.Add(itemAbout);
			menuList.Add(itemWindow);
			menuList.Add(itemSystemEnv);
			menuList.Add(itemSetting);
			menuList.Add(itemExit);
			
			// 情報
			itemAbout.Name = menuNameAbout;
			itemAbout.Click += (object sender, EventArgs e) => {
				MessageBox.Show("デュン！");
			};
			
			// ウィンドウ
			itemWindow.Name = menuNameWindow;
			AttachmentWindowSubMenu(itemWindow);

			// システム環境
			itemSystemEnv.Name = menuNameSystemEnv;
			AttachmentSystemEnvSubMenu(itemSystemEnv);

			// 設定
			itemSetting.Name = menuNameSetting;
			itemSetting.Click += (object sender, EventArgs e) => {
				PauseOthers(() => OpenSetting());
			};
			
			// 終了
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
			
			// ディスプレイ分生成
			foreach(var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
				var toolbar = new ToolbarForm();
				toolbar.Logger = this._logForm;
				toolbar.DockScreen = screen;
				toolbar.MessageString +=  screen.DeviceName;
				toolbar.SetSettingData(this._language, this._mainSetting);
				this._toolbarForms.Add(screen, toolbar);
			}
			/*
			this._toolbarForms = new ToolbarForm();
			this._toolbarForms.Logger = this._logForm;
			this._toolbarForms.SetSettingData(this._language, this._mainSetting);
			*/
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
			
			ApplyLanguage();
			
			this._logForm.PutsList(initLog, false);
		}
	}
}
