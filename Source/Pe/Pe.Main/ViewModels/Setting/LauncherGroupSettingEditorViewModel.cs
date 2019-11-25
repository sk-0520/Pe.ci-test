using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupElement>, ILauncherGroupId
    {
        #region variable

        string _name;
        Color _imageColor;
        LauncherGroupImageName _imageName;
        long _sequence;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupElement model, ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> launcherItems, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            LauncherGroupTheme = launcherGroupTheme;
            DispatcherWrapper = dispatcherWrapper;
            LauncherItems = launcherItems;

            this._name = Model.Name;
            this._imageColor = Model.ImageColor;
            this._imageName = Model.ImageName;
            Kind = Model.Kind;
        }

        #region property

        public ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }

        ILauncherGroupTheme LauncherGroupTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        [Required]
        public string Name
        {
            get => this._name;
            set
            {
                SetProperty(ref this._name, value);
                ValidateProperty(value);
            }
        }

        public Color ImageColor
        {
            get => this._imageColor;
            set
            {
                SetProperty(ref this._imageColor, value);
                ReloadIcon();
            }
        }

        public LauncherGroupImageName ImageName
        {
            get => this._imageName;
            set
            {
                SetProperty(ref this._imageName, value);
                ReloadIcon();
            }
        }

        public long Sequence
        {
            get => this._sequence;
            set => SetProperty(ref this._sequence, value);
        }

        public LauncherGroupKind Kind { get; }

        public object GroupIcon => DispatcherWrapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, false));

        #endregion

        #region command
        #endregion

        #region function

        void ReloadIcon()
        {
            RaisePropertyChanged(nameof(GroupIcon));
        }

        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId => Model.LauncherGroupId;

        #endregion

    }
}
