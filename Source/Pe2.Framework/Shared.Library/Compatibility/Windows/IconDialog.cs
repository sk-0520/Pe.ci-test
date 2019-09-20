using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.PInvoke.Windows;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows
{
    /// <summary>
    /// アイコン選択ダイアログ。
    /// </summary>
    public class IconDialog : CommonDialog
    {
        public IconDialog()
            : base()
        {
            ResetCore();
        }

        #region property

        /// <summary>
        /// アイコン情報。
        /// </summary>
        public string IconPath { get; set; }
        public int IconIndex { get; set; }

        #endregion

        #region function

        void ResetCore()
        {
            IconPath = string.Empty;
            IconIndex = 0;
        }

        #endregion

        #region CommonDialog

        public override void Reset()
        {
            ResetCore();
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            var iconIndex = IconIndex;
            var sb = new StringBuilder(IconPath, (int)MAX.MAX_PATH);
            var result = NativeMethods.SHChangeIconDialog(hwndOwner, sb, sb.Capacity, ref iconIndex);
            if(result) {
                IconIndex = iconIndex;
                IconPath = sb.ToString();
            }

            return result;
        }

        #endregion
    }
}
