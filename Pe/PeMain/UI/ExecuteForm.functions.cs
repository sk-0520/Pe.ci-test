/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	public partial class ExecuteForm
	{
		public void SetSettingData(Language language, MainSetting mainSetting, LauncherItem launcherItem)
		{
			Language = language;
			this._mainSetting = mainSetting;
			LauncherItem = launcherItem;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(LauncherItem != null);
			
			ApplyLanguage();
			
			Icon = LauncherItem.GetIcon(IconSize.Small);
			
			this.viewCommand.Text = LauncherItem.Command;
			this.inputOption.Items.AddRange(LauncherItem.LauncherHistory.Options.ToArray());
			this.inputOption.Text = LauncherItem.WorkDirPath;
			this.inputWorkDirPath.Items.AddRange(LauncherItem.LauncherHistory.WorkDirs.ToArray());
			this.inputWorkDirPath.Text = LauncherItem.WorkDirPath;
			this.selectStdStream.Checked = LauncherItem.StdOutputWatch;
			
			this.selectUserDefault.Checked = LauncherItem.EnvironmentSetting.UseDefault;
			this.envUpdate.SetItem(LauncherItem.EnvironmentSetting.Update.ToDictionary(pair => pair.Key, pair => pair.Value));
			this.envRemove.SetItem(LauncherItem.EnvironmentSetting.Remove);
		}
		
		void SubmitInput()
		{
			var item = (LauncherItem)LauncherItem.Clone();
			item.Option = this.inputOption.Text;
			item.WorkDirPath = this.inputWorkDirPath.Text;
			item.StdOutputWatch = this.selectStdStream.Checked;
			
			item.EnvironmentSetting.UseDefault = this.selectUserDefault.Checked;
			//item.EnvironmentSetting.Update = this.envUpdate
			//item.EnvironmentSetting.Remove = this.envRemove
			
			EditedLauncherItem = item;
		}
	}
}
