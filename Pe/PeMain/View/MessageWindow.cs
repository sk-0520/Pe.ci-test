/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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

namespace ContentTypeTextNet.Pe.PeMain.View
{
    /// <summary>
    /// 将来的に別ウィンドウを本体として機能移植する。
    /// </summary>
    public class MessageWindow: CommonDataWindow
    {
        public MessageWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            WindowStyle = System.Windows.WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            ShowInTaskbar = false;
            Width = 0;
            Height = 0;

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

            ApplySetting_Impl();
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

                        CommonData.AppSender.SendInputHotKey(hotKeyId, hotKeyModel);
                    }
                    break;
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        protected override void ApplySetting()
        {
            base.ApplySetting();
            if(IsHandleCreated) {
                ApplySetting_Impl();
            }
        }

        #endregion

        #region function

        void ApplySetting_Impl()
        {
            ApplyHotKey();
            RegistClipboardListener();
        }

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
                    new { Id = HotKeyId.ShowCommand,         HotKey = CommonData.MainSetting.Command.ShowHotkey,                 UnRegistMessageName = "log/hotkey/unregist/command",         RegistMessageName = "log/hotkey/regist/command" },
                    new { Id = HotKeyId.HiddenFile,            HotKey = CommonData.MainSetting.SystemEnvironment.HideFileHotkey,   UnRegistMessageName = "log/hotkey/unregist/hidden-file",     RegistMessageName = "log/hotkey/regist/hidden-file" },
                    new { Id = HotKeyId.Extension,           HotKey = CommonData.MainSetting.SystemEnvironment.ExtensionHotkey,  UnRegistMessageName = "log/hotkey/unregist/extension",       RegistMessageName = "log/hotkey/regist/extension" },
                    new { Id = HotKeyId.CreateNote,          HotKey = CommonData.MainSetting.Note.CreateHotKey,                  UnRegistMessageName = "log/hotkey/unregist/create-note",     RegistMessageName = "log/hotkey/regist/create-note" },
                    new { Id = HotKeyId.HideNote,            HotKey = CommonData.MainSetting.Note.HideHotKey,                    UnRegistMessageName = "log/hotkey/unregist/hide-note",       RegistMessageName = "log/hotkey/regist/hide-note" },
                    new { Id = HotKeyId.CompactNote,         HotKey = CommonData.MainSetting.Note.CompactHotKey,                 UnRegistMessageName = "log/hotkey/unregist/compact-note",    RegistMessageName = "log/hotkey/regist/compact-note" },
                    new { Id = HotKeyId.ShowFrontNote,       HotKey = CommonData.MainSetting.Note.ShowFrontHotKey,               UnRegistMessageName = "log/hotkey/unregist/show-front-note", RegistMessageName = "log/hotkey/regist/show-front-note" },
                    new { Id = HotKeyId.SwitchClipboardShow, HotKey = CommonData.MainSetting.Clipboard.ToggleHotKey,             UnRegistMessageName = "log/hotkey/unregist/clipboard",       RegistMessageName = "log/hotkey/regist/clipboard" },
                    new { Id = HotKeyId.SwitchTemplateShow,  HotKey = CommonData.MainSetting.Template.ToggleHotKey,              UnRegistMessageName = "log/hotkey/unregist/template",        RegistMessageName = "log/hotkey/regist/template" },
                };
            // 登録解除
            foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.IsRegistered)) {
                if(UnRegisterHotKey(hotKeyData.Id)) {
                    hotKeyData.HotKey.IsRegistered = false;
                } else {
                    var message = CommonData.Language[hotKeyData.UnRegistMessageName];

                    CommonData.Logger.Warning(message, hotKeyData.HotKey);
                }
            }

            // 登録
            foreach(var hotKeyData in hotKeyDatas.Where(hk => hk.HotKey.IsEnabled)) {
                if(RegistHotKey(hotKeyData.Id, hotKeyData.HotKey)) {
                    hotKeyData.HotKey.IsRegistered = true;
                } else {
                    var message = CommonData.Language[hotKeyData.RegistMessageName];

                    CommonData.Logger.Warning(message, hotKeyData.HotKey);
                }
            }
        }

        #endregion
    }
}
