namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System.Diagnostics;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	public partial class MessageWindow: Form, ISetCommonData
	{
		#region property

		private readonly App _parent;

		#endregion

		public MessageWindow(App parent)
		{
			InitializeComponent();

			this._parent = parent;
			ClipboardRegisted = false;
		}

		#region ISetCommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();

			//NextWndHandle = NativeMethods.SetClipboardViewer(Handle);
			if(ClipboardRegisted) {
				NativeMethods.RemoveClipboardFormatListener(Handle);
			}
			ClipboardRegisted = NativeMethods.AddClipboardFormatListener(Handle);
		}

		#endregion

		#region property

		CommonData CommonData { get; set; }
		public ILogger StartupLogger { get; set; }
		//IntPtr NextWndHandle { get; set; }
		bool ClipboardRegisted { get; set; }

		#endregion ////////////////////////////////

		#region wndProc

		protected override void WndProc(ref Message m)
		{
			switch(m.Msg) {
				case (int)WM.WM_HOTKEY: {
						var id = (HotKeyId)m.WParam;
						var mod = (MOD)unchecked((short)(long)m.LParam);
						var key = (Keys)unchecked((ushort)((long)m.LParam >> 16));
						CommonData.RootSender.SendHotKey(id, mod, key);
					}
					break;

				case (int)WM.WM_DEVICECHANGE: {
						var changeDevice = new ChangeDevice(m);
						CommonData.RootSender.SendDeviceChanged(changeDevice);
					}
					break;
				/*
			case (int)WM.WM_CHANGECBCHAIN: 
				{
					if(m.WParam == NextWndHandle) {
						NextWndHandle = m.LParam;
					} else if(NextWndHandle != null) {
						NativeMethods.SendMessage(NextWndHandle, (WM)m.Msg, m.WParam, m.LParam);
					}
				}
				break;
						
			case (int)WM.WM_DRAWCLIPBOARD: 
				{
					//if(CommonData.RootSender.EnabledClipboard) {
					CommonData.RootSender.ChangeClipboard();
					//}
					if(NextWndHandle != null) {
						NativeMethods.SendMessage(NextWndHandle, (WM)m.Msg, m.WParam, m.LParam);
					}
				}
				break;
				*/
				case (int)WM.WM_CLIPBOARDUPDATE: {
						CommonData.RootSender.ChangedClipboard();
					}
					break;

				default:
					break;
			}
			base.WndProc(ref m);
		}


		#endregion ///////////////////////////////////

		#region language

		void ApplyLanguage()
		{
			// 無意味
		}

		#endregion

		#region function

		bool RegisterHotKey(HotKeyId hotKeyId, MOD modKey, Keys key)
		{
			return NativeMethods.RegisterHotKey(Handle, (int)hotKeyId, modKey, (uint)key);
		}

		bool UnRegisterHotKey(HotKeyId hotKeyId)
		{
			return NativeMethods.UnregisterHotKey(Handle, (int)hotKeyId);
		}

		void ApplyHotKey()
		{
			var hotKeyDatas = new[] {
					new { Id = HotKeyId.ShowCommand,   HotKey = CommonData.MainSetting.Command.HotKey,                 UnRegistMessageName = "hotkey/unregist/command",         RegistMessageName = "hotkey/regist/command" },
					new { Id = HotKeyId.HiddenFile,    HotKey = CommonData.MainSetting.SystemEnv.HiddenFileShowHotKey, UnRegistMessageName = "hotkey/unregist/hidden-file",     RegistMessageName = "hotkey/regist/hidden-file" },
					new { Id = HotKeyId.Extension,     HotKey = CommonData.MainSetting.SystemEnv.ExtensionShowHotKey,  UnRegistMessageName = "hotkey/unregist/extension",       RegistMessageName = "hotkey/regist/extension" },
					new { Id = HotKeyId.CreateNote,    HotKey = CommonData.MainSetting.Note.CreateHotKey,              UnRegistMessageName = "hotkey/unregist/create-note",     RegistMessageName = "hotkey/regist/create-note" },
					new { Id = HotKeyId.HiddenNote,    HotKey = CommonData.MainSetting.Note.HiddenHotKey,              UnRegistMessageName = "hotkey/unregist/hidden-note",     RegistMessageName = "hotkey/regist/hidden-note" },
					new { Id = HotKeyId.CompactNote,   HotKey = CommonData.MainSetting.Note.CompactHotKey,             UnRegistMessageName = "hotkey/unregist/compact-note",    RegistMessageName = "hotkey/regist/compact-note" },
					new { Id = HotKeyId.ShowFrontNote, HotKey = CommonData.MainSetting.Note.ShowFrontHotKey,           UnRegistMessageName = "hotkey/unregist/show-front-note", RegistMessageName = "hotkey/regist/show-front-note" },
					new { Id = HotKeyId.SwitchClipboardShow, HotKey = CommonData.MainSetting.Clipboard.ToggleHotKeySetting, UnRegistMessageName = "hotkey/unregist/show-front-note", RegistMessageName = "hotkey/regist/clipborad" },
				};
			// 登録解除
			foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.IsRegistered)) {
				if(UnRegisterHotKey(hotKeyData.Id)) {
					hotKeyData.HotKey.IsRegistered = false;
				} else {
					var logData = new LogData();
					logData.LogType = LogType.Warning;
					logData.Title = CommonData.Language["hotkey/unregist/fail"];
					logData.Detail = CommonData.Language[hotKeyData.UnRegistMessageName];
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
					hotKeyData.HotKey.IsRegistered = true;
				} else {
					var logData = new LogData();
					logData.LogType = LogType.Warning;
					logData.Title = CommonData.Language["hotkey/regist/fail"];
					logData.Detail = CommonData.Language[hotKeyData.RegistMessageName];
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
		#endregion /////////////////////////////////
	}
}
