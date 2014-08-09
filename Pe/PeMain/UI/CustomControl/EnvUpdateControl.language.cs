/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 21:44
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class EnvUpdateControl
	{
		void ApplyLanguage(Language language)
		{
			this.gridEnv_columnKey.SetLanguage(language);
			this.gridEnv_columnValue.SetLanguage(language);
		}
	}
}
