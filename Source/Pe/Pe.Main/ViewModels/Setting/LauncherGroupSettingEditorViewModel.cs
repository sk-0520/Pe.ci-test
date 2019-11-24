using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupElement>
    {
        #region variable

        string _name;
        Color _imageColor;
        LauncherGroupImageName _imageName;
        long _sequence;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }


            DispatcherWrapper = dispatcherWrapper;

            this._name = Model.Name;
            this._imageColor = Model.ImageColor;
            this._imageName = Model.ImageName;
            Kind = Model.Kind;
        }

        #region property

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
            set => SetProperty(ref this._imageColor, value);
        }

        public LauncherGroupImageName ImageName
        {
            get => this._imageName;
            set => SetProperty(ref this._imageName, value);
        }

        public long Sequence
        {
            get => this._sequence;
            private set => SetProperty(ref this._sequence, value);
        }

        public LauncherGroupKind Kind { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

    }
}
