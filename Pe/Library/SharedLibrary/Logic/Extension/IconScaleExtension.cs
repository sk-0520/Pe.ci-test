namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public static class IconScaleExtension
	{
		public static double ToWidth(this IconScale iconScale)
		{
			return (int)iconScale;
		}

		public static double ToHeight(this IconScale iconScale)
		{
			return (int)iconScale;
		}

		public static Size ToSize(this IconScale iconScale)
		{
			return new Size((int)iconScale, (int)iconScale);
		}
	}
}
