namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherButtonViewModel: LauncherSimpleViewModel, IHavingClipboardWatcher
	{
		#region varable

		IconScale _iconScale;

		bool _existsCommand;
		bool _existsParentDir;
		bool _existsWorkDir;

		bool _hasDataCommand;
		bool _hasDataParentDir;
		bool _hasDataWorkDir;

		#endregion

		public LauncherButtonViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess, IClipboardWatcher clipboardWatcher)
			: base(model, launcherIconCaching, nonPorocess)
		{
			ClipboardWatcher = clipboardWatcher;
		}

		#region property


		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion
	
		public IconScale IconScale 
		{
			get { return this._iconScale; }
			set
			{
				//if(this._iconScale != value) {
				//	this._iconScale = value;
				//	OnPropertyChanged();
				//	OnPropertyChanged("ToolbarImage");
				//}
				if(SetVariableValue(ref this._iconScale, value)) {
					OnPropertyChanged("ToolbarImage");
				}
			}
		}

		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(Model); } }
		public ImageSource ToolbarImage { get { return GetIcon(IconScale); } }
		public Color ToolbarHotTrack { get { return GetIconColor(IconScale); } }

		public Visibility VisibilityFile { get { return ToVisibility(Model.LauncherKind == LauncherKind.File); } }
		//public Visibility VisibilityDirectory { get { return ToVisibility(Model.LauncherKind == LauncherKind.Directory); } }
		public Visibility VisibilityCommand { get { return ToVisibility(Model.LauncherKind == LauncherKind.Command); } }

		public bool ExistsCommand
		{
			get { return this._existsCommand; }
			set
			{
				//if(this._existsCommand != value) {
				//	this._existsCommand = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._existsCommand, value);
			}
		}
		public bool ExistsParentDirectory
		{
			get { return this._existsParentDir; }
			set
			{
				//if(this._existsParentDir != value) {
				//	this._existsParentDir = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._existsParentDir, value);
			}
		}
		public bool ExistsWorkDirectory
		{
			get { return this._existsWorkDir; }
			set
			{
				//if(this._existsWorkDir != value) {
				//	this._existsWorkDir = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._existsWorkDir, value);
			}
		}

		public bool HasDataCommand
		{
			get { return this._hasDataCommand; }
			set
			{
				//if(this._hasDataCommand != value) {
				//	this._hasDataCommand = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._hasDataCommand, value);
			}
		}
		public bool HasDataParentDirectory
		{
			get { return this._hasDataParentDir; }
			set
			{
				//if(this._hasDataParentDir != value) {
				//	this._hasDataParentDir = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._hasDataParentDir, value);
			}
		}
		public bool HasDataWorkDirectory
		{
			get { return this._hasDataWorkDir; }
			set
			{
				//if(this._hasDataWorkDir != value) {
				//	this._hasDataWorkDir = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._hasDataWorkDir, value);
			}
		}

		#endregion

		#region command

		public ICommand OpenDropDownCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Application.Current.Dispatcher.BeginInvoke(new Action(CalculateStatus));
					}
				);

				return result;
			}
		}

		public ICommand RunCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Execute();
					}
				);

				return result;
			}
		}

		public ICommand OpenDirectory
		{
			get
			{
				var result = CreateCommand(
					o => {
						var type = (LauncherCommandType)o;
						var map = new Dictionary<LauncherCommandType, Func<string>>() {
							{ LauncherCommandType.ParentDirectory, () => Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(Model.Command)) },
							{ LauncherCommandType.WorkDirectory, () => Environment.ExpandEnvironmentVariables(Model.WorkDirectoryPath) },
						};
						var s = map[type]();

						ExecuteUtility.OpenDirectory(s, NonProcess, default(LauncherItemModel));
					}
				);

				return result;
			}
		}

		public ICommand CopyText
		{
			get
			{
				var result = CreateCommand(
					o => {
						var type = (LauncherCommandType)o;
						var map = new Dictionary<LauncherCommandType, Func<string>>() {
							{ LauncherCommandType.Command, () => Model.Command },
							{ LauncherCommandType.ParentDirectory, () => Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(Model.Command)) },
							{ LauncherCommandType.WorkDirectory, () => Model.WorkDirectoryPath },
						};
						var s = map[type]();

						ClipboardUtility.CopyText(s, ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand OpenProperty
		{
			get
			{
				var result = CreateCommand(
					o => {
						var s = Environment.ExpandEnvironmentVariables(Model.Command);
						ExecuteUtility.OpenProperty(s, IntPtr.Zero);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		Visibility ToVisibility(bool test)
		{
			return test ? Visibility.Visible : Visibility.Collapsed;
		}

		void CalculateStatus()
		{
			// コマンド
			var command = Environment.ExpandEnvironmentVariables(Model.Command ?? string.Empty);
			HasDataCommand = !string.IsNullOrWhiteSpace(command);

			// 作業ディレクトリ
			var workDir = Environment.ExpandEnvironmentVariables(Model.WorkDirectoryPath ?? string.Empty);
			HasDataWorkDirectory = !string.IsNullOrWhiteSpace(workDir);
			ExistsWorkDirectory = HasDataWorkDirectory && Directory.Exists(workDir);

			switch(Model.LauncherKind) {
				case LauncherKind.File: 
					{
						// ファイル(ディレクトリとして有効か)
						ExistsCommand = HasDataCommand && FileUtility.Exists(command);

						// 親ディレクトリ
						var parentDir = Path.GetDirectoryName(command);
						HasDataParentDirectory = !string.IsNullOrWhiteSpace(parentDir);
						ExistsParentDirectory = ExistsCommand && HasDataParentDirectory && Directory.Exists(parentDir);
					}
					break;
			}
		}

		#endregion
	}
}
