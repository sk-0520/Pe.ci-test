namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class LauncherViewModel: SingleModelWrapperViewModelBase<LauncherItemModel>
	{
		public LauncherViewModel(LauncherItemModel model)
			: base(model)
		{ }

		#region property

		public string ToolbarText { get { return Model.Name; } }
		public ImageSource ToolbarImage { get { return null; } }

		public Visibility VisibilityFile { get { return Model.LauncherKind == LauncherKind.File ? Visibility.Visible : Visibility.Collapsed; } }
		public Visibility VisibilityDirectory { get { return Model.LauncherKind == LauncherKind.Directory ? Visibility.Visible : Visibility.Collapsed; } }
		public Visibility VisibilityCommand { get { return Model.LauncherKind == LauncherKind.Command ? Visibility.Visible : Visibility.Collapsed; } }

		#endregion
	}
}
