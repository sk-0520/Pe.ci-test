/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:57
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class AboutForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			ApplyLanguage();
		}
		
		void OpenDirectory(string path)
		{
			Executer.OpenDirectory(path, CommonData, null);
		}

	}
}
