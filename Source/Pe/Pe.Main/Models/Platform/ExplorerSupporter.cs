using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// Windows Explorer に対する(独善的)補助処理。
    /// </summary>
    /// <remarks>
    /// <para>今んとこフォルダツリーの横スクロールくらい。</para>
    /// </remarks>
    public class ExplorerSupporter: DisposerBase
    {
        #region define

        private const string Windows10ChildClass = "win10";


        #endregion

        public ExplorerSupporter(TimeSpan checkSpan, int cacheSize, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            CheckSpan = checkSpan;
            Timer = new Timer() {
                Interval = CheckSpan.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;

            IgnoreHorizontalScrollbarExplorerHandles = new FixedQueue<IntPtr>(cacheSize);
        }

        #region property

        private ILogger Logger { get; }

        private Timer Timer { get; }

        public TimeSpan CheckSpan { get; }

        private int WindowClassNameLength { get; } = WindowsUtility.WindowClassNameLength;

        private IReadOnlyList<string> ExplorerClassNames { get; } = new string[] {
            "CabinetWClass", // win10だけならこれでいいはず(他は知らん)
        };

        private IReadOnlyDictionary<string, IReadOnlyList<string>> ChildClassNameTrees { get; } = new Dictionary<string, IReadOnlyList<string>> {
            [Windows10ChildClass] = new string[] {
                "ShellTabWindowClass",
                "DUIViewWndClassName",
                "DirectUIHWND",
                "CtrlNotifySink",
                "NamespaceTreeControl",
                "SysTreeView32",
            }
        };

        private IFixedQueue<IntPtr> IgnoreHorizontalScrollbarExplorerHandles { get; }

        #endregion

        #region function

        public void Start()
        {
            Timer.Stop();
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        private string GetClassName(IntPtr hWnd)
        {
            var buffer = new StringBuilder(WindowClassNameLength);
            NativeMethods.GetClassName(hWnd, buffer, buffer.Capacity);

            var className = buffer.ToString();
            return className;
        }

        private IReadOnlyList<IntPtr> GetExplorerWindowHandles()
        {
            var result = new List<IntPtr>();

            NativeMethods.EnumWindows((hWnd, lParam) => {
                if(!NativeMethods.IsWindowVisible(hWnd)) {
                    return true;
                }

                var className = GetClassName(hWnd);
                if(ExplorerClassNames.Contains(className)) {
                    result.Add(hWnd);
                }

                return true;
            }, IntPtr.Zero);

            return result;
        }

        private IntPtr GetExplorerTreeViewHandle(IntPtr hParentWnd, IReadOnlyList<string> classTrees)
        {
            IntPtr result = IntPtr.Zero;

            if(!classTrees.Any()) {
                return result;
            }
            var className = classTrees.First();
            var count = classTrees.Count();

            NativeMethods.EnumChildWindows(hParentWnd, (hWnd, lParam) => {
                var currentClassName = GetClassName(hWnd);
                if(className != currentClassName) {
                    return true;
                }

                if(count == 1) {
                    result = hWnd;
                    return false;
                }

                var hTreeView = GetExplorerTreeViewHandle(hWnd, classTrees.Skip(1).ToList());
                if(hTreeView == IntPtr.Zero) {
                    return true;
                }

                result = hTreeView;

                return false;
            }, IntPtr.Zero);

            return result;
        }

        //private IReadOnlyList<IntPtr> GetExplorerTreeViewHandles(IEnumerable<IntPtr> hParentWnds, IReadOnlyList<string> classTrees)
        //{
        //    var result = new List<IntPtr>();

        //    foreach(var hParentWnd in hParentWnds) {
        //        var hTreeView = GetExplorerTreeViewHandle(hParentWnd, classTrees);
        //        if(hTreeView != IntPtr.Zero) {
        //            result.Add(hTreeView);
        //        }
        //    }

        //    return result;
        //}

        //TODO: 32/64bit の分離はもう不要
        private void SetHorizontalScrollbarForTreeView(IntPtr treeViewHandle, bool isSet)
        {
            var rawStyle = WindowsUtility.GetWindowLongPtr(treeViewHandle, (int)GWL.GWL_STYLE);
            if(isSet) {
                if(Environment.Is64BitProcess) {
                    var style = rawStyle;
                    if((style & (int)TVS.TVS_NOHSCROLL) == (int)TVS.TVS_NOHSCROLL) {
                        style &= ~(int)TVS.TVS_NOHSCROLL;
                        WindowsUtility.SetWindowLongPtr(treeViewHandle, (int)GWL.GWL_STYLE, style);
                    }
                } else {
                    var style = rawStyle;
                    if((style & (int)TVS.TVS_NOHSCROLL) == (int)TVS.TVS_NOHSCROLL) {
                        style &= ~(int)TVS.TVS_NOHSCROLL;
                        WindowsUtility.SetWindowLongPtr(treeViewHandle, (int)GWL.GWL_STYLE, style);
                    }
                }
            } else {
                if(Environment.Is64BitProcess) {
                    var style = rawStyle;
                    if((style & (int)TVS.TVS_NOHSCROLL) != (int)TVS.TVS_NOHSCROLL) {
                        style |= (int)TVS.TVS_NOHSCROLL;
                        WindowsUtility.SetWindowLongPtr(treeViewHandle, (int)GWL.GWL_STYLE, style);
                    }
                } else {
                    var style = rawStyle;
                    if((style & (int)TVS.TVS_NOHSCROLL) != (int)TVS.TVS_NOHSCROLL) {
                        style &= ~(int)TVS.TVS_NOHSCROLL;
                        WindowsUtility.SetWindowLongPtr(treeViewHandle, (int)GWL.GWL_STYLE, style);
                    }
                }
            }
        }

        private void SetHorizontalScrollbarForTreeViews(bool isEnable)
        {
            var win10 = ChildClassNameTrees[Windows10ChildClass];

            var hExplorers = GetExplorerWindowHandles();
            foreach(var hExplorer in hExplorers) {
                if(isEnable) {
                    if(IgnoreHorizontalScrollbarExplorerHandles.Contains(hExplorer)) {
                        continue;
                    }
                }

                var hTreeView = GetExplorerTreeViewHandle(hExplorer, win10);
                if(hTreeView != IntPtr.Zero) {
                    SetHorizontalScrollbarForTreeView(hTreeView, isEnable);

                    if(isEnable) {
                        IgnoreHorizontalScrollbarExplorerHandles.Enqueue(hExplorer);
                    }
                }
            }
        }

        public void Refresh()
        {
            SetHorizontalScrollbarForTreeViews(true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Stop();
                    SetHorizontalScrollbarForTreeViews(false);
                    Timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Stop();

            Refresh();

            Start();
        }

    }
}
