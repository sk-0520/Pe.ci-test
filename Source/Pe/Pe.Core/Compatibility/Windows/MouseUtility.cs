using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Windows
{
    public static class MouseUtility
    {
        /// <summary>
        /// マウスカーソルの現在位置を物理座標で取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Device)]
        public static Point GetDevicePosition()
        {
            NativeMethods.GetCursorPos(out var deviceCursorPosition);

            return PodStructUtility.Convert(deviceCursorPosition);
        }
    }
}
