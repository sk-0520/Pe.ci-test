/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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
    using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;
    using System.Windows.Media;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
    using ContentTypeTextNet.Library.PInvoke.Windows;
    using System.Windows.Threading;
    using System.ComponentModel;

    public class CommandViewModel: HasViewSingleModelWrapperViewModelBase<CommandSettingModel, CommandWindow>, IHasAppNonProcess, IHasAppSender, IWindowHitTestData, IVisualStyleData
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

        IEnumerable<CommandItemViewModel> _driveItems;
        //Thickness _resizeThickness = new Thickness(3);

        Brush _borderBrush = SystemColors.ActiveBorderBrush;
        Thickness _borderThickness = SystemParameters.WindowResizeBorderThickness;

        #endregion

        public CommandViewModel(CommandSettingModel model, CommandWindow view, LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view)
        {
            LauncherItemSetting = launcherItemSetting;
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            CommandItems = new CollectionModel<CommandItemViewModel>(GetAllCommandItems());

            HideTimer = new DispatcherTimer();
            HideTimer.Interval = Model.HideTime;
            HideTimer.Tick += HideTimer_Tick;

            if(HasView) {
                BorderBrush = View.Background;

                var backgroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(
                    Window.BackgroundProperty, typeof(Brush)
                );
                if(backgroundPropertyDescriptor != null) {
                    backgroundPropertyDescriptor.AddValueChanged(View, OnBackgroundChanged);
                }
            }
        }

        #region property

        LauncherItemSettingModel LauncherItemSetting { get; set; }

        DispatcherTimer HideTimer { get; set; }

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
            set
            {
                SetVariableValue(ref this._visibility, value);
                if(HasView) {
                    if(Visibility == Visibility.Visible) {
                        InputText = string.Empty;
                        View.Activate();
                        //View.inputCommand.Focus();
                    }
                }
            }
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
                SetVariableValue(ref this._inputText, value.TrimStart());
                var isAll = string.IsNullOrWhiteSpace(InputText);
                var items = isAll
                    ? GetAllCommandItems()
                    : GetCommandItems(InputText)
                ;
                CommandItems = new CollectionModel<CommandItemViewModel>(items);
                if(!isAll && items.Any(i => i.CommandKind == CommandKind.Drive)) {
                    var pair = items
                        .Select((f, i) => new { Item = f, Index = i })
                        .Where(p => p.Item.CommandKind == CommandKind.Drive)
                        .FirstOrDefault(p => p.Item.FilePath.StartsWith(InputText, StringComparison.OrdinalIgnoreCase))
                    ;
                    if(pair != null) {
                        SelectedIndex = pair.Index;
                        //SelectedCommandItem = pair.Item;
                    } else {
                        SelectedIndex = 0;
                    }
                } else {
                    if(this._commandItems != null && this._commandItems.Any()) {
                        SelectedIndex = 0;
                    }
                }

                CallOnPropertyChangeIsOpen();
            }
        }

        public CommandItemViewModel SelectedCommandItem
        {
            get { return this._selectedCommandItem; }
            set
            {
                SetVariableValue(ref this._selectedCommandItem, value);
                CallOnPropertyChange(
                    nameof(IsSelectedCommandItem),
                    nameof(IsNotSelectedCommandItem)
                );
            }
        }

        public bool IsSelectedCommandItem
        {
            get { return SelectedCommandItem != null; }
        }
        public bool IsNotSelectedCommandItem
        {
            get { return !IsSelectedCommandItem; }
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

        public double CaptionWidth { get; set; }
        public double CaptionHeight { get; set; }

        public Brush BorderBrush
        {
            get { return this._borderBrush; }
            set { SetVariableValue(ref this._borderBrush, value); }
        }
        public Thickness BorderThickness { get { return this._borderThickness; } }

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            //set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            //set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            //set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            //set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        #endregion


        #endregion

        #region function

        void CallOnPropertyChangeIsOpen()
        {
            OnPropertyChanged(nameof(IsOpen));
        }

        IEnumerable<CommandItemViewModel> GetAllCommandItems()
        {
            return LauncherItemSetting.Items
                .Select(i => new CommandItemViewModel(Model.IconScale, i, AppNonProcess, AppSender))
            ;
        }

        IEnumerable<CommandItemViewModel> GetCommandItems(string filter)
        {
            if(string.IsNullOrWhiteSpace(filter)) {
                return GetAllCommandItems();
            }
            var items = LauncherItemSetting.Items
                .Where(i => LauncherItemUtility.FilterItemName(i, filter))
                .Select(i => new CommandItemViewModel(Model.IconScale, i, AppNonProcess, AppSender))
            ;

            IEnumerable<CommandItemViewModel> tags = null;
            if(Model.FindTag) {
                tags = LauncherItemSetting.Items
                    .Where(i => i.Tag.Items.Any(t => t.StartsWith(filter)))
                    .Select(i => new CommandItemViewModel(Model.IconScale, i, i.Tag.Items.First(t => t.StartsWith(filter)), AppNonProcess, AppSender))
                ;
            }
            if(tags == null) {
                tags = emptyCommandList;
            }

            IEnumerable<CommandItemViewModel> files = null;
            if(Model.FindFile) {
                var inputPath = Environment.ExpandEnvironmentVariables(filter);
                if(inputPath.Length >= @"C:\".Length) {
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
                        var showHiddenFile = SystemEnvironmentUtility.IsHiddenFileShow();
                        var directoryInfo = new DirectoryInfo(baseDir);
                        try {
                            files = directoryInfo
                                .EnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly)
                                .Where(fs => fs.Exists)
                                .Where(fs => showHiddenFile ? true : !fs.IsHidden())
                                .Select(fs => new CommandItemViewModel(Model.IconScale, fs.FullName, fs.IsDirectory(), fs.IsHidden(), AppNonProcess, AppSender))
                            ;
                            if(isDir) {
                                var parentItem = new CommandItemViewModel(Model.IconScale, inputPath, true, File.GetAttributes(inputPath).HasFlag(FileAttributes.Hidden), AppNonProcess, AppSender);
                                files = new[] { parentItem }.Concat(files);
                            }
                        } catch(IOException ex) {
                            AppNonProcess.Logger.Warning(ex);
                        } catch(UnauthorizedAccessException ex) {
                            AppNonProcess.Logger.Warning(ex);
                        }
                    }
                } else if(inputPath.Length == @"C:".Length && char.IsLetter(inputPath[0]) && inputPath[1] == Path.VolumeSeparatorChar) {
                    if(this._driveItems == null) {
                        try {
                            this._driveItems = DriveInfo.GetDrives()
                                .Where(d => d.IsReady)
                                .Select(d => new CommandItemViewModel(Model.IconScale, d.RootDirectory.FullName, d.VolumeLabel, AppNonProcess, AppSender))
                            ;
                        } catch(IOException ex) {
                            AppNonProcess.Logger.Warning(ex);
                        } catch(UnauthorizedAccessException ex) {
                            AppNonProcess.Logger.Warning(ex);
                        }
                    }

                    files = this._driveItems;
                }
            }
            if(files == null) {
                files = emptyCommandList;
            }

            return items.Concat(tags).Concat(files);
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

        void RunItem(CommandItemViewModel commandItem, bool extension)
        {
            CheckUtility.EnforceNotNull(commandItem);

            AppNonProcess.Logger.Information(SelectedCommandItem.ToString());

            switch(commandItem.CommandKind) {
                case CommandKind.File:
                case CommandKind.Drive:
                    try {
                        if(commandItem.CommandKind == CommandKind.File && extension) {
                            ExecuteUtility.OpenDirectoryWithFileSelect(commandItem.FilePath, AppNonProcess, default(LauncherItemModel));
                        } else {
                            ExecuteUtility.OpenFile(commandItem.FilePath, AppNonProcess);
                        }
                    } catch(Exception ex) {
                        AppNonProcess.Logger.Warning(ex);
                    }
                    break;

                case CommandKind.LauncherItemName:
                case CommandKind.LauncherItemTag:
                    {
                        if(extension) {
                            ScreenModel screen = null;
                            if(HasView) {
                                screen = Screen.FromHandle(View.Handle);
                            }
                            var data = new LauncherItemWithScreen(commandItem.LauncherItemModel, screen, null);
                            var window = AppSender.SendCreateWindow(WindowKind.LauncherExecute, data, null);
                            window.Show();
                        } else {
                            var viewModel = new LauncherItemSimpleViewModel(commandItem.LauncherItemModel, AppNonProcess, AppSender);
                            viewModel.Execute(null);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
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

        public ICommand RunItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(SelectedCommandItem != null) {
                            var showExtension = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
                            RunItem(SelectedCommandItem, showExtension);
                            Visibility = Visibility.Hidden;
                        }
                    }
                );

                return result;
            }
        }

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

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region HasViewSingleModelWrapperIndexViewModelBase

        protected override void InitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing += View_UserClosing;
            View.Activated += View_Activated;
            View.Deactivated += View_Deactivated;
            PopupUtility.Attachment(View, View.popup);
            View.inputCommand.KeyDown += inputCommand_KeyDown;
            View.listItems.SelectionChanged += listItems_SelectionChanged;

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing -= View_UserClosing;
            View.Activated -= View_Activated;
            View.Deactivated -= View_Deactivated;
            View.inputCommand.KeyDown -= inputCommand_KeyDown;
            View.listItems.SelectionChanged -= listItems_SelectionChanged;

            base.UninitializeView();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(HideTimer != null) {
                    HideTimer.Stop();
                    HideTimer = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IWindowHitTestData

        /// <summary>
        /// ボーダーに対するヒットテストを行うか
        /// </summary>
        public bool UsingBorderHitTest { get { return true; } }
        /// <summary>
        /// タイトルバーに対するヒットテストを行うか
        /// </summary>
        public bool UsingCaptionHitTest { get { return true; } }

        /// <summary>
        /// タイトルバーとして認識される領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect CaptionArea
        {
            get
            {
                var result = new Rect(
                    BorderThickness.Left, BorderThickness.Top,
                    View.caption.ActualWidth, View.caption.ActualHeight
                );
                return result;
            }
        }
        /// <summary>
        /// サイズ変更に使用する境界線。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness ResizeThickness { get { return BorderThickness; } }


        #endregion

        #region IVisualStyleData

        public bool UsingVisualStyle { get { return true; } }
        public bool EnabledVisualStyle { get; set; }
        public Color VisualPlainColor { get; set; }
        public Color VisualAlphaColor { get; set; }

        #endregion

        private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HideTimer.Stop();
            e.Cancel = true;
            Visibility = Visibility.Hidden;
            InputText = string.Empty;
        }

        void View_Activated(object sender, EventArgs e)
        {
            AppNonProcess.Logger.Debug("command window: active");
            CallOnPropertyChangeIsOpen();
            HideTimer.IsEnabled = false;
            if(HasView) {
                View.inputCommand.Focus();
            }
        }

        void View_Deactivated(object sender, EventArgs e)
        {
            CallOnPropertyChangeIsOpen();
            HideTimer.IsEnabled = true;
        }

        void inputCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Oem5 || e.Key == Key.OemBackslash) {
                if(SelectedCommandItem != null && (SelectedCommandItem.CommandKind == CommandKind.File || SelectedCommandItem.CommandKind == CommandKind.Drive)) {
                    var textBox = (TextBox)sender;
                    InputText = SelectedCommandItem.FilePath;
                    textBox.Select(InputText.Length, 0);
                    e.Handled = true;
                }
            }
        }

        void HideTimer_Tick(object sender, EventArgs e)
        {
            //HideTimer.IsEnabled = false;
            AppNonProcess.Logger.Debug("command window: hide");
            if(HasView) {
                View.UserClose();
            }
        }

        /// <summary>
        /// <para>http://stackoverflow.com/questions/8827489/scroll-wpf-listbox-to-the-selecteditem-set-in-code-in-a-view-model?answertab=votes#tab-top</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            listBox.Dispatcher.BeginInvoke((Action)(() => {
                listBox.UpdateLayout();
                if(listBox.SelectedItem != null) {
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }
            }));
        }

        void OnBackgroundChanged(object sender, EventArgs e)
        {
            var viewBrush = View.Background as SolidColorBrush;
            if(viewBrush != null) {
                BorderBrush = viewBrush;
            }
        }

    }
}
