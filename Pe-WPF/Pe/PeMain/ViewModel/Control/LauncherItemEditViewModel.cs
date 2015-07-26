namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherItemEditViewModel: LauncherItemSimpleViewModel
	{
		public LauncherItemEditViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{ }

		#region property

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		public override LauncherKind LauncherKind
		{
			get { return base.LauncherKind; }
			set { SetModelValue(value); }
		}

		public string IconPath
		{
			get { return Model.Icon.DisplayText; }
		}

		public EnvironmentVariablesEditViewModel EnvironmentVariables
		{
			get { return new EnvironmentVariablesEditViewModel(Model.EnvironmentVariables, NonProcess); }
		}

		#endregion

		#region command

		public ICommand SelectCommandFileCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dialogResult = LauncherItemUtility.ShowOpenCommandDialog(Command, NonProcess);
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
					}
				);

				return result;
			}
		}


		#endregion
	}
}
