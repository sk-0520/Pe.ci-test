using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PInvoke.Windows;

namespace PeUtility
{
    public class OpenIconDialog: CommonDialog
    {
        public OpenIconDialog()
            : base()
        {
            IconPath = new IconPath();
        }
        /*
        public string IconPath { set; get; }
        public int IconIndex { set; get; }
         */

        public IconPath IconPath { set; get; }

        //表示
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            var iconIndex = IconPath.Index;
            var sb = new StringBuilder(IconPath.Path, (int)MAX.MAX_PATH);
            var result = NativeMethods.SHChangeIconDialog(hwndOwner, sb, sb.Capacity, ref iconIndex);
            if(result) {
                IconPath.Index = iconIndex;
                IconPath.Path = sb.ToString();
            }

            return result;
        }
        //ダイアログを初期化する。
        public override void Reset()
        {
            /*
            IconPath = string.Empty;
            IconIndex = 0;
             */
        }
    }
}
