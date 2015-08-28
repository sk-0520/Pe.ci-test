namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class ScreenViewModel : HavingViewModelBase<ScreenWindow>, IHavingAppNonProcess
	{
		#region variable

		Color _backColor;

		#endregion

		public ScreenViewModel(ScreenWindow view, ScreenModel screen, IAppNonProcess appNonProcess)
			: base(view)
		{
			Screen = screen;
			AppNonProcess = appNonProcess;

			this._backColor = Colors.Red;
		}

		#region property

		ScreenModel Screen { get; set; }

		public Brush Foreground
		{
			get 
			{
				var result = new SolidColorBrush();
				result.Color = MediaUtility.GetAutoColor(this._backColor);

				return result;
			}
		}

		public Brush Background
		{
			get
			{
				var result = new SolidColorBrush();
				result.Color = this._backColor;

				return result;
			}
		}

		public string ScreenName
		{
			get { return ScreenUtility.GetScreenName(Screen); }
		}

		public string DeviceName
		{
			get { return Screen.DeviceName; }
		}

		public bool IsPrimaryScreen
		{
			get { return Screen.Primary; }
		}

		#endregion

		#region function

		#endregion

		#region HavingViewModelBase

		protected override void InitializeView()
		{
			base.InitializeView();

			View.Loaded += View_Loaded;
		}

		protected override void UninitializeView()
		{
			View.Loaded -= View_Loaded;

			base.UninitializeView();
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			var deviceArea = PodStructUtility.Convert(Screen.DeviceBounds);
			NativeMethods.MoveWindow(View.Handle, deviceArea.X, deviceArea.Y, deviceArea.Width, deviceArea.Height, true);
		}
	}
}
