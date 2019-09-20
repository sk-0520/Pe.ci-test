using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows
{
    public interface IWindowsHandle
    {
        /// <summary>
        /// h。
        /// </summary>
        IntPtr Handle { get; }
    }
}
