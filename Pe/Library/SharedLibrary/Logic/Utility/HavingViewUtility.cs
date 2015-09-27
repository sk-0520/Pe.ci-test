namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public static class HavingViewUtility
	{
		public static bool GetHasView<TView>(IHavingView<TView> view)
			where TView: UIElement
		{
			return view.View != null;
		}

		public static DispatcherOperation BeginInvoke<TView>(IHavingView<TView> view, Action action, DispatcherPriority priority, params object[] args)
			where TView: UIElement
		{
			Func<Action, DispatcherPriority, object[], DispatcherOperation> beginInvoke;
			
			if(view.HasView) {
				beginInvoke = view.View.Dispatcher.BeginInvoke;
			} else {
				beginInvoke = Dispatcher.CurrentDispatcher.BeginInvoke;
			}

			return beginInvoke(action, priority, args);
		}

		public static DispatcherOperation BeginInvoke<TView>(IHavingView<TView> view, Action action, params object[] args)
			where TView: UIElement
		{
			Func<Action, object[], DispatcherOperation> beginInvoke;

			if(view.HasView) {
				beginInvoke = view.View.Dispatcher.BeginInvoke;
			} else {
				beginInvoke = Dispatcher.CurrentDispatcher.BeginInvoke;
			}

			return beginInvoke(action, args);
		}
	}
}
