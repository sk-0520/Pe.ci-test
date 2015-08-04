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

	public class CommandViewModel : HavingViewSingleModelWrapperViewModelBase<CommandSettingModel, CommandWindow>, IHavingAppNonProcess
	{
		#region define

		static readonly IReadOnlyList<CommandItemViewModel> emptyCommandList = new List<CommandItemViewModel>();

		#endregion

		#region variable

		double _windowLeft, _windowTop;
		Visibility _visibility = Visibility.Hidden;
		CollectionModel<CommandItemViewModel> _commandItems;
		string _inputText;

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
			set { SetVariableValue(ref this._commandItems, value); }
		}

		public string InputText
		{
			get { return this._inputText; }
			set 
			{
				if (SelectedCommandItem == null) {
					SetVariableValue(ref this._inputText, value.Trim());
					if (!string.IsNullOrEmpty(this._inputText)) {
						var items = GetCommandItems(this._inputText);
						CommandItems = new CollectionModel<CommandItemViewModel>(items);
					}
				} else {
					SetVariableValue(ref this._inputText, SelectedCommandItem.DisplayText);
				}
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

		#endregion

		#region function

		IEnumerable<CommandItemViewModel> GetAllCommandItems()
		{
			return LauncherItemSetting.Items
				.Select(i => new CommandItemViewModel(i, AppNonProcess))
			;
		}

		IEnumerable<CommandItemViewModel> GetCommandItems(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter)) {
				return GetAllCommandItems();
			}
			var items = LauncherItemSetting.Items
				.Where(i => i.Name.StartsWith(filter))
				.Select(i => new CommandItemViewModel(i, AppNonProcess))
			;

			IEnumerable<CommandItemViewModel> tags = null;
			if (Model.FindTag) {
				tags = LauncherItemSetting.Items
					.Where(i => i.Tag.Items.Any(t => t.StartsWith(filter)))
					.Select(i => new CommandItemViewModel(i, i.Tag.Items.First(t => t.StartsWith(filter)), AppNonProcess))
				;
			}
			if (tags == null) {
				tags = emptyCommandList;
			}

			IEnumerable<CommandItemViewModel> files = null;
			if (Model.FindFile) {
				var inputPath = Environment.ExpandEnvironmentVariables(filter);
				var isDir = Directory.Exists(inputPath);
				string baseDir;
				try {
					baseDir = isDir
						? inputPath.Last() == Path.VolumeSeparatorChar
							? inputPath + Path.DirectorySeparatorChar
							: inputPath
						: Path.GetDirectoryName(inputPath)
					;
				} catch (ArgumentException) {
					baseDir = inputPath;
				}
				if (FileUtility.Exists(baseDir)) {
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
					} catch (IOException ex) {
						AppNonProcess.Logger.Warning(ex);
					} catch (UnauthorizedAccessException ex) {
						AppNonProcess.Logger.Warning(ex);
					}
				}
			}
			if (files == null) {
				files = emptyCommandList;
			}

			return items.Concat(tags).Concat(files);
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

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Visibility = Visibility.Hidden;
		}

	}
}
