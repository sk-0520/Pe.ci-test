/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_initialize.
	/// </summary>
	public partial class Pe
	{
		void InitializeMessage(string[] args)
		{
			this._messageWindow = new MessageWindow(this);
		}

		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(string[] args)
		{
			//this._mainSetting = Initializer.GetMainSetting(Literal.UserMainSettingPath);
			this._mainSetting = Initializer.GetMainSetting(@"Z:mainsetting.xml");
			this._language = Initializer.GetLanguage(Path.Combine(Literal.PeLanguageDirPath, "default.xml"));
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
			
		void InitializeCommand(string[] args)
		{
			
		}
		
		void InitializeToolbar(string[] args)
		{
			Debug.Assert(this._mainSetting != null);
			
			this._toolbarForm = new ToolbarForm();
			this._toolbarForm.SetSettingData(this._language, this._mainSetting);
		}

		void InitializeUI(string[] args)
		{
			InitializeMain(args);
			InitializeCommand(args);
			InitializeToolbar(args);
		}
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="args"></param>
		void Initialize(string[] args)
		{
			InitializeMessage(args);
			InitializeSetting(args);
			InitializeUI(args);
		}
	}
}
