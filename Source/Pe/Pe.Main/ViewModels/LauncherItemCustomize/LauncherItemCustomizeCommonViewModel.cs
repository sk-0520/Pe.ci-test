using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeCommonViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        #endregion

        public LauncherItemCustomizeCommonViewModel(LauncherItemCustomizeEditorElement model, IRequestSender iconSelectRequest, IRequestSender imageSelectRequest, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            IconSelectRequest = iconSelectRequest;
            ImageSelectRequest = imageSelectRequest;
        }

        #region property


        public IRequestSender IconSelectRequest { get; }
        public IRequestSender ImageSelectRequest { get; }


        [Required]
        public string Name
        {
            get => Model.Name;
            set
            {
                SetModelValue(value);
                ValidateProperty(value);
            }
        }

        public string? IconPath
        {
            get => IconData!.ToString();
        }

        public IconData? IconData
        {
            get => Model.IconData;
            private set
            {
                SetModelValue(value);
                RaisePropertyChanged(nameof(IconPath));
            }
        }

        public bool IsEnabledIconSetting => Model.Kind != LauncherItemKind.Separator;

        public bool IsEnabledCommandLauncher
        {
            get => Model.IsEnabledCommandLauncher;
            set => SetModelValue(value);
        }

        public bool IsEnabledCommandLauncherSetting=> Model.Kind != LauncherItemKind.Separator;

        #endregion

        #region command

        private ICommand? _IconFileSelectCommand;
        public ICommand IconFileSelectCommand => this._IconFileSelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectIcon(
                    IconSelectRequest,
                    dialogRequester.ExpandPath(IconData?.Path),
                    IconData?.Index ?? 0,
                    r => {
                        IconData = new IconData() {
                            Path = r.FileName,
                            Index = r.IconIndex,
                        };
                    }
                );

            }
        );

        private ICommand? _IconImageSelectCommand;
        public ICommand IconImageSelectCommand => this._IconImageSelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    ImageSelectRequest,
                    dialogRequester.ExpandPath(IconData?.Path),
                    true,
                    new[] {
                        new DialogFilterItem("image", string.Empty, IconImageLoaderBase.ImageFileExtensions.Select(i => "*." + i)),
                    },
                    r => {
                        IconData = new IconData() {
                            Path = r.ResponseFilePaths[0],
                            Index = 0,
                        };
                    }
                );
            }
        );

        private ICommand? _IconClearSelectCommand;
        public ICommand IconClearSelectCommand => this._IconClearSelectCommand ??= new DelegateCommand(
            () => {
                IconData = new IconData();
            }
        );

        #endregion

        #region function

        public static ValidationResult? ValidateCode(string value, ValidationContext context)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return new ValidationResult(null);
            }

            var codeSymbols = string.Join(string.Empty, LauncherFactory.CodeSymbols.Select(c => new string(c, 1)));
            var pattern = "^[A-Za-z0-9" + Regex.Escape(codeSymbols).Replace("]", @"\]") + "]*$";
            var isMatch = Regex.IsMatch(value, pattern);
            if(!isMatch) {
                return new ValidationResult(null);
            }

            return ValidationResult.Success;
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            //Name = Model.Name;
            //Code = Model.Code;
            //IconData = new IconData(Model.IconData);
            //IsEnabledCommandLauncher = Model.IsEnabledCommandLauncher;
        }

        #endregion
    }
}
