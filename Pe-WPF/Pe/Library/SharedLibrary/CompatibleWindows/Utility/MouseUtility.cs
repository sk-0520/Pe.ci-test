namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using Drawing = System.Drawing;

	public class MouseUtility
	{
		/// <summary>
		/// マウスカーソルの現在位置をデバイス座標で取得。
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
