namespace ContentTypeTextNet.Library.SharedLibrary.FormsCompatible.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using Forms = System.Windows.Forms;
	using Drawing = System.Drawing;

	public static class ConvertExtension
	{
		public static Rect ToRect(this Drawing.Rectangle rectangle)
		{
			return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}
}
