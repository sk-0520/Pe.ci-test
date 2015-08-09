namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherItemEditViewModel: LauncherItemSimpleViewModel
	{
		#region define

		class NullRefreshFromViewModel: IRefreshFromViewModel
		{
			public void Refresh()
			{ }
		}

		#endregion

		public LauncherItemEditViewModel(LauncherItemModel model, IRefreshFromViewModel refreshFromViewModel, IAppNonProcess nonPorocess, IAppSender appSender)
			: base(model, nonPorocess, appSender)
		{
			if(refreshFromViewModel == null) {
				RefreshFromViewModel = new NullRefreshFromViewModel();
			} else {
				RefreshFromViewModel = refreshFromViewModel;
			}
		}

		#region property

		public IRefreshFromViewModel RefreshFromViewModel { get; private set; }

		public string Name
		{
			get { return Model.Name; }
			set
			{
				if(SetModelValue(value)) {
					OnPropertyChangeDisplayItem();
				}
			}
		}

		

		public override LauncherKind LauncherKind
		{
			get { return base.LauncherKind; }
			set { SetModelValue(value); }
		}

		public string IconDisplayText
		{
			get { return Model.Icon.DisplayText; }
		}

		public EnvironmentVariablesEditViewModel EnvironmentVariables
		{
			get { return new EnvironmentVariablesEditViewModel(Model.EnvironmentVariables, AppNonProcess); }
		}

		#endregion

		#region command

		public ICommand SelectCommandFileCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialogResult = LauncherItemUtility.ShowOpenCommandDialog(Command, AppNonProcess);
						if(dialogResult != null) {
							Command = dialogResult;
						}
					}
				);

				return result;
			}
		}

		public ICommand SelectCommandDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialogResult = DialogUtility.ShowDirectoryDialog(Command);
						if(dialogResult != null) {
							Command = dialogResult;
						}
					}
				);

				return result;
			}
		}

		public ICommand SelectOptionFilesCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var files = LauncherItemUtility.ShowOpenOptionDialog(Option);
						if(files != null) {
							Option = files;
						}
					}
				);

				return result;
			}
		}

		public ICommand SelectOptionDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialogResult = DialogUtility.ShowDirectoryDialog(Option);
						if(dialogResult != null) {
							Option = dialogResult;
						}
					}
				);

				return result;
			}
		}

		public ICommand SelectWorkDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialogResult = DialogUtility.ShowDirectoryDialog(WorkDirectoryPath);
						if(dialogResult != null) {
							WorkDirectoryPath = dialogResult;
						}
					}
				);

				return result;
			}
		}

		public ICommand SelectIconCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialog = new IconDialog();
						dialog.Icon.Path = Environment.ExpandEnvironmentVariables(Icon.Path ?? string.Empty);
						dialog.Icon.Index = Icon.Index;
						var dialogResult = dialog.ShowDialog();
						if(dialogResult.GetValueOrDefault()) {
							Icon.Path = dialog.Icon.Path;
							Icon.Index = dialog.Icon.Index;
							OnPropertyChanged("IconDisplayText");
							
							RefreshFromViewModel.Refresh();
						}
					}
				);

				return result;
			}
		}


		#endregion

		#region LauncherItemSimpleViewModel
		
		protected override void OnPropertyChangeDisplayItem()
		{
			base.OnPropertyChangeDisplayItem();
			RefreshFromViewModel.Refresh();
		}

		#endregion
	}
}
