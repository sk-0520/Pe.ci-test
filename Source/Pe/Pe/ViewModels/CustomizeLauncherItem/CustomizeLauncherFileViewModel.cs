using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using Microsoft.Extensions.Logging;

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
