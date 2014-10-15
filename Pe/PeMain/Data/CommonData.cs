/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/21
 * 時刻: 20:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.IF;
using PeMain.Logic;
using PeMain.UI;
using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// 持ち運び用共通データ。
	/// </summary>
	public sealed class CommonData: IDisposable
	{
		/// <summary>
		/// 主設定。
		/// </summary>
		public MainSetting MainSetting { get; set; }
		/// <summary>
		/// 言語設定。
		/// </summary>
		public Language Language { get; set; }
		/// <summary>
		/// スキン。
		/// </summary>
		public ISkin Skin { get; set; }
		/// <summary>
		/// ロガー。
		/// </summary>
		public ILogger Logger { get; set; }
		/// <summary>
		/// メッセージ送信。
		/// </summary>
		public IRootSender RootSender { get; set; }
		/// <summary>
		/// データベースコネクション。
		/// 
		/// 入れるべきじゃない気がする。
		/// </summary>
		public AppDBManager Database { get; set; }
		
		public void Dispose()
		{
			Database.ToDispose();
		}
	}
}
