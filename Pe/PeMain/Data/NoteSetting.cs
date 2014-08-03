/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 12:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeUtility;

namespace PeMain.Data
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class NoteSetting: Item, IDisposable
	{
		private DBManager _db;
		
		public NoteSetting()
		{
			CreateHotKey = new HotKeySetting();
			HiddenHotKey = new HotKeySetting();
			CompactHotKey = new HotKeySetting();
			
			CaptionFontSetting = new FontSetting();
		}
		
		/// <summary>
		/// 新規作成時のホットキー
		/// </summary>
		public HotKeySetting CreateHotKey { get; set; }
		public HotKeySetting HiddenHotKey { get; set; }
		public HotKeySetting CompactHotKey { get; set; }
		
		public FontSetting CaptionFontSetting { get; set; }
		
		public void setDatabase(DBManager db)
		{
			this._db  = db;
		}
		
		public void Dispose()
		{
			CaptionFontSetting.Dispose();
		}
		
	}
}
