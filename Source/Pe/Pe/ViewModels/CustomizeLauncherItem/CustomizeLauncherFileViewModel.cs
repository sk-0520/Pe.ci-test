using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherFileViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        string? _path;
        string? _workingDirectoryPath;
        string? _option;

        bool _isEnabledCustomEnvironmentVariable;
        bool _isEnabledStandardInputOutput;
        bool _runAdministrator;

        #endregion

        public CustomizeLauncherFileViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

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

        #endregion

        #region command

        public ICommand LauncherFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var parameter = new LauncherFileSelectRequestParameter() {
                    FilePath = Environment.ExpandEnvironmentVariables(Path ?? string.Empty),
                    IsFile = true,
                };

                var systemExecutor = new SystemExecutor();
                var exeExts = systemExecutor.GetSystemExecuteExtensions();
                parameter.Filter.Add(new DialogFilterItem("exe", exeExts.First(), exeExts));

                parameter.Filter.Add(new DialogFilterItem("all", string.Empty, "*"));

                FileSelectRequest.Send< LauncherFileSelectRequestResponse>(parameter, r => {
                    if(r.ResponseIsCancel) {
                        return;
                    }

                });
            }
        ));

        public ICommand LauncherDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                FileSelectRequest.Send();
            }
        ));
        public ICommand OptionFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                FileSelectRequest.Send();
            }
        ));

        public ICommand OptionDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                FileSelectRequest.Send();
            }
        ));

        public ICommand WorkingDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 FileSelectRequest.Send();
             }
         ));
        public ICommand WorkingDirectoryClearCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
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
        }

        #endregion
    }
}
