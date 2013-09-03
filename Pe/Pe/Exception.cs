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
	/// <summary>
	/// 
	/// </summary>
	public class PeException: ShareLibException
	{
		/// <summary>
		/// 
		/// </summary>
		public PeException() 
		{
			Initialize();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public PeException(string message) : base(message) 
		{
			Initialize();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public PeException(string message, Exception inner) : base(message) 
		{
			Initialize();
		}
		
		private void Initialize()
		{
		}
	}
}
