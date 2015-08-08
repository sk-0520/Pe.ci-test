namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;
	using System.Diagnostics;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using System.IO;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using System.Windows.Input;
	using System.Windows.Controls;

	public class CommandViewModel: HavingViewSingleModelWrapperViewModelBase<CommandSettingModel, CommandWindow>, IHavingAppNonProcess
	{
		#region define

		static readonly IReadOnlyList<CommandItemViewModel> emptyCommandList = new List<CommandItemViewModel>();

		#endregion

		#region variable

		double _windowLeft, _windowTop;
		Visibility _visibility = Visibility.Hidden;
		CollectionModel<CommandItemViewModel> _commandItems;
		string _inputText, _selectedText;
		int _selectedIndex;

		CommandItemViewModel _selectedCommandItem;

		#endregion

		public CommandViewModel(CommandSettingModel model, CommandWindow view, LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess)
			: base(model, view)
		{
			LauncherItemSetting = launcherItemSetting;
			AppNonProcess = appNonProcess;

			CommandItems = new CollectionModel<CommandItemViewModel>(GetAllCommandItems());
		}

		#region property

		LauncherItemSettingModel LauncherItemSetting { get; set; }

		public double WindowLeft
		{
			get { return this._windowLeft; }
			set { SetVariableValue(ref this._windowLeft, value); }
		}

		public double WindowTop
		{
			get { return this._windowTop; }
			set { SetVariableValue(ref this._windowTop, value); }
		}

		public double WindowWidth
		{
			get { return Model.WindowWidth; }
			set { SetModelValue(value); }
		}

		public Visibility Visibility
		{
			get { return this._visibility; }
			set { SetVariableValue(ref this._visibility, value); }
		}

		public double IconWidth { get { return Model.IconScale.ToWidth(); } }
		public double IconHeight { get { return Model.IconScale.ToHeight(); } }

		public CollectionModel<CommandItemViewModel> CommandItems
		{
			get { return this._commandItems; }
			set
			{
				SetVariableValue(ref this._commandItems, value);
				if(this._commandItems != null && this._commandItems.Any()) {
					SelectedIndex = 0;
				}
				OnPropertyChangeIsOpen();
			}
		}

		public string InputText
		{
			get { return this._inputText; }
			set
			{
				SetVariableValue(ref this._inputText, value.Trim());
				var items = string.IsNullOrWhiteSpace(InputText)
					? GetAllCommandItems()
					: GetCommandItems(InputText)
				;
				CommandItems = new CollectionModel<CommandItemViewModel>(items);

				OnPropertyChangeIsOpen();
			}
		}

		public CommandItemViewModel SelectedCommandItem
		{
			get { return this._selectedCommandItem; }
			set
			{
				SetVariableValue(ref this._selectedCommandItem, value);
				//if (this._selectedCommandItem == null) {
				//	var items = GetAllCommandItems();
				//	CommandItems = new CollectionModel<CommandItemViewModel>(items);
				//}
			}
		}

		public int SelectedIndex
		{
			get { return this._selectedIndex; }
			set { SetVariableValue(ref this._selectedIndex, value); }
		}

		public string SelectedText
		{
			get { return this._selectedText; }
			set { SetVariableValue(ref this._selectedText, value); }
		}

		public bool IsOpen
		{
			get
			{
				var result = CommandItems.Any();

				if(HasView) {
					result &= View.IsActive;
				}
				return result;
			}
		}

		#endregion

		#region function

		void OnPropertyChangeIsOpen()
		{
			OnPropertyChanged("IsOpen");
		}

		IEnumerable<CommandItemViewModel> GetAllCommandItems()
		{
			return LauncherItemSetting.Items
				.Select(i => new CommandItemViewModel(i, AppNonProcess))
			;
		}

		IEnumerable<CommandItemViewModel> GetCommandItems(string filter)
		{
			if(string.IsNullOrWhiteSpace(filter)) {
				return GetAllCommandItems();
			}
			var items = LauncherItemSetting.Items
				.Where(i => i.Name.StartsWith(filter))
				.Select(i => new CommandItemViewModel(i, AppNonProcess))
			;

			IEnumerable<CommandItemViewModel> tags = null;
			if(Model.FindTag) {
				tags = LauncherItemSetting.Items
					.Where(i => i.Tag.Items.Any(t => t.StartsWith(filter)))
					.Select(i => new CommandItemViewModel(i, i.Tag.Items.First(t => t.StartsWith(filter)), AppNonProcess))
				;
			}
			if(tags == null) {
				tags = emptyCommandList;
			}

			IEnumerable<CommandItemViewModel> files = null;
			if(Model.FindFile) {
				var inputPath = Environment.ExpandEnvironmentVariables(filter);
				if(inputPath.Length > @"C:".Length) {
					var isDir = Directory.Exists(inputPath);
					string baseDir;
					try {
						baseDir = isDir
							? inputPath.Last() == Path.VolumeSeparatorChar
								? inputPath + Path.DirectorySeparatorChar
								: inputPath
							: Path.GetDirectoryName(inputPath)
						;
					} catch(ArgumentException) {
						baseDir = inputPath;
					}
					if(FileUtility.Exists(baseDir)) {
						Debug.WriteLine(inputPath);
						//var isDir = Directory.Exists(inputPath);
						//var baseDir = isDir ? inputPath : Path.GetDirectoryName(inputPath);
						var searchPattern = isDir ? "*" : Path.GetFileName(inputPath) + "*";
						var showHiddenFile = SystemEnvironmentUtility.IsHideFileShow();
						var directoryInfo = new DirectoryInfo(baseDir);
						try {
							files = directoryInfo
								.EnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly)
								.Where(fs => fs.Exists)
								.Where(fs => showHiddenFile ? true : !fs.IsHidden())
								.Select(fs => new CommandItemViewModel(fs.FullName, AppNonProcess))
							;
						} catch(IOException ex) {
							AppNonProcess.Logger.Warning(ex);
						} catch(UnauthorizedAccessException ex) {
							AppNonProcess.Logger.Warning(ex);
						}
					}
				}
			}
			if(files == null) {
				files = emptyCommandList;
			}

			return items.Concat(tags).Concat(files);
		}

		#endregion

		#region command

		public ICommand UpListCommand
		{
			get
			{
				var result = CreateCommand(
					o => { ChangeSelectedItemFromList(true); }
				);

				return result;
			}
		}

		public ICommand DownListCommand
		{
			get
			{
				var result = CreateCommand(
					o => { ChangeSelectedItemFromList(false); }
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
						if(SelectedCommandItem != null) {
							AppNonProcess.Logger.Information(SelectedCommandItem.ToString());
						}
					}
				);

				return result;
			}
		}

		//public ICommand InputDirectorySeparator
		//{
		//	get
		//	{
		//		var result = CreateCommand(
		//			o => {
		//				if(SelectedCommandItem != null && SelectedCommandItem.CommandKind == CommandKind.File) {
		//					if(InputText.LastOrDefault() != Path.DirectorySeparatorChar) {
		//						InputText = SelectedCommandItem.FilePath;
		//					}
		//				}
		//			}
		//		);

		//		return result;
		//	}
		//}

		public ICommand FileFindCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(SelectedCommandItem != null && SelectedCommandItem.CommandKind == CommandKind.File) {
							InputText = SelectedCommandItem.FilePath;
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region HavingViewSingleModelWrapperIndexViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing += View_UserClosing;
			View.Activated += View_Activated;
			View.Deactivated += View_Deactivated;
			PopupUtility.Attachment(View, View.popup);
			View.inputCommand.KeyDown += inputCommand_KeyDown;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;
			View.Activated -= View_Activated;
			View.Deactivated -= View_Deactivated;
			View.inputCommand.KeyDown -= inputCommand_KeyDown;

			base.UninitializeView();
		}

		void ChangeSelectedItemFromList(bool isUp)
		{
			if(isUp) {
				if(SelectedIndex == 0) {
					SelectedIndex = CommandItems.Count - 1;
				} else {
					SelectedIndex = SelectedIndex - 1;
				}
			} else {
				if(SelectedIndex >= CommandItems.Count - 1) {
					SelectedIndex = 0;
				} else {
					SelectedIndex = SelectedIndex + 1;
				}
			}
		}

		#endregion

		private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Visibility = Visibility.Hidden;
		}

		void View_Activated(object sender, EventArgs e)
		{
			OnPropertyChangeIsOpen();
			if(HasView) {
				View.inputCommand.Focus();
			}
		}

		void View_Deactivated(object sender, EventArgs e)
		{
			OnPropertyChangeIsOpen();
		}

		void inputCommand_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Oem5 || e.Key == Key.OemBackslash) {
				if(SelectedCommandItem != null && SelectedCommandItem.CommandKind == CommandKind.File) {
					var textBox = (TextBox)sender;
					InputText = SelectedCommandItem.FilePath;
					textBox.Select(InputText.Length, 0);
					e.Handled = true;
				}
			}
		}
	}
}
