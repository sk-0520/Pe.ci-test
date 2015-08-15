namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Shapes;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class WindowItemCollectionViewModel : SingleModelWrapperViewModelBase<WindowItemCollectionModel>, IMenuItem
	{
		public WindowItemCollectionViewModel(WindowItemCollectionModel model)
			:base(model)
		{ }

		#region command

		public ICommand WindowMenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						AppUtility.ChangeWindowFromWindowList(Model);
					}
				);

				return result;
			}
		}
		#endregion

		#region IMenuItem

		public FrameworkElement MenuImage
		{
			get 
			{
				return null; 
			}
		}

		public override string DisplayText
		{
			get
			{
				return Model.Name;
			}
		}

		#endregion
	}
}
