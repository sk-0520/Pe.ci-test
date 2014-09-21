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
using PInvoke.Windows;

namespace PeMain.UI
{
	public partial class Pe
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
					new { Id = HotKeyId.ShowCommand, HotKey = CommonData.MainSetting.Command.HotKey,                 UnRegistMessageName = "hotkey/unregist/command",      RegistMessageName = "hotkey/regist/command" },
					new { Id = HotKeyId.HiddenFile,  HotKey = CommonData.MainSetting.SystemEnv.HiddenFileShowHotKey, UnRegistMessageName = "hotkey/unregist/hidden-file",  RegistMessageName = "hotkey/regist/hidden-file" },
					new { Id = HotKeyId.Extension,   HotKey = CommonData.MainSetting.SystemEnv.ExtensionShowHotKey,  UnRegistMessageName = "hotkey/unregist/extension",    RegistMessageName = "hotkey/regist/extension" },
					new { Id = HotKeyId.CreateNote,  HotKey = CommonData.MainSetting.Note.CreateHotKey,              UnRegistMessageName = "hotkey/unregist/create-note",  RegistMessageName = "hotkey/regist/create-note" },
					new { Id = HotKeyId.HiddenNote,  HotKey = CommonData.MainSetting.Note.HiddenHotKey,              UnRegistMessageName = "hotkey/unregist/hidden-note",  RegistMessageName = "hotkey/regist/hidden-note" },
					new { Id = HotKeyId.CompactNote, HotKey = CommonData.MainSetting.Note.CompactHotKey,             UnRegistMessageName = "hotkey/unregist/compact-note", RegistMessageName = "hotkey/regist/compact-note" },
				};
				// 登録解除
				foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.Registered)) {
					if(UnRegisterHotKey(hotKeyData.Id)) {
						hotKeyData.HotKey.Registered = false;
					} else {
						var logData = new LogData();
						logData.LogType = LogType.Warning;
						logData.Title   = CommonData.Language["hotkey/unregist/fail"];
						logData.Detail  = CommonData.Language[hotKeyData.UnRegistMessageName];
						if(StartupLogger == null) {
							CommonData.Logger.Puts(logData.LogType, logData.Title, logData.Detail);
						} else {
							StartupLogger.Puts(logData.LogType, logData.Title, logData.Detail);
						}
					}
				}
				
				// 登録
				foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.Enabled)) {
					if(RegisterHotKey(hotKeyData.Id, hotKeyData.HotKey.Modifiers, hotKeyData.HotKey.Key)) {
						hotKeyData.HotKey.Registered = true;
					} else {
						var logData = new LogData();
						logData.LogType = LogType.Warning;
						logData.Title   = CommonData.Language["hotkey/regist/fail"];
						logData.Detail  = CommonData.Language[hotKeyData.RegistMessageName];
						if(StartupLogger == null) {
							CommonData.Logger.Puts(logData.LogType, logData.Title, logData.Detail);
						} else {
							StartupLogger.Puts(logData.LogType, logData.Title, logData.Detail);
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
}
