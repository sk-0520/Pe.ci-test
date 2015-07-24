namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;
	using Microsoft.Win32;

	public class LauncherExecuteItemViewModel : LauncherSimpleItemViewModel
	{
		#region variable

		EnvironmentVariablesItemModel _environmentVariablesItem;
		string _option;
		string _workDirPath;
		bool _stdStreamOutput;
		bool _admin;
		
		#endregion

		public LauncherExecuteItemViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{
			this._environmentVariablesItem = (EnvironmentVariablesItemModel)Model.EnvironmentVariables.DeepClone();
			EnvironmentVariables = new EnvironmentVariablesEditViewModel(this._environmentVariablesItem, NonProcess);

			this._option = Model.Option;
			this._workDirPath = Model.WorkDirectoryPath;
			this._stdStreamOutput = Model.StdStream.OutputWatch;
		}

		#region property

		public EnvironmentVariablesEditViewModel EnvironmentVariables { get; set; }

		public override bool StdStreamOutput
		{
			get { return this._stdStreamOutput; }
			set { SetVariableValue(ref this._stdStreamOutput, value); }
		}

		public override bool Administrator
		{
			get { return this._admin; }
			set { SetVariableValue(ref this._admin, value); }
		}

		public override string Option
		{
			get { return this._option; }
			set { SetVariableValue(ref this._option, value); }
		}

		public override string WorkDirectoryPath
		{
			get { return this._workDirPath; }
			set { SetVariableValue(ref this._workDirPath, value); }
		}

		#endregion

		#region command

		public ICommand OpenOptionFilesCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var defFilePath = File.Exists(Option) ? Option: string.Empty;
						var dialog = new OpenFileDialog() {
							CheckFileExists = true,
							FileName = defFilePath,
							Multiselect = true,
						};
						var dialogResult = dialog.ShowDialog();
						if (dialogResult.GetValueOrDefault()) {
							Option = string.Join(" ", TextUtility.WhitespaceToQuotation(dialog.FileNames));
						}
					}
				);

				return result;
			}
		}

		public ICommand OpenOptionDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						using (var dialog = new FolderBrowserDialog()) {
							var dialogResult = dialog.ShowDialog();
							if (dialogResult.GetValueOrDefault()) {

							}
						}
					}
				);

				return result;
			}
		}

		public ICommand OpenWorkDirectoryDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => { }
				);

				return result;
			}
		}

		#endregion
	}
}
