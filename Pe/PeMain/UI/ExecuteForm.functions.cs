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
		public void SetParameter(LauncherItem launcherItem)
		{
			LauncherItem = launcherItem;
		}
		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			this.MainSetting = mainSetting;
			
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
			
			this.selectEnvironment.Checked = !this.selectEnvironment.Checked;
			this.selectEnvironment.Checked = LauncherItem.EnvironmentSetting.EditEnvironment;
			this.envUpdate.SetItem(LauncherItem.EnvironmentSetting.Update.ToDictionary(pair => pair.Key, pair => pair.Value));
			this.envRemove.SetItem(LauncherItem.EnvironmentSetting.Remove);
		}
		
		void SubmitInput()
		{
			var item = (LauncherItem)LauncherItem.Clone();
			item.Option = this.inputOption.Text;
			item.WorkDirPath = this.inputWorkDirPath.Text;
			item.StdOutputWatch = this.selectStdStream.Checked;
			
			item.EnvironmentSetting.EditEnvironment = this.selectEnvironment.Checked;
			if(item.EnvironmentSetting.EditEnvironment) {
				item.EnvironmentSetting.Update = this.envUpdate.Items.ToList();
				item.EnvironmentSetting.Remove = this.envRemove.Items.ToList();
			}
			EditedLauncherItem = item;
		}
	}
}
