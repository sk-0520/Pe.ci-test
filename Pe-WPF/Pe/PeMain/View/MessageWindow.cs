namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// 将来的に別ウィンドウを本体として機能移植する。
	/// </summary>
	public class MessageWindow : CommonDataWindow
	{
		public MessageWindow ()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
			WindowStyle = System.Windows.WindowStyle.None;
			Width = 0;
			Height = 0;
			ResizeMode = System.Windows.ResizeMode.NoResize;
			ShowInTaskbar = false;

			ClipboardListenerRegisted = false;
		}

		#region property

		public bool ClipboardListenerRegisted { get; private set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.OnLoaded(sender, e);
			Visibility = System.Windows.Visibility.Collapsed;

			ApplyHotKey();
			RegistClipboardListener();
		}
		
		protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch(msg) {
				case (int)WM.WM_DESTROY:
					{
						UnregistClipboardListener();
					}
					break;

				case (int)WM.WM_DEVICECHANGE:
					{
						var changedDevice = new ChangedDevice(hWnd, msg, wParam, lParam);
						CommonData.AppSender.SendDeviceChanged(changedDevice);
					}
					break;

				case (int)WM.WM_CLIPBOARDUPDATE:
					{
						CommonData.AppSender.SendClipboardChanged();
					}
					break;

				case (int)WM.WM_HOTKEY: 
					{
						var hotKeyId = (HotKeyId)wParam;
						var hotKeyModel = new HotKeyModel() {
							Key = WindowsUtility.ConvertKeyFromLParam(lParam),
							ModifierKeys = WindowsUtility.ConvertModifierKeysFromLParam(lParam),
						};

						CommonData.AppSender.SendHotKey(hotKeyId, hotKeyModel);
					}
					break;
			}

			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region function
		
		public void RegistClipboardListener()
		{
			if(!ClipboardListenerRegisted) {
				ClipboardListenerRegisted = NativeMethods.AddClipboardFormatListener(Handle);
			}
		}

		public void UnregistClipboardListener()
		{
			if(ClipboardListenerRegisted) {
				NativeMethods.RemoveClipboardFormatListener(Handle);
				ClipboardListenerRegisted = false;
			}
		}

		bool RegistHotKey(HotKeyId hotKeyId, HotKeyModel hotkeyModel)
		{
			var mod = WindowsUtility.ConvertMODFromModifierKeys(hotkeyModel.ModifierKeys);
			var key = KeyInterop.VirtualKeyFromKey(hotkeyModel.Key);

			return NativeMethods.RegisterHotKey(Handle, (int)hotKeyId, mod, key);
		}

		bool UnRegisterHotKey(HotKeyId hotKeyId)
		{
			return NativeMethods.UnregisterHotKey(Handle, (int)hotKeyId);
		}

		void ApplyHotKey()
		{
			var hotKeyDatas = new[] {
					new { Id = HotKeyId.ShowCommand,   HotKey = CommonData.MainSetting.Command.ShowHotkey,                 UnRegistMessageName = "hotkey/unregist/command",         RegistMessageName = "hotkey/regist/command" },
					new { Id = HotKeyId.HideFile,      HotKey = CommonData.MainSetting.SystemEnvironment.HideFileHotkey, UnRegistMessageName = "hotkey/unregist/hidden-file",     RegistMessageName = "hotkey/regist/hidden-file" },
					new { Id = HotKeyId.Extension,     HotKey = CommonData.MainSetting.SystemEnvironment.ExtensionHotkey,  UnRegistMessageName = "hotkey/unregist/extension",       RegistMessageName = "hotkey/regist/extension" },
					new { Id = HotKeyId.CreateNote,    HotKey = CommonData.MainSetting.Note.CreateHotKey,              UnRegistMessageName = "hotkey/unregist/create-note",     RegistMessageName = "hotkey/regist/create-note" },
					new { Id = HotKeyId.HiddenNote,    HotKey = CommonData.MainSetting.Note.HideHotKey,              UnRegistMessageName = "hotkey/unregist/hidden-note",     RegistMessageName = "hotkey/regist/hidden-note" },
					new { Id = HotKeyId.CompactNote,   HotKey = CommonData.MainSetting.Note.CompactHotKey,             UnRegistMessageName = "hotkey/unregist/compact-note",    RegistMessageName = "hotkey/regist/compact-note" },
					new { Id = HotKeyId.ShowFrontNote, HotKey = CommonData.MainSetting.Note.ShowFrontHotKey,           UnRegistMessageName = "hotkey/unregist/show-front-note", RegistMessageName = "hotkey/regist/show-front-note" },
					new { Id = HotKeyId.SwitchClipboardShow, HotKey = CommonData.MainSetting.Clipboard.ToggleHotKey, UnRegistMessageName = "hotkey/unregist/show-front-note", RegistMessageName = "hotkey/regist/clipborad" },
				};
			// 登録解除
			foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.IsRegistered)) {
				if(UnRegisterHotKey(hotKeyData.Id)) {
					hotKeyData.HotKey.IsRegistered = false;
				} else {
					var message = CommonData.Language["hotkey/unregist/fail"];
					var detail = CommonData.Language[hotKeyData.UnRegistMessageName];

					CommonData.Logger.Warning(message, detail);
				}
			}

			// 登録
			foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.Enabled)) {
				if(RegistHotKey(hotKeyData.Id, hotKeyData.HotKey)) {
					hotKeyData.HotKey.IsRegistered = true;
				} else {
					var message = CommonData.Language["hotkey/regist/fail"];
					var detail = CommonData.Language[hotKeyData.RegistMessageName];

					CommonData.Logger.Warning(message, detail);
				}
			}
		}

		#endregion
	}
}
