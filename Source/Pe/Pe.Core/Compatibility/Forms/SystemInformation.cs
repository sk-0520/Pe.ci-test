using System;
using System.Collections.Generic;
using System.Text;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Windows.Forms.SystemInformation" /> の互換ラッパー。
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// マウス操作がダブルクリックであると OS に認識されるための、1 回目のクリックと 2 回目のクリックの間の最大経過時間 (ミリ秒単位) を取得します。
        /// </summary>
        public static TimeSpan DoubleClickTime
        {
            get { return TimeSpan.FromMilliseconds(WinForms.SystemInformation.DoubleClickTime); }
        }
    }
}
