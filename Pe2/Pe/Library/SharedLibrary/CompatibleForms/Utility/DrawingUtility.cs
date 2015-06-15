namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using Drawing = System.Drawing;

	public static class DrawingUtility
	{
		public static Drawing.Size Convert(Size size)
		{
			return new Drawing.Size((int)size.Width, (int)size.Height);
		}
		public static Size Convert(Drawing.Size size)
		{
			return new Size(size.Width, size.Height);
		}
	}
}
