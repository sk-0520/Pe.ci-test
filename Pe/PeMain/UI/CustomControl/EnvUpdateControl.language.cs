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

namespace PeMain.UI
{
	public partial class EnvUpdateControl
	{
		void ApplyLanguage(Language language)
		{
			this.headerKey.HeaderText = language["env-updater/key"];
			this.headerValue.HeaderText = language["env-updater/value"];
		}
	}
}
