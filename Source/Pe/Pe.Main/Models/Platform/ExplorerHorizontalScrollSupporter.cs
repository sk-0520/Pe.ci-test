using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class ExplorerHorizontalScrollSupporter : DisposerBase
    {
        #region define

        const string Windows10ChildClass = "win10";

        #endregion
        public ExplorerHorizontalScrollSupporter(TimeSpan checkSpan, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            CheckSpan = checkSpan;

            Timer = new Timer() {
                Interval = CheckSpan.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        #region property
        ILogger Logger { get; }

        Timer Timer { get; }

        public TimeSpan CheckSpan { get; }

        int WindowClassNameLength { get; } = 128;

        IReadOnlyList<string> ExplorerClassNames { get; } = new string[] {
            "CabinetWClass", // win10だけならこれでいいはず(他は知らん)
        };

        IReadOnlyDictionary<string, IReadOnlyList<string>> ChildClassNameTrees { get; } = new Dictionary<string, IReadOnlyList<string>> {
            [Windows10ChildClass] = new string[] {
                "ShellTabWindowClass",
                "DUIViewWndClassName",
                "DirectUIHWND",
                "CtrlNotifySink",
                "NamespaceTreeControl",
                "SysTreeView32",
            }
        };

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

        private IReadOnlyList<IntPtr> GetExplorerTreeViewHandles(IEnumerable<IntPtr> hParentWnds, IReadOnlyList<string> classTrees)
        {
            var result = new List<IntPtr>();

            foreach(var hParentWnd in hParentWnds) {
                var hTreeView = GetExplorerTreeViewHandle(hParentWnd, classTrees);
                if(hTreeView != IntPtr.Zero) {
                    result.Add(hTreeView);
                }
            }

            return result;
        }

        public void Refresh()
        {
            var explorers = GetExplorerWindowHandles();
            var win10 = ChildClassNameTrees[Windows10ChildClass];
            var treeViews = GetExplorerTreeViewHandles(explorers, win10);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Stop();
                    Timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stop();

            Refresh();

            Start();
        }

    }
}
