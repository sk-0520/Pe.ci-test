namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls.Primitives;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public static class PopupUtility
	{
		public static void Attachment(Window window, Popup popup)
		{
			EventDisposer<EventHandler> locationEvent;
			window.LocationChanged += EventUtility.Create<EventHandler>(
				(sender, e) => ResetPopupPosition(popup),
				releaseEvent => window.LocationChanged -= releaseEvent,
				out locationEvent
			);

			EventDisposer<SizeChangedEventHandler> sizeEvent;
			window.SizeChanged += EventUtility.Create<SizeChangedEventHandler>(
				(sender, e) => ResetPopupPosition(popup),
				releaseEvent => window.SizeChanged -= releaseEvent,
				out sizeEvent
			);

			EventDisposer<EventHandler> closeEvent = null;
			window.Closed += EventUtility.Create<EventHandler>(
				(sender, e) => {
					locationEvent.Dispose();
					sizeEvent.Dispose();
					closeEvent.Dispose();

					locationEvent = null;
					sizeEvent = null;
				},
				releaseEvent => {
					window.Closed -= releaseEvent;
					closeEvent = null;
				},
				out closeEvent
			);
		}

		#region function

		static void ResetPopupPosition(Popup popup)
		{
			popup.HorizontalOffset += 1;
			popup.HorizontalOffset -= 1;
		}

		#endregion
	}
}
