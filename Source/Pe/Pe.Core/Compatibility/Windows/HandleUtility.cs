using System;
using System.Windows;
using System.Windows.Interop;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Windows
{
    public static class HandleUtility
    {
        /// <summary>
        /// ウィンドウハンドル取得。
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IntPtr GetWindowHandle(Window view)
        {
            var helper = new WindowInteropHelper(view);
            return helper.Handle;
        }
    }
}
