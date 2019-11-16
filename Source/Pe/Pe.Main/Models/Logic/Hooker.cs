using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class HookerBase : DisposerBase
    {
        public HookerBase(ILoggerFactory loggerFactory)
        {
            HookProc = HookProcedure;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IntPtr HookHandle { get; private set; }
        HookProc HookProc { get; }
        protected ILogger Logger { get; }

        public bool IsEnabled => HookHandle != IntPtr.Zero;

        #endregion

        #region function

        protected abstract IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle);

        public void Register()
        {
            var moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
            var moduleHandle = NativeMethods.GetModuleHandle(moduleName);
            HookHandle = RegisterImpl(HookProc, moduleHandle);
        }

        public void Unregister()
        {
            NativeMethods.UnhookWindowsHookEx(HookHandle);
            HookHandle = IntPtr.Zero;
        }

        protected abstract IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam);

        protected IntPtr CallNextProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            Logger.LogTrace("code = {0}, wParam = {1}, lParam = {2}", code, wParam, lParam);
            return NativeMethods.CallNextHookEx(HookHandle, code, wParam, lParam);
        }


        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(IsEnabled) {
                    Unregister();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 修飾キーが押されているか。
    /// </summary>
    public readonly struct ModifierKeyState
    {
        public ModifierKeyState(bool left, bool right)
        {
            Left = left;
            Right = right;
        }

        #region property

        /// <summary>
        /// 左。
        /// </summary>
        public bool Left { get; }
        /// <summary>
        /// 右。
        /// </summary>
        public bool Right { get; }

        #endregion

        #region ValueType

        public override string ToString()
        {
            if(Left || Right) {
                if(Left && Right) {
                    return "All";
                }
                return Left ? nameof(Left) : nameof(Right);
            }
            return "None";
        }

        #endregion
    }

    public readonly struct ModifierKeyStatus
    {
        #region variable

        public readonly ModifierKeyState shift;
        public readonly ModifierKeyState contrl;
        public readonly ModifierKeyState alt;
        public readonly ModifierKeyState super;

        #endregion

        public ModifierKeyStatus(ModifierKeyState shift, ModifierKeyState contrl, ModifierKeyState alt, ModifierKeyState super)
        {
            this.shift = shift;
            this.contrl = contrl;
            this.alt = alt;
            this.super = super;
        }


        #region function

        public static ModifierKeyStatus Create()
        {
            static ModifierKeyState GetModifierKeyState(Key left, Key right)
            {
                return new ModifierKeyState(Keyboard.IsKeyDown(left), Keyboard.IsKeyDown(right));
            }

            return new ModifierKeyStatus(
                GetModifierKeyState(Key.LeftShift, Key.RightShift),
                GetModifierKeyState(Key.LeftCtrl, Key.RightCtrl),
                GetModifierKeyState(Key.LeftAlt, Key.RightAlt),
                GetModifierKeyState(Key.LWin, Key.RWin)
            );
        }

        #endregion

        #region ValueType

        public readonly override string ToString()
        {
            return $"shift = {this.shift}, alt = {this.alt}, ctrl = {this.contrl}, win = {this.super}";
        }

        #endregion

    }

    public class KeyboardHookEventArgs : EventArgs
    {
        #region proeprty

        public readonly KBDLLHOOKSTRUCT kbdll;
        public readonly ModifierKeyStatus modifierKeyStatus;

        #endregion

        public KeyboardHookEventArgs(bool isDown, IntPtr lParam, ModifierKeyStatus modifierKeyStatus)
        {
            IsDown = isDown;
            this.kbdll = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT))!;
            Key = KeyInterop.KeyFromVirtualKey((int)this.kbdll.vkCode);
            this.modifierKeyStatus = modifierKeyStatus;
        }

        #region property

        public bool IsDown {get;}
        public Key Key { get; }

        /// <summary>
        /// 処理したか。
        /// </summary>
        public bool Handled { get; set; }

        #endregion

    }

    public class KeyboradHooker : HookerBase
    {
        #region event

        public event EventHandler<KeyboardHookEventArgs>? KeyDown;
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        #endregion

        public KeyboradHooker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property
        #endregion

        #region function

        #endregion

        #region HookerBase

        protected override IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle)
        {
            return NativeMethods.SetWindowsHookEx(WH.WH_KEYBOARD_LL, hookProc, moduleHandle, 0);
        }

        protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            if(0<=code) {
                var message = wParam.ToInt32();
                var isDown = message == (int)WM.WM_KEYDOWN || message == (int)WM.WM_SYSKEYDOWN;
                var isUp = message == (int)WM.WM_KEYUP || message == (int)WM.WM_SYSKEYUP;
                if(isDown || isUp) {
                    var keyEvent = isDown ? KeyDown: KeyUp;
                    if(keyEvent != null) {
                        var modifierKeyStatus = ModifierKeyStatus.Create();
                        var e = new KeyboardHookEventArgs(isDown, lParam, modifierKeyStatus);
                        keyEvent.Invoke(this, e);
                        if(e.Handled) {
                            Logger.LogTrace("input disable: {0}, {1}", e.Key, e.modifierKeyStatus);
                            return new IntPtr(1);
                        }
                    }
                }
            }
            return CallNextProcedure(code, wParam, lParam);
        }

        #endregion
    }

    public class MouseHooker : HookerBase
    {
        public MouseHooker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region HookerBase

        protected override IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle)
        {
            return NativeMethods.SetWindowsHookEx(WH.WH_MOUSE_LL, hookProc, moduleHandle, 0);
        }

        protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            return CallNextProcedure(code, wParam, lParam);
        }

        #endregion
    }
}
