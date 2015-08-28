namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
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

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
