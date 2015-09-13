namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public static class ThicknessExtension
	{
		public static double GetHorizon(this Thickness t)
		{
			return t.Left + t.Right;
		}
		
		public static double GetVertical(this Thickness t)
		{
			return t.Top + t.Bottom;
		}
	}
}
