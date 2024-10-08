using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class HookBase: DisposerBase
    {
        protected HookBase(ILoggerFactory loggerFactory)
        {
            HookProc = HookProcedure;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IntPtr HookHandle { get; private set; }
        private HookProc HookProc { get; }
        protected ILogger Logger { get; }

        public bool IsEnabled => HookHandle != IntPtr.Zero;

        #endregion

        #region function

        protected abstract IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle);

        public void Register()
        {
            var moduleName = Process.GetCurrentProcess().MainModule!.ModuleName!;
            var moduleHandle = NativeMethods.GetModuleHandle(moduleName);
            HookHandle = RegisterImpl(HookProc, moduleHandle);
        }

        public void Unregister()
        {
            NativeMethods.UnhookWindowsHookEx(HookHandle);
            HookHandle = IntPtr.Zero;
        }

        protected bool IsSkipCode(int code)
        {
            if(code < 0) {
                return true;
            }

            if(code == (int)HC.HC_NOREMOVE) {
                return true;
            }

            return false;
        }

        protected abstract IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam);

        protected IntPtr CallNextProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
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
        public readonly ModifierKeyState control;
        public readonly ModifierKeyState alt;
        public readonly ModifierKeyState super;

        #endregion

        public ModifierKeyStatus(ModifierKeyState shift, ModifierKeyState control, ModifierKeyState alt, ModifierKeyState super)
        {
            this.shift = shift;
            this.control = control;
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
            return $"shift = {this.shift}, alt = {this.alt}, ctrl = {this.control}, win = {this.super}";
        }

        #endregion
    }

    public class KeyboardHookEventArgs: EventArgs
    {
        #region variable

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

        public bool IsDown { get; }
        public Key Key { get; }

        /// <summary>
        /// 処理したか。
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 生成時間。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        #endregion
    }

    public class KeyboardHook: HookBase
    {
        #region event

        public event EventHandler<KeyboardHookEventArgs>? KeyDown;
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        #endregion

        public KeyboardHook(ILoggerFactory loggerFactory)
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
            if(!IsSkipCode(code)) {
                var message = wParam.ToInt32();
                var isDown = message == (int)WM.WM_KEYDOWN || message == (int)WM.WM_SYSKEYDOWN;
                var isUp = message == (int)WM.WM_KEYUP || message == (int)WM.WM_SYSKEYUP;
                if(isDown || isUp) {
                    var keyEvent = isDown ? KeyDown : KeyUp;
                    Stopwatch? stopwatch = null;
                    if(keyEvent != null) {
                        var logging = Logger.IsEnabled(LogLevel.Trace) || Logger.IsEnabled(LogLevel.Warning);
                        if(logging) {
                            stopwatch = new Stopwatch();
                            stopwatch.Start();
                        }
                        try {

                            var modifierKeyStatus = ModifierKeyStatus.Create();
                            var e = new KeyboardHookEventArgs(isDown, lParam, modifierKeyStatus);
                            keyEvent.Invoke(this, e);
                            if(e.Handled) {
                                Logger.LogInformation("キーボード入力制御: {0}, {1}, {2}", isUp ? nameof(KeyDown) : nameof(KeyUp), e.Key, e.modifierKeyStatus);
                                return new IntPtr(1);
                            }
                        } finally {
                            if(logging) {
                                Debug.Assert(stopwatch != null);
                                stopwatch.Stop();
                                if(TimeSpan.FromMilliseconds(300) < stopwatch.Elapsed) {
                                    Logger.LogWarning("キーボード {0} フック 実装部 所要時間長期: {1}", isUp ? nameof(KeyDown) : nameof(KeyUp), stopwatch.Elapsed);
                                }
                            }
                        }
                    }
                }
            }

            return CallNextProcedure(code, wParam, lParam);
        }

        #endregion
    }

    public class MouseHookEventArgs: EventArgs
    {
        #region variable

        public readonly MSLLHOOKSTRUCT msll;

        #endregion

        public MouseHookEventArgs(IntPtr lParam)
        {
            this.msll = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT))!;

            DeviceLocation = PodStructUtility.Convert(this.msll.pt);

            IsButtonEvent = false;
        }

        public MouseHookEventArgs(MouseButton mouseButton, MouseButtonState mouseButtonState, IntPtr lParam)
            : this(lParam)
        {
            Button = mouseButton;
            ButtonState = mouseButtonState;

            IsButtonEvent = true;
        }

        public MouseHookEventArgs(int buttonX, MouseButtonState mouseButtonState, in MSLLHOOKSTRUCT msll)
        {
            if(buttonX == 0) {
                throw new ArgumentException(null, nameof(buttonX));
            }

            this.msll = msll;

            Button = buttonX switch {
                1 => MouseButton.XButton1,
                2 => MouseButton.XButton2,
                _ => throw new NotImplementedException(),
            };

            ButtonState = mouseButtonState;
            IsButtonEvent = true;
        }

        #region property

        /// <summary>
        /// マウスボタン系のイベントか。
        /// </summary>
        /// <remarks>
        /// <para>真の場合のみ、<see cref="Button"/>, <seealso cref="ButtonState"/> が有効な値となる。</para>
        /// </remarks>
        public bool IsButtonEvent { get; }

        [PixelKind(Px.Device)]
        public Point DeviceLocation { get; }

        public MouseButton Button { get; }
        public MouseButtonState ButtonState { get; }

        /// <summary>
        /// 処理したか。
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 生成時間。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        #endregion
    }

    public partial class MouseHook: HookBase
    {
        #region event

        public event EventHandler<MouseHookEventArgs>? MouseMove;
        public event EventHandler<MouseHookEventArgs>? MouseDown;
        public event EventHandler<MouseHookEventArgs>? MouseUp;

        #endregion

        public MouseHook(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        #endregion

        #region HookerBase

        protected override IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle)
        {
            return NativeMethods.SetWindowsHookEx(WH.WH_MOUSE_LL, hookProc, moduleHandle, 0);
        }

        //protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        //{
        //    if(IsSkipCode(code)) {
        //        return CallNextProcedure(code, wParam, lParam);
        //    }

        //    if(code != (int)HC.HC_ACTION) {
        //        return CallNextProcedure(code, wParam, lParam);
        //    }

        //    MouseHookEventArgs? e = null;
        //    EventHandler<MouseHookEventArgs>? target = null;

        //    switch(wParam.ToInt32()) {
        //        case (int)WM.WM_MOUSEMOVE:
        //            target = MouseMove;
        //            if(target != null) {
        //                e = new MouseHookEventArgs(lParam);
        //            }
        //            break;

        //        case (int)WM.WM_LBUTTONDOWN:
        //            target = MouseDownEvent;
        //            if(target != null) {
        //                e = new MouseHookEventArgs(MouseButton.Left, MouseButtonState.Pressed, lParam);
        //            }
        //            break;

        //        case (int)WM.WM_LBUTTONUP:
        //        case (int)WM.WM_RBUTTONDOWN:
        //        case (int)WM.WM_RBUTTONUP:
        //        case (int)WM.WM_MBUTTONDOWN:
        //        case (int)WM.WM_MBUTTONUP:
        //        case (int)WM.WM_XBUTTONUP:
        //        case (int)WM.WM_XBUTTONDOWN:

        //            Logger.LogInformation("{0}", (WM)wParam.ToInt32());
        //            break;
        //        default:
        //            break;
        //    }

        //    if(target != null) {
        //        Debug.Assert(e != null);
        //        target.Invoke(this, e);
        //        if(e.Handled) {
        //            return new IntPtr(1);
        //        }
        //    }
        //    return CallNextProcedure(code, wParam, lParam);
        //}

        #endregion
    }
}
