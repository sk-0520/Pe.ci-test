using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class DeviceChangedData
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        public DeviceChangedData(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            DBT = (DBT)wParam.ToInt32();
        }

        #region property

        /// <summary>
        /// DBT! DBT!
        /// </summary>
        public DBT DBT { get; }

        #endregion
    }
}
