namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	public static class PodStructUtility
	{
		public static RECT Convert(Rect rect)
		{
			var result = new RECT() {
				Left = (int)rect.Left,
				Top = (int)rect.Top,
				Width = (int)rect.Width,
				Height = (int)rect.Height,
			};

			return result;
		}

		public static Rect Convert(RECT rect)
		{
			var result = new Rect(
				rect.Left,
				rect.Top,
				rect.Width,
				rect.Height
			);

			return result;
		}
	}
}
