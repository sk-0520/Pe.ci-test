using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeFileViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        //string? _path;
        //string? _workingDirectoryPath;
        //string? _option;

        //bool _isEnabledCustomEnvironmentVariable;
        //bool _isEnabledStandardInputOutput;
        //Encoding? _standardInputOutputEncoding;
        //bool _runAdministrator;
        private bool _isDropDownPathItems;
        private EncodingViewModel? _selectedStandardInputOutputEncoding;

        #endregion

        public LauncherItemCustomizeFileViewModel(LauncherItemCustomizeEditorElement model, IRequestSender fileSelectRequest, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            if(Model.Kind != Models.Data.LauncherItemKind.File) {
                throw new ArgumentException(null, nameof(model) + "." + nameof(Model.Kind));
            }
            if(!Model.IsLazyLoad) {
                if(Model.File == null) {
                    throw new ArgumentNullException(nameof(model) + "." + nameof(Model.File));
                }
            }

            FileSelectRequest = fileSelectRequest;
        }

        #region property

        private LauncherFileData File => Model.File!;

        public bool IsDropDownPathItems
        {
            get => this._isDropDownPathItems;
            set => SetProperty(ref this._isDropDownPathItems, value);
        }

        private static EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        public IRequestSender FileSelectRequest { get; }

        public string Path
        {
            get => File.Path;
            set => SetPropertyValue(File, value);
        }
        public string WorkingDirectoryPath
        {
            get => File.WorkDirectoryPath;
            set => SetPropertyValue(File, value, nameof(File.WorkDirectoryPath));
        }
        public string? Option
        {
            get => File.Option;
            set => SetPropertyValue(File, value);
        }

        public ShowMode ShowMode
        {
            get => File.ShowMode;
            set => SetPropertyValue(File, value);
        }

        public bool IsEnabledCustomEnvironmentVariable
        {
            get => File.IsEnabledCustomEnvironmentVariable;
            set => SetPropertyValue(File, value);
        }
        public bool IsEnabledStandardInputOutput
        {
            get => File.IsEnabledStandardInputOutput;
            set => SetPropertyValue(File, value);
        }
        public Encoding StandardInputOutputEncoding
        {
            get => File.StandardInputOutputEncoding;
            set => SetPropertyValue(File, value);
        }

        #region View側未実装エンコーディング
        // 追加でとってきた一覧取得ができない悲しみ
        public ObservableCollection<EncodingViewModel> EncodingItems { get; } = new ObservableCollection<EncodingViewModel>();
        public EncodingViewModel? SelectedStandardInputOutputEncoding
        {
            get => this._selectedStandardInputOutputEncoding;
            set
            {
                SetProperty(ref this._selectedStandardInputOutputEncoding, value);
                if(this._selectedStandardInputOutputEncoding != null) {
                    StandardInputOutputEncoding = Encoding.GetEncoding(this._selectedStandardInputOutputEncoding.CodePage);
                }
            }
        }
        #endregion

        public bool RunAdministrator
        {
            get => File.RunAdministrator;
            set => SetPropertyValue(File, value);
        }

        public ObservableCollection<EnvironmentPathExecuteItemViewModel> PathItems { get; } = new ObservableCollection<EnvironmentPathExecuteItemViewModel>();

        #endregion

        #region command

        private ICommand? _LauncherFileSelectCommand;
        public ICommand LauncherFileSelectCommand => this._LauncherFileSelectCommand ??= new DelegateCommand(
            () => {
                var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
                var exeExtensions = environmentExecuteFile.GetSystemExecuteExtensions(true);

                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Path),
                    true,
                    new[] {
                        new DialogFilterItem("exe", exeExtensions.First(), exeExtensions),
                        dialogRequester.CreateAllFilter(),
                    },
                    r => {
                        Path = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _LauncherDirectorySelectCommand;
        public ICommand LauncherDirectorySelectCommand => this._LauncherDirectorySelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Path),
                    r => {
                        Path = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _SetPathToFullPathCommand;
        public ICommand SetPathToFullPathCommand => this._SetPathToFullPathCommand ??= new DelegateCommand<EnvironmentPathExecuteItemViewModel>(
            o => {
                Path = o.FullPath;
                IsDropDownPathItems = false;
            }
        );

        private ICommand? _OptionFileSelectCommand;
        public ICommand OptionFileSelectCommand => this._OptionFileSelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Option),
                    true,
                    new[] { dialogRequester.CreateAllFilter() },
                    r => {
                        Option = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _OptionDirectorySelectCommand;
        public ICommand OptionDirectorySelectCommand => this._OptionDirectorySelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Option),
                    r => {
                        Option = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _OptionClearCommand;
        public ICommand OptionClearCommand => this._OptionClearCommand ??= new DelegateCommand(
            () => {
                Option = string.Empty;
            }
        );

        private ICommand? _WorkingDirectorySelectCommand;
        public ICommand WorkingDirectorySelectCommand => this._WorkingDirectorySelectCommand ??= new DelegateCommand(
             () => {
                 var dialogRequester = new DialogRequester(LoggerFactory);
                 dialogRequester.SelectDirectory(
                     FileSelectRequest,
                     dialogRequester.ExpandPath(WorkingDirectoryPath),
                     r => {
                         WorkingDirectoryPath = r.ResponseFilePaths[0];
                     }
                 );
             }
         );

        private ICommand? _WorkingDirectoryClearCommand;
        public ICommand WorkingDirectoryClearCommand => this._WorkingDirectoryClearCommand ??= new DelegateCommand(
             () => {
                 WorkingDirectoryPath = string.Empty;
             }
         );

        #endregion

        #region function


        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            //var data = Model.LoadFileData();
            //Path = data.Path;
            //WorkingDirectoryPath = data.WorkDirectoryPath;
            //Option = data.Option;
            //IsEnabledCustomEnvironmentVariable = data.IsEnabledCustomEnvironmentVariable;
            //IsEnabledStandardInputOutput = data.IsEnabledStandardInputOutput;
            //StandardInputOutputEncoding = data.StandardInputOutputEncoding;
            //RunAdministrator = data.RunAdministrator;

            if(Model.IsLazyLoad) {
                return;
            }

            var pathItems = EnvironmentPathExecuteFileCache.GetItems(LoggerFactory);
            PathItems.SetRange(pathItems.Select(i => new EnvironmentPathExecuteItemViewModel(i, LoggerFactory)));

            var encItems = Encoding.GetEncodings().Select(i => new EncodingViewModel(i, LoggerFactory));
            EncodingItems.SetRange(encItems);
            this._selectedStandardInputOutputEncoding = EncodingItems.FirstOrDefault(i => i.CodePage == StandardInputOutputEncoding.CodePage) ?? new EncodingViewModel(StandardInputOutputEncoding, LoggerFactory);
        }

        #endregion
    }
}
