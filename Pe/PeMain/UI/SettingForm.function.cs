/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:26
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using PeMain.Logic;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	partial class SettingForm
	{
		void AttachmentIconScale(ComboBox control, IconScale defaultData)
		{
			var iconSizeDataList = new List<IconScaleDisplayValue>();
			//foreach(var value in new [] { IconScale.Small, IconScale.Normal, IconScale.Big, IconScale.Large }) {
			foreach(var value in new [] { IconScale.Small, IconScale.Normal, IconScale.Big }) {
				var data = new IconScaleDisplayValue(value);
				data.SetLanguage(Language);
				iconSizeDataList.Add(data);
			}
			control.Attachment(iconSizeDataList, defaultData);
		}
		
		bool CheckValidate()
		{
			var checkResult = true;
			this.errorProvider.Clear();
			
			if(!LauncherItemValid()) {
				checkResult = false;
			}
			
			if(!NoteValid()) {
				checkResult = false;
			}
			
			return checkResult;
		}
		
		public void SaveFiles()
		{
			SaveFileMainStartup();
		}
		
		public void SaveDB(AppDBManager db)
		{
			using(var tran = db.BeginTransaction()) {
				try {
					SaveDBNoteItems(db);
					tran.Commit();
				} catch(Exception) {
					tran.Rollback();
					throw;
				}
			}
		}
		void CreateSettingData()
		{
			var mainSetting = new MainSetting();
			
			// 現在状況
			mainSetting.RunningInfo.Running = true;
			mainSetting.RunningInfo.SetDefaultVersion();
			
			// 本体
			MainExportSetting(mainSetting);
			
			// ランチャ
			LauncherExportSetting(mainSetting.Launcher);
			
			// コマンド
			CommandExportSetting(mainSetting.Command);
			
			// ツールバー
			ToolbarExportSetting(mainSetting.Toolbar);
			
			// ノート
			NoteExportSetting(mainSetting.Note);
			
			// ディスプレイ
			
			// プロパティ設定
			MainSetting = mainSetting;
		}
		
		/*
		string GetStartupAppLinkPath()
		{
			var startupDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			var appLinkPath = Path.Combine(startupDirPath, Literal.shortcutName);

			return appLinkPath;
		}
		*/
	}
}
