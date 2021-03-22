using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    partial class NativeMethods
    {
        [DllImport("psapi")]
        public static extern bool EmptyWorkingSet(IntPtr hProcess);
    }
}
