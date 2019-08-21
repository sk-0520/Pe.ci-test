using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Windows
{
    public interface IWindowsHandle
    {
        /// <summary>
        /// h。
        /// </summary>
        IntPtr Handle { get; }
    }
}
