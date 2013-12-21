/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
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
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(string[] args)
		{
			this.mainSetting = Initializer.GetMainSetting(Literal.UserMainSettingPath);
			this.language = Initializer.GetLanguage(Path.Combine(Literal.PeLanguageDirPath, "default.xml"));
		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem(this.language["main/menu/setting"], (object sender, EventArgs e) => {
				             	var f = new SettingForm(this.language, this.mainSetting);
				             	PauseOthers(() => f.ShowDialog());
				}),
				new MenuItem(this.language["common/menu/exit"], menuExitClick),
			};
			return menu;
		}
		
		/// <summary>
		/// 本体UI初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeUI(string[] args)
		{
			this.notifyIcon = new NotifyIcon();
			this.notificationMenu = new ContextMenu(InitializeMenu());
			
			this.notifyIcon.DoubleClick += IconDoubleClick;
			this.notifyIcon.Visible = true;
			
			this.notifyIcon.Icon = global::PeMain.Properties.Images.Pe;
			this.notifyIcon.ContextMenu = this.notificationMenu;
		}
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="args"></param>
		void Initialize(string[] args)
		{
			InitializeSetting(args);
			InitializeUI(args);
		}
	}
}
