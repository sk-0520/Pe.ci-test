namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Drawing = System.Drawing;

	public static class FontUtility
	{
		/// <summary>
		/// FormsフォントサイズをWOFフォントサイズに変換。
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static double ConvertSizeFromDrawing(float size)
		{
			return size / 72.0 * 96.0;
		}

		/// <summary>
		/// WPFフォントサイズをFormsフォントサイズに変換。
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static float ConvertSizeFromWpf(double size)
		{
			return (float)(size * 72.0 / 96.0);
		}

	}
}
