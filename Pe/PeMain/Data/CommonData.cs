/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/21
 * 時刻: 20:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Logic;
using PeMain.UI;

namespace PeMain.Data
{
	public sealed class CommonData
	{
		public MainSetting MainSetting { get; set; }
		public Language Language { get; set; }
		public ISkin Skin { get; set; }
		public ILogger Logger { get; set; }
		public IRootSender RootSender { get; set; }
		/// <summary>
		/// 入れるべきじゃない気がする
		/// </summary>
		public PeDBManager Database { get; set; }
	}
}
