using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Main.Models;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{

    public class LauncherItemCustomizeFileViewModel : LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        string? _path;
        string? _workingDirectoryPath;
        string? _option;

        bool _isEnabledCustomEnvironmentVariable;
        bool _isEnabledStandardInputOutput;
        Encoding? _standardInputOutputEncoding;
        bool _runAdministrator;

        EncodingViewModel? _selectedStandardInputOutputEncoding;

        #endregion

        public LauncherItemCustomizeFileViewModel(LauncherItemCustomizeElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        private static EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        public RequestSender FileSelectRequest { get; } = new RequestSender();

        public string? Path
        {
            get => this._path;
            set => SetProperty(ref this._path, value);
        }
        public string? WorkingDirectoryPath
        {
            get => this._workingDirectoryPath;
            set => SetProperty(ref this._workingDirectoryPath, value);
        }
        public string? Option
        {
            get => this._option;
            set => SetProperty(ref this._option, value);
        }

        public bool IsEnabledCustomEnvironmentVariable
        {
            get => this._isEnabledCustomEnvironmentVariable;
            set => SetProperty(ref this._isEnabledCustomEnvironmentVariable, value);
        }
        public bool IsEnabledStandardInputOutput
        {
            get => this._isEnabledStandardInputOutput;
            set => SetProperty(ref this._isEnabledStandardInputOutput, value);
        }
        public Encoding? StandardInputOutputEncoding
        {
            get => this._standardInputOutputEncoding;
            set => SetProperty(ref this._standardInputOutputEncoding, value);
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
            get => this._runAdministrator;
            set => SetProperty(ref this._runAdministrator, value);
        }

        public ObservableCollection<EnvironmentPathExecuteItemViewModel> PathItems { get; } = new ObservableCollection<EnvironmentPathExecuteItemViewModel>();

        #endregion

        #region command

        public ICommand LauncherFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
                var exeExts = environmentExecuteFile.GetSystemExecuteExtensions(true);

                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Path),
                    true,
                    new[] {
                        new DialogFilterItem("exe", exeExts.First(), exeExts),
                        dialogRequester.CreateAllFilter(),
                    },
                    r => {
                        Path = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        public ICommand LauncherDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
        ));

        public ICommand OptionFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
        ));

        public ICommand OptionDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
        ));

        public ICommand OptionClearCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Option = string.Empty;
            }
        ));

        public ICommand WorkingDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
         ));

        public ICommand WorkingDirectoryClearCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 WorkingDirectoryPath = string.Empty;
             }
         ));

        #endregion

        #region function


        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            var data = Model.LoadFileData();
            Path = data.Path;
            WorkingDirectoryPath = data.WorkDirectoryPath;
            Option = data.Option;
            IsEnabledCustomEnvironmentVariable = data.IsEnabledCustomEnvironmentVariable;
            IsEnabledStandardInputOutput = data.IsEnabledStandardInputOutput;
            StandardInputOutputEncoding = data.StandardInputOutputEncoding;
            RunAdministrator = data.RunAdministrator;

            var pathItems = EnvironmentPathExecuteFileCache.GetItems(LoggerFactory);
            PathItems.SetRange(pathItems.Select(i => new EnvironmentPathExecuteItemViewModel(i, LoggerFactory)));

            var encItems = Encoding.GetEncodings().Select(i => new EncodingViewModel(i, LoggerFactory));
            EncodingItems.SetRange(encItems);
            SelectedStandardInputOutputEncoding = EncodingItems.FirstOrDefault(i => i.CodePage == StandardInputOutputEncoding.CodePage) ?? new EncodingViewModel(StandardInputOutputEncoding, LoggerFactory);
        }

        #endregion
    }
}
