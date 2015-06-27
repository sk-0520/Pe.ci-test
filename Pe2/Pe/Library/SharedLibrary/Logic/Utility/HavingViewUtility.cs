namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public static class HavingViewUtility
	{
		public static bool GetHasView<TView>(IHavingView<TView> view)
			where TView: UIElement
		{
			return view.View != null;
		}
	}
}
