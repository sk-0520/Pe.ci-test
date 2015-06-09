namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;

	public static class UIUtility
	{
		/// <summary>
		/// <para>http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObj"></param>
		/// <returns></returns>
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
			where T: DependencyObject
		{
			if(depObj != null) {
				for(int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if(child != null && child is T) {
						yield return (T)child;
					}

					foreach(T childOfChild in FindVisualChildren<T>(child)) {
						yield return childOfChild;
					}
				}
			}
		}
	}
}
