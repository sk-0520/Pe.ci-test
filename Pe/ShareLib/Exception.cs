/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/30
 * 時刻: 23:12
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace ShareLib
{
	public class ShareLibException: Exception
	{
		public ShareLibException() 
		{
			Initialize();
		}
		
		public ShareLibException(string message) : base(message) 
		{
			Initialize();
		}
		
		public ShareLibException(string message, Exception inner) : base(message) 
		{
			Initialize();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime DateTime { get; private set; }
		
		/// <summary>
		/// 
		/// </summary>
		private void Initialize()
		{
			DateTime = DateTime.Now;
		}
	}
}
