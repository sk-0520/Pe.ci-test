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
        bool _runAdministrator;

        #endregion

        public LauncherItemCustomizeFileViewModel(LauncherItemCustomizeElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(PathExecuteFileCache == null) {
                PathExecuteFileCache = new PathExecuteFileCache(TimeSpan.FromHours(3), LoggerFactory);
            }
        }

        #region property

        private static PathExecuteFileCache PathExecuteFileCache { get; set; } = null!;

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
        public bool RunAdministrator
        {
            get => this._runAdministrator;
            set => SetProperty(ref this._runAdministrator, value);
        }

        public ObservableCollection<SystemExecuteItemViewModel> PathItems { get; } = new ObservableCollection<SystemExecuteItemViewModel>();

        #endregion

        #region command

        public ICommand LauncherFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var systemExecutor = new SystemExecutor(LoggerFactory);
                var exeExts = systemExecutor.GetSystemExecuteExtensions();

                SelectFile(
                    FileSelectRequest,
                    ExpandPath(Path),
                    true,
                    new[] {
                        new DialogFilterItem("exe", exeExts.First(), exeExts),
                        CreateAllFilter(),
                    },
                    r => {
                        Path = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        public ICommand LauncherDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                SelectFile(
                    FileSelectRequest,
                    ExpandPath(Path),
                    false,
                    Enumerable.Empty<DialogFilterItem>(),
                    r => {
                        Path = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        public ICommand OptionFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                SelectFile(
                    FileSelectRequest,
                    ExpandPath(Option),
                    true,
                    new[] { CreateAllFilter() },
                    r => {
                        Option = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        public ICommand OptionDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                SelectFile(
                    FileSelectRequest,
                    ExpandPath(Option),
                    false,
                    Enumerable.Empty<DialogFilterItem>(),
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
                 SelectFile(
                     FileSelectRequest,
                     ExpandPath(WorkingDirectoryPath),
                     false,
                     Enumerable.Empty<DialogFilterItem>(),
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
            RunAdministrator = data.RunAdministrator;

            var pathItems = PathExecuteFileCache.GetItems();
            PathItems.SetRange(pathItems.Select(i => new SystemExecuteItemViewModel(i, LoggerFactory)));
        }

        #endregion
    }
}
