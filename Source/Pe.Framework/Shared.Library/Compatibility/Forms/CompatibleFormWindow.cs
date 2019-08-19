using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms
{
    /// <summary>
    /// FormsのFormをウィンドウとして扱う。
    /// <para>要はウィンドウハンドル欲しい。</para>
    /// </summary>
    public class CompatibleFormWindow : WinForms.IWin32Window, IWindowsHandle
    {
        public CompatibleFormWindow(Window window)
        {
            Handle = HandleUtility.GetWindowHandle(window);
        }

        #region IWin32Window, IWindowsHandle

        /// <summary>
        /// ウィンドウハンドル。
        /// </summary>
        public IntPtr Handle { get; }

        #endregion
    }
}
