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
using System.Linq;
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
		bool RegisterHotKey(HotKeyId hotKeyId, MOD modKey, Keys key)
		{
			return API.RegisterHotKey(Handle, (int)hotKeyId, modKey, (uint)key);
		}
		
		bool UnRegisterHotKey(HotKeyId hotKeyId)
		{
			return API.UnregisterHotKey(Handle, (int)hotKeyId);
		}
		
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplyHotKey()
		{
			var hotKeyDatas = new [] {
				new { Id = HotKeyId.ShowCommand, HotKey = CommonData.MainSetting.Command.HotKey,                 UnRegistMessage = string.Empty, RegistMessage = string.Empty },
				new { Id = HotKeyId.HiddenFile,  HotKey = CommonData.MainSetting.SystemEnv.HiddenFileShowHotKey, UnRegistMessage = string.Empty, RegistMessage = string.Empty },
				new { Id = HotKeyId.Extension,   HotKey = CommonData.MainSetting.SystemEnv.ExtensionShowHotKey,  UnRegistMessage = string.Empty, RegistMessage = string.Empty },
			};
			// 登録解除
			foreach(var hotKeyData in hotKeyDatas) {
				if(!UnRegisterHotKey(hotKeyData.Id)) {
					CommonData.Logger.Puts(LogType.Warning, CommonData.Language["hotkey/unregist/fail"], hotKeyData.UnRegistMessage);
				}
			}
			// 登録
			foreach(var hotKeyData in hotKeyDatas.Where(data => data.HotKey.Enabled)) {
				if(!RegisterHotKey(hotKeyData.Id, hotKeyData.HotKey.Modifiers, hotKeyData.HotKey.Key)) {
					CommonData.Logger.Puts(LogType.Error, CommonData.Language["hotkey/regist/fail"], hotKeyData.RegistMessage);
				}
			}
		}
		
		void ApplySetting()
		{
			Debug.Assert(CommonData != null);
			
			ApplyLanguage();
			
			ApplyHotKey();
		}

		
	}
}
