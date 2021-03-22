using System;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <inheritdoc cref="System.Windows.Forms.SystemInformation"/>
    public static class SystemInformation
    {
        /// <inheritdoc cref="System.Windows.Forms.SystemInformation.DoubleClickTime"/>
        public static TimeSpan DoubleClickTime
        {
            get { return TimeSpan.FromMilliseconds(WinForms.SystemInformation.DoubleClickTime); }
        }
    }
}
