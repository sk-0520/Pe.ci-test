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
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherViewModel: LauncherViewModelBase
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

		public LauncherViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{ }

		#region property

		public IconScale IconScale 
		{
			get { return this._iconScale; }
			set
			{
				if(this._iconScale != value) {
					this._iconScale = value;
					OnPropertyChanged();
					OnPropertyChanged("ToolbarImage");
				}
			}
		}

		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(Model); } }
		public ImageSource ToolbarImage { get { return GetIcon(IconScale); } }

		public Visibility VisibilityFile { get { return ToVisibility(Model.LauncherKind == LauncherKind.File); } }
		//public Visibility VisibilityDirectory { get { return ToVisibility(Model.LauncherKind == LauncherKind.Directory); } }
		public Visibility VisibilityCommand { get { return ToVisibility(Model.LauncherKind == LauncherKind.Command); } }

		public bool ExistsCommand
		{
			get { return this._existsCommand; }
			set
			{
				if(this._existsCommand != value) {
					this._existsCommand = value;
					OnPropertyChanged();
				}
			}
		}

		public bool ExistsParentDirectory
		{
			get { return this._existsParentDir; }
			set
			{
				if(this._existsParentDir != value) {
					this._existsParentDir = value;
					OnPropertyChanged();
				}
			}
		}
		public bool ExistsWorkDirectory
		{
			get { return this._existsWorkDir; }
			set
			{
				if(this._existsWorkDir != value) {
					this._existsWorkDir = value;
					OnPropertyChanged();
				}
			}
		}

		public bool HasDataCommand
		{
			get { return this._hasDataCommand; }
			set
			{
				if(this._hasDataCommand != value) {
					this._hasDataCommand = value;
					OnPropertyChanged();
				}
			}
		}
		public bool HasDataParentDirectory
		{
			get { return this._hasDataParentDir; }
			set
			{
				if(this._hasDataParentDir != value) {
					this._hasDataParentDir = value;
					OnPropertyChanged();
				}
			}
		}
		public bool HasDataWorkDirectory
		{
			get { return this._hasDataWorkDir; }
			set
			{
				if(this._hasDataWorkDir != value) {
					this._hasDataWorkDir = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion

		#region command

		public ICommand ExecuteCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						NonProcess.Logger.Debug(Model.ToString());
					}
				);

				return result;
			}
		}

		public ICommand OpenDropDownCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Application.Current.Dispatcher.BeginInvoke(new Action(() => {
						if(Model.LauncherKind == LauncherKind.File) {
							// コマンド
							var command = Environment.ExpandEnvironmentVariables(Model.Command ?? string.Empty);
							HasDataCommand = !string.IsNullOrWhiteSpace(command);
							ExistsCommand = HasDataCommand && FileUtility.Exists(command);
							// ディレクトリ
							var parentDir = Path.GetDirectoryName(command);
							HasDataParentDirectory = !string.IsNullOrWhiteSpace(parentDir);
							ExistsParentDirectory = ExistsCommand && HasDataParentDirectory && Directory.Exists(parentDir);
							var workDir = Environment.ExpandEnvironmentVariables(Model.WorkDirectoryPath ?? string.Empty);
							HasDataWorkDirectory = !string.IsNullOrWhiteSpace(workDir);
							ExistsWorkDirectory = HasDataWorkDirectory && Directory.Exists(workDir);
						}
						}));
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

		#endregion
	}
}
