namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
using System.Windows;

	public static class ConvertUtility
	{
		public static Visibility To(bool value)
		{
			return value ? Visibility.Visible : Visibility.Hidden;
		}

		public static bool To(Visibility value)
		{
			return value != Visibility.Hidden;
		}

	}
}
