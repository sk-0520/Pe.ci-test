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
using PeMain.Setting;
using PeUtility;

namespace PeMain.UI
{
	public partial class SettingForm
	{
		void AttachmentIconSize(ComboBox control, IconSize defaultData)
		{
			var iconSizeList = new List<IconSizeItem>();
			foreach(var value in new [] { IconSize.Small, IconSize.Normal, IconSize.Big, IconSize.Large }) {
				var data = new IconSizeItem(value, Language);
				iconSizeList.Add(data);
			}
			control.Attachment(iconSizeList, defaultData);
		}
		
		void OpenDialogFilePath(TextBox input)
		{
			var path = input.Text.Trim();
			using(var dialog = new OpenFileDialog()) {
				if(path.Length > 0 && File.Exists(path)) {
					dialog.InitialDirectory = Path.GetDirectoryName(path);
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.FileName;
				}
			}
		}
		
		void OpenDialogDirPath(TextBox input)
		{
			var path = input.Text.Trim();
			using(var dialog = new FolderBrowserDialog()) {
				dialog.ShowNewFolderButton = true;
				
				if(path.Length > 0 && Directory.Exists(path)) {
					dialog.SelectedPath = path;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.SelectedPath;
				}
			}
		}
		
		void SetViewMessage(Control viewControl, FontSetting fontSetting)
		{
			string viewText = Language["common/font-view"];
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
					result.Height = font.Height;
					return result;
				} else {
					return null;
				}
			}
		}
		
		bool CheckValidate() 
		{
			return true;
		}
	}
}
