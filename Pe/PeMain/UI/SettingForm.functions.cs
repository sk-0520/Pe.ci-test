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
	public partial class SettingForm
	{
		void AttachmentIconSize(ComboBox control, IconSize defaultData)
		{
			var iconSizeList = new List<IconSizeItem>();
			//foreach(var value in new [] { IconSize.Small, IconSize.Normal, IconSize.Big, IconSize.Large }) {
			foreach(var value in new [] { IconSize.Small, IconSize.Normal, IconSize.Big }) {
				var data = new IconSizeItem(value, Language);
				iconSizeList.Add(data);
			}
			control.Attachment(iconSizeList, defaultData);
		}
		
		void SetViewMessage(Control viewControl, FontSetting fontSetting)
		{
			string viewText = Language["common/command/default-font"];
			if(fontSetting != null && !fontSetting.IsDefault) {
				viewText = string.Format("{0} {1}", fontSetting.Family, fontSetting.Height);
			}
			viewControl.Text = viewText;
		}
		
		FontSetting OpenDialogFontSetting(Control viewControl, FontSetting fontSetting)
		{
			using(var dialog = new FontDialog()) {
				if(fontSetting != null && !fontSetting.IsDefault) {
					dialog.Font = fontSetting.Font;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					var result = new FontSetting();
					var font = dialog.Font;
					result.Family = font.FontFamily.Name;
					result.Height = font.Size;
					return result;
				} else {
					return null;
				}
			}
		}
		
		bool CheckValidate()
		{
			var checkResult = true;
			this.errorProvider.Clear();
			
			if(!LauncherItemValid()) {
				this.errorProvider.SetError(this.selecterLauncher, Language["setting/check/item-name-dup"]);
				checkResult = false;
			}
			
			return checkResult;
		}
		
		void CreateSettingData()
		{
			var mainSetting = new MainSetting();
			
			// 本体
			MainExportSetting(mainSetting);
			
			// ランチャ
			LauncherExportSetting(mainSetting.Launcher);
			
			// コマンド
			CommandExportSetting(mainSetting.Command);
			
			// ツールバー
			ToolbarExportSetting(mainSetting.Toolbar);
			
			// ノート
			// ディスプレイ
			
			// プロパティ設定
			MainSetting = mainSetting;
		}
	}
}
