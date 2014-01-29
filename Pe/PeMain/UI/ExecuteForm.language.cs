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

namespace PeMain.UI
{
	public partial class ExecuteForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			var map = new Dictionary<string, string>() {
				{ "ITEM", LauncherItem.Name },
			};
			
			Text = Language["window/execute", map];
			this.commandSubmit.Text= Language["common/button/ok"];
			this.commandCancel.Text = Language["common/button/cancel"];
			
			this.envUpdate.SetLanguage(Language);
			this.envRemove.SetLanguage(Language);
			
			this.pageBasic.Text = Language["execute/tab/basic"];
			this.pageEnv.Text = Language["execute/tab/env"];
			this.labelOption.Text = Language["execute/label/option"];
			this.labelWorkDirPath.Text = Language["execute/label/work-dir"];
			this.selectStdStream.Text  = Language["execute/check/std-stream"];
			this.selectEnvironment.Text  = Language["execute/check/edit-env"];
		}
	}
}
