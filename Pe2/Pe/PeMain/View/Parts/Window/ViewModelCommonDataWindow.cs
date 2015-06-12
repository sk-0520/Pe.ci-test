namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Window
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ViewModelCommonDataWindow<TViewModel> : CommonDataWindow, IHavingViewModel<TViewModel>
		where TViewModel : ViewModelBase
	{
		public ViewModelCommonDataWindow()
			:base()
		{ }

		#region property

		#region TViewModel

		public TViewModel ViewModel { get; protected set; }

		#endregion

		#endregion

		#region CommonDataWindow

		protected override void CreateViewModel()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
