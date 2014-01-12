/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			ApplyLanguage();
		}
	}
}
