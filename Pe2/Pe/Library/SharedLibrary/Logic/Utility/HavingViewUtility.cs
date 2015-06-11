namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public static class HavingViewUtility
	{
		public static void Initialize<TView>(IHavingView<TView> ui)
			where TView: Window
		{
			ui.View.Closed += View_Closed;
		}

		static void View_Closed(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
