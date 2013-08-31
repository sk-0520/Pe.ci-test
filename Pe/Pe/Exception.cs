/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/31
 * 時刻: 21:02
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ShareLib;

namespace Pe
{
	public class PeException: ShareLibException
	{
		public PeException() 
		{
			Initialize();
		}
		
		public PeException(string message) : base(message) 
		{
			Initialize();
		}
		
		public PeException(string message, Exception inner) : base(message) 
		{
			Initialize();
		}
		
		private void Initialize()
		{
		}
	}
}
