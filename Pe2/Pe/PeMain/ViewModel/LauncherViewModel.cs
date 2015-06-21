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
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherViewModel: LauncherViewModelBase
	{
		public LauncherViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{
			IconScale = IconScale.Small;
		}

		#region property

		public IconScale IconScale { get; set; }

		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(Model); } }
		public ImageSource ToolbarImage { get { return GetIcon(IconScale); } }

		public Visibility VisibilityFile { get { return ToVisibility(Model.LauncherKind == LauncherKind.File); } }
		//public Visibility VisibilityDirectory { get { return ToVisibility(Model.LauncherKind == LauncherKind.Directory); } }
		public Visibility VisibilityCommand { get { return ToVisibility(Model.LauncherKind == LauncherKind.Command); } }

		#endregion

		#region command

		public ICommand ExecuteCommand
		{
			get
			{
				var result = new DelegateCommand();
				result.Command = o => {
					NonProcess.Logger.Debug(Model.ToString());
				};
				return result;
			}
		}

		#endregion

		#region function

		Visibility ToVisibility(bool test)
		{
			return test ? Visibility.Visible : Visibility.Collapsed;
		}

		#endregion
	}
}
