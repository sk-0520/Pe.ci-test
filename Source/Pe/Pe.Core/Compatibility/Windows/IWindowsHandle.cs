using System;

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
