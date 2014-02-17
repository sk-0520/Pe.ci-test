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
				new { Id = HotKeyId.ShowCommand, HotKey = CommonData.MainSetting.Command.HotKey,                 UnRegistMessageName = "hotkey/unregist/command",     RegistMessageName = "hotkey/regist/command" },
				new { Id = HotKeyId.HiddenFile,  HotKey = CommonData.MainSetting.SystemEnv.HiddenFileShowHotKey, UnRegistMessageName = "hotkey/unregist/hidden-file", RegistMessageName = "hotkey/regist/hidden-file" },
				new { Id = HotKeyId.Extension,   HotKey = CommonData.MainSetting.SystemEnv.ExtensionShowHotKey,  UnRegistMessageName = "hotkey/unregist/extension",   RegistMessageName = "hotkey/regist/extension" },
			};
			// 登録解除
			foreach(var hotKeyData in hotKeyDatas) {
				if(!UnRegisterHotKey(hotKeyData.Id)) {
					var logData = new LogData();
					logData.LogType = LogType.Warning;
					logData.Title   = CommonData.Language["hotkey/unregist/fail"];
					logData.Detail  = CommonData.Language[hotKeyData.UnRegistMessageName];
					if(InitLog == null) {
						CommonData.Logger.Puts(logData.LogType, logData.Title, logData.Detail);
					} else {
						InitLog.Add(new LogItem(logData.LogType, logData.Title, logData.Detail));
					}
				}
			}
			// 登録
			foreach(var hotKeyData in hotKeyDatas.Where(data => data.HotKey.Enabled)) {
				if(!RegisterHotKey(hotKeyData.Id, hotKeyData.HotKey.Modifiers, hotKeyData.HotKey.Key)) {
					var logData = new LogData();
					logData.LogType = LogType.Warning;
					logData.Title   = CommonData.Language["hotkey/regist/fail"];
					logData.Detail  = CommonData.Language[hotKeyData.RegistMessageName];
					if(InitLog == null) {
						CommonData.Logger.Puts(logData.LogType, logData.Title, logData.Detail);
					} else {
						InitLog.Add(new LogItem(logData.LogType, logData.Title, logData.Detail));
					}
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
