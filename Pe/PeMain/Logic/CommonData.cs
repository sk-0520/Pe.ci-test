/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/29
 * 時刻: 22:00
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Data.Common;
using PeMain.Data;
using PeMain.UI;
using PeUtility;

namespace PeMain.Logic
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
	/// <summary>
	/// Description of ISetCommonData.
	/// </summary>
	public interface ISetCommonData
	{
		void SetCommonData(CommonData commonData);
	}
}
