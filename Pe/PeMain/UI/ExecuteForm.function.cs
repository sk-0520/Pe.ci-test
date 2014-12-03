/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ContentTypeTextNet.Pe.Application.Data;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Application.UI
{
	partial class ExecuteForm
	{
		public void SetParameter(LauncherItem launcherItem, IEnumerable<string> exOptions)
		{
			LauncherItem = launcherItem;
			ExOptions = exOptions;
		}
		
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(LauncherItem != null);
			
			ApplyLanguage();
			
			Icon = LauncherItem.GetIcon(IconScale.Small, LauncherItem.IconItem.Index);
			
			this.viewCommand.Text = LauncherItem.Command;
			this.inputOption.Items.AddRange(LauncherItem.LauncherHistory.Options.ToArray());
			this.inputOption.Text = LauncherItem.Option;
			this.inputWorkDirPath.Items.AddRange(LauncherItem.LauncherHistory.WorkDirs.ToArray());
			this.inputWorkDirPath.Text = LauncherItem.WorkDirPath;
			this.selectStdStream.Checked = LauncherItem.StdOutputWatch;
			this.selectAdministrator.Checked = LauncherItem.Administrator;
			this.selectEnvironment.Checked = !this.selectEnvironment.Checked;
			this.selectEnvironment.Checked = LauncherItem.EnvironmentSetting.EditEnvironment;
			this.envUpdate.SetItem(LauncherItem.EnvironmentSetting.Update.ToDictionary(pair => pair.First, pair => pair.Second));
			this.envRemove.SetItem(LauncherItem.EnvironmentSetting.Remove);
			
			if(ExOptions != null && ExOptions.Any()) {
				var args = string.Join(" ", ExOptions.WhitespaceToQuotation());
				this.inputOption.Text = args;
			}
		}
		
		void SubmitInput()
		{
			var item = (LauncherItem)LauncherItem.Clone();
			item.Option = this.inputOption.Text;
			item.WorkDirPath = this.inputWorkDirPath.Text;
			item.StdOutputWatch = this.selectStdStream.Checked;
			item.Administrator = this.selectAdministrator.Checked;
			
			item.EnvironmentSetting.EditEnvironment = this.selectEnvironment.Checked;
			if(item.EnvironmentSetting.EditEnvironment) {
				item.EnvironmentSetting.Update = this.envUpdate.Items.ToList();
				item.EnvironmentSetting.Remove = this.envRemove.Items.ToList();
			}
			EditedLauncherItem = item;
		}
	}
}
