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
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;
	using Microsoft.Win32;

	public class LauncherExecuteItemViewModel: LauncherSimpleItemViewModel, IHavingView<LauncherExecuteWindow>
	{
		#region variable

		EnvironmentVariablesItemModel _environmentVariablesItem;
		string _option;
		string _workDirPath;
		bool _stdStreamOutput;
		bool _admin;
		
		#endregion

		public LauncherExecuteItemViewModel(LauncherItemModel model, LauncherExecuteWindow view, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{
			View = view;

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
			get { return this._option ?? string.Empty; }
			set { SetVariableValue(ref this._option, value); }
		}

		public override string WorkDirectoryPath
		{
			get { return this._workDirPath ?? string.Empty; }
			set { SetVariableValue(ref this._workDirPath, value); }
		}

		public IEnumerable<string> Options
		{
			get
			{
				var result = new List<string>(1 + Model.History.Options.Count);

				result.Add(Option);
				result.AddRange(Model.History.Options);

				return result;
			}
		}

		public IReadOnlyList<string> WorkDirectoryPaths
		{
			get 
			{
				var result = new List<string>(1 + Model.History.WorkDirectoryPaths.Count);

				result.Add(WorkDirectoryPath);
				result.AddRange(Model.History.WorkDirectoryPaths);

				return result;
			}
		}

		#endregion

		#region command

		public ICommand RunCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dummyModel = (LauncherItemModel)Model.DeepClone();
						dummyModel.Option = Option;
						dummyModel.WorkDirectoryPath = WorkDirectoryPath;
						dummyModel.StdStream.OutputWatch = StdStreamOutput;
						dummyModel.Administrator = Administrator;
						dummyModel.EnvironmentVariables = this._environmentVariablesItem;
						try {
							ExecuteUtility.RunItem(dummyModel, NonProcess);
							SettingUtility.IncrementLauncherItem(Model, Option, WorkDirectoryPath, NonProcess);
						} catch(Exception ex) {
							NonProcess.Logger.Warning(ex);
						}
					}
				);

				return result;
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(HasView) {
							View.Close();
						}
					}
				);

				return result;
			}
		}

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
						var dialogResult = OpenDirectoryDialog(Option);
						if(dialogResult != null) {
							Option = dialogResult;
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
					o => {
						var dialogResult = OpenDirectoryDialog(WorkDirectoryPath);
						if(dialogResult != null) {
							WorkDirectoryPath = dialogResult;
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		/// <summary>
		/// ディレクトリを開く。
		/// TODO: 独立。
		/// </summary>
		/// <param name="defPath">初期パス</param>
		/// <returns>選択されたディレクトリパス。未選択の場合は null 。</returns>
		string OpenDirectoryDialog(string defPath)
		{
			using(var dialog = new FolderBrowserDialog()) {
				var expandedPath = Environment.ExpandEnvironmentVariables(defPath);
				if(Directory.Exists(expandedPath)) {
					dialog.SelectedPath = expandedPath;
				}
				var dialogResult = dialog.ShowDialog();
				if(dialogResult.GetValueOrDefault()) {
					return dialog.SelectedPath;
				} else {
					return null;
				}
			}
		}

		/// <summary>
		/// 外部からデータを設定する。
		/// </summary>
		/// <param name="path"></param>
		public void SetFile(string path)
		{ }

		#endregion

		#region IHavingView

		public LauncherExecuteWindow View { get; private set; }

		public bool HasView { get { return View != null;} }


		#endregion
	}
}
