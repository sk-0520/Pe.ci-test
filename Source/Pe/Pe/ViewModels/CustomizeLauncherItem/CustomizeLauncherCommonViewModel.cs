using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherCommonViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        string? _name;
        string? _code;

        string? _iconPath;
        bool _isEnabledCommandLauncher;
        #endregion

        public CustomizeLauncherCommonViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Guid LauncherItemId => Model.LauncherItemId;
        public LauncherItemKind Kind => Model.Kind;
        public IconData? IconData => Model.IconData;

        [Required]
        public string? Name
        {
            get => this._name;
            set
            {
                SetProperty(ref this._name, value);
                ValidateProperty(value);
            }
        }

        [Required]
        [CustomValidation(typeof(CustomizeLauncherCommonViewModel), nameof(CustomizeLauncherCommonViewModel.ValidateCode))]
        public string? Code
        {
            get => this._code;
            set
            {
                SetProperty(ref this._code, value);
                ValidateProperty(value);
            }
        }
        public string? IconPath
        {
            get => this._iconPath;
            set => SetProperty(ref this._iconPath, value);
        }

        public bool IsEnabledCommandLauncher
        {
            get => this._isEnabledCommandLauncher;
            set => SetProperty(ref this._isEnabledCommandLauncher, value);
        }

        #endregion

        #region command
        #endregion

        #region function

        public static ValidationResult ValidateCode(string value, ValidationContext context)
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
            Name = Model.Name;
            Code = Model.Code;
            IconPath = IconData?.ToString();
            IsEnabledCommandLauncher = Model.IsEnabledCommandLauncher;
        }

        #endregion
    }
}
