using System;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// Formsの<see cref="WinForms.Form"/>をウィンドウとして扱う。
    /// </summary>
    /// <remarks>
    /// <para>要はウィンドウハンドル欲しい。</para>
    /// </remarks>
    public class CompatibleFormWindow: WinForms.IWin32Window, IWindowsHandle
    {
        /// <summary>
        /// ,生成。
        /// </summary>
        /// <param name="window"></param>
        public CompatibleFormWindow(Window window)
        {
            Handle = HandleUtility.GetWindowHandle(window);
        }

        #region IWin32Window, IWindowsHandle

        /// <inheritdoc cref="WinForms.IWin32Window.Handle"/>
        public IntPtr Handle { get; }

        #endregion
    }
}
