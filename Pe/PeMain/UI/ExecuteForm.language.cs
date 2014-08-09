/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

using PeMain.Logic;

namespace PeMain.UI
{
	public partial class ExecuteForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);
			
			var map = new Dictionary<string, string>() {
				{ SystemLanguageName.itemName, LauncherItem.Name },
			};
			
			DialogUtility.SetDefaultText(this, CommonData.Language, CommonData.Language["window/execute", map]);
			
			this.envUpdate.SetLanguage(CommonData.Language);
			this.envRemove.SetLanguage(CommonData.Language);
			
			/*
			this.pageBasic.Text = CommonData.Language["execute/tab/basic"];
			this.pageEnv.Text = CommonData.Language["common/tab/env"];
			this.labelOption.Text = CommonData.Language["execute/label/option"];
			this.labelWorkDirPath.Text = CommonData.Language["execute/label/work-dir"];
			this.selectStdStream.Text  = CommonData.Language["execute/check/std-stream"];
			this.selectEnvironment.Text  = CommonData.Language["execute/check/edit-env"];
			this.groupUpdate.Text = CommonData.Language["common/label/edit"];
			this.groupRemove.Text = CommonData.Language["common/label/remove"];
			this.selectAdministrator.Text = CommonData.Language["common/check/admin"];
			*/
			this.pageBasic.SetLanguage(CommonData.Language);
			this.pageEnv.SetLanguage(CommonData.Language);
			this.labelOption.SetLanguage(CommonData.Language);
			this.labelWorkDirPath.SetLanguage(CommonData.Language);
			this.selectStdStream.SetLanguage(CommonData.Language);
			this.selectEnvironment.SetLanguage(CommonData.Language);
			this.groupUpdate.SetLanguage(CommonData.Language);
			this.groupRemove.SetLanguage(CommonData.Language);
			this.selectAdministrator.SetLanguage(CommonData.Language);
			
		}
	}
}
