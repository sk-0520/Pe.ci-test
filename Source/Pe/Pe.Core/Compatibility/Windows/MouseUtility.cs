using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Windows
{
    public class MouseUtility
    {
        /// <summary>
        /// マウスカーソルの現在位置を物理座標で取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Device)]
        public static Point GetDevicePosition()
        {
            var deviceCursolPosition = new POINT();
            NativeMethods.GetCursorPos(out deviceCursolPosition);

            return PodStructUtility.Convert(deviceCursolPosition);
        }
    }
}
