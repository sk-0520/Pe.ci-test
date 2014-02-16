/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 20:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class MessageWindow
	{
		void RegisterHotKey(HotKeyId hotKeyId, MOD modKey, Keys key)
		{
			API.RegisterHotKey(Handle, (int)hotKeyId, modKey, (uint)key);
		}
		
		void UnRegisterHotKey(HotKeyId hotKeyId)
		{
			API.UnregisterHotKey(Handle, (int)hotKeyId);
		}
		
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplyHotkey()
		{
			//API.RegisterHotKey(
		}
		
		void ApplySetting()
		{
			Debug.Assert(CommonData != null);
			
			ApplyLanguage();
			
			ApplyHotkey();
		}
		
	}
}
