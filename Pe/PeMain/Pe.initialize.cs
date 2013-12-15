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

namespace PeMain
{
	/// <summary>
	/// Description of Pe_initialize.
	/// </summary>
	public partial class Pe
	{
		void Initialize(string[] args)
		{
			InitializeSetting(args);
			InitializeUI(args);
		}
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(string[] args)
		{
			this.mainSetting = new MainSetting();
			InitializeMainSetting(args, this.mainSetting);
		}
		void InitializeMainSetting(string[] args, MainSetting setting)
		{
			var settingPath = Literal.UserMainSettingPath;
			if(File.Exists(settingPath)) {
				var serializer = new XmlSerializer(typeof(MainSetting));
				using(var stream = new FileStream(settingPath, FileMode.Open)) {
					serializer.Serialize(stream, setting);
				}
			}
			
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		void InitializeUI(string[] args)
		{
			this.notifyIcon = new NotifyIcon();
			this.notificationMenu = new ContextMenu(InitializeMenu());
			
			this.notifyIcon.DoubleClick += IconDoubleClick;
			this.notifyIcon.Visible = true;
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pe));
			this.notifyIcon.Icon = global::PeMain.Properties.Images.Pe;
			this.notifyIcon.ContextMenu = this.notificationMenu;
		}
	}
}
