/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/17
 * 時刻: 16:37
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PInvoke.Windows;

namespace PeMain.Data
{
	/// <summary>
	/// デバイス情報変更
	/// </summary>
	public class ChangeDevice
	{
		private Message _message;
		
		public ChangeDevice(Message message)
		{
			this._message = message;
			
			DBT = (DBT)this._message.WParam.ToInt32();
		}
		
		public DBT DBT { get; private set; }
		
		
	}
}
