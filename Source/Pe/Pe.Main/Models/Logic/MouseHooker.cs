// [T4] build 2020-04-29 09:14:59Z(UTC)
using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    partial class MouseHooker
    {
        protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            if(IsSkipCode(code)) {
                return CallNextProcedure(code, wParam, lParam);
            }

            if(code != (int)HC.HC_ACTION) {
                return CallNextProcedure(code, wParam, lParam);
            }

            MouseHookEventArgs? e = null;
            EventHandler<MouseHookEventArgs>? target = null;

            
            switch(wParam.ToInt32()) {
                case (int)WM.WM_MOUSEMOVE:
                    target = MouseMove;
                    if(target != null) {
                        e = new MouseHookEventArgs(lParam);
                    }
                    break;

                #region 機械生成
                #region 通常ボタン
                
                case (int)WM.WM_LBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Left, MouseButtonState.Pressed, lParam);
                    }
                    break;

                
                case (int)WM.WM_RBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Right, MouseButtonState.Pressed, lParam);
                    }
                    break;

                
                case (int)WM.WM_MBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Middle, MouseButtonState.Pressed, lParam);
                    }
                    break;

                
                case (int)WM.WM_LBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Left, MouseButtonState.Released, lParam);
                    }
                    break;

                
                case (int)WM.WM_RBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Right, MouseButtonState.Released, lParam);
                    }
                    break;

                
                case (int)WM.WM_MBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Middle, MouseButtonState.Released, lParam);
                    }
                    break;

                
                #endregion

                #region Xボタン
                
                case (int)WM.WM_XBUTTONDOWN: {
                        var msll = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT))!;
                        var xbutton = WindowsUtility.HIWORD(msll.mouseData);
                        if(xbutton == (int)XBUTTON.XBUTTON1) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(1, MouseButtonState.Pressed, msll);
                            }
                        } else if (xbutton == (int)XBUTTON.XBUTTON2) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(2, MouseButtonState.Pressed, msll);
                            }
                        }
                    }
                    break;

                
                case (int)WM.WM_XBUTTONUP: {
                        var msll = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT))!;
                        var xbutton = WindowsUtility.HIWORD(msll.mouseData);
                        if(xbutton == (int)XBUTTON.XBUTTON1) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(1, MouseButtonState.Released, msll);
                            }
                        } else if (xbutton == (int)XBUTTON.XBUTTON2) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(2, MouseButtonState.Released, msll);
                            }
                        }
                    }
                    break;

                
                #endregion

                #endregion

                default:
                    break;
            }

            if(target != null) {
                Debug.Assert(e != null);
                target.Invoke(this, e);
                if(e.Handled) {
                    return new IntPtr(1);
                }
            }
            return CallNextProcedure(code, wParam, lParam);
        }
    }
}

