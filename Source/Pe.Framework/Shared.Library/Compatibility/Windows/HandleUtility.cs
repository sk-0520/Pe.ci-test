using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows
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
