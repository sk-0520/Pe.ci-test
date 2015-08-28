namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;

	public class ScreenViewModel : HavingViewModelBase<ScreenWindow>, IHavingAppNonProcess
	{
		#region variable
		#endregion

		public ScreenViewModel(ScreenWindow view, ScreenModel screen, IAppNonProcess appNonProcess)
			: base(view)
		{
			Screen = screen;
			AppNonProcess = appNonProcess;
		}

		#region property

		ScreenModel Screen { get; set; }

		public Brush Background
		{
			get
			{
				var result = new SolidColorBrush();
				result.Color = Colors.Red;

				return result;
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
