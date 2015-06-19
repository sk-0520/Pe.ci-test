namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherViewModel: SingleModelWrapperViewModelBase<LauncherItemModel>
	{
		public LauncherViewModel(LauncherItemModel model)
			: base(model)
		{ }

		#region property

		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(Model); } }
		public ImageSource ToolbarImage { get { return null; } }

		public Visibility VisibilityFile { get { return ToVisibility(Model.LauncherKind == LauncherKind.File); } }
		//public Visibility VisibilityDirectory { get { return ToVisibility(Model.LauncherKind == LauncherKind.Directory); } }
		public Visibility VisibilityCommand { get { return ToVisibility(Model.LauncherKind == LauncherKind.Command); } }


		#endregion

		#region function

		Visibility ToVisibility(bool test)
		{
			return test ? Visibility.Visible : Visibility.Collapsed;
		}

		#endregion
	}
}
