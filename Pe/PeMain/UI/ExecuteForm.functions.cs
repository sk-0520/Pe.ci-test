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
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	public partial class ExecuteForm
	{
		public void SetSettingData(Language language, LauncherItem launcherItem)
		{
			Language = language;
			LauncherItem = launcherItem;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(LauncherItem != null);
			
			ApplyLanguage();
		}
	}
}
