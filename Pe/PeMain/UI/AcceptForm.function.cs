﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/21/2014
 * 時刻: 10:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AcceptForm_functions.
	/// </summary>
	partial class AcceptForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			ApplyLanguage();
			
			this.selectUpdateCheck.Checked = CommonData.MainSetting.RunningInfo.CheckUpdate;
			this.selectUpdateCheckRC.Checked = CommonData.MainSetting.RunningInfo.CheckUpdateRC;
		}
		
	}
}
