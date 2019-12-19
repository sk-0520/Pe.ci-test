using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToobarSettingEditorViewModel : SettingItemViewModelBase<LauncherToobarSettingEditorElement>
    {
        public LauncherToobarSettingEditorViewModel(LauncherToobarSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property


        public FontViewModel? Font { get; private set; }
        public Guid LauncherGroupId { get; private set; }
        public AppDesktopToolbarPosition ToolbarPosition
        {
            get => Model.ToolbarPosition;
            set => SetModelValue(value);
        }
        public LauncherToolbarIconDirection IconDirection
        {
            get => Model.IconDirection;
            set => SetModelValue(value);
        }

        public IconBox IconBox
        {
            get => Model.IconBox;
            set => SetModelValue(value);
        }

        //public TimeSpan AutoHideTimeout
        //{
        //    get => Model.AutoHideTimeout;
        //    set => SetModelValue(value);
        //}

        public double AutoHideTimeSeconds
        {
            get => Model.AutoHideTimeout.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value));
        }
        public double MaximumAutoHideTimeSeconds => 10;
        public double MinimumAutoHideTimeSeconds => 0.5;

        public int TextWidth
        {
            get => Model.TextWidth;
            set => SetModelValue(value);
        }
        public double MaximumTextWidth => 200;
        public double MinimumTextWidth => 40;

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
            set => SetModelValue(value);
        }

        public bool IsAutoHide
        {
            get => Model.IsAutoHide;
            set => SetModelValue(value);
        }

        public bool IsIconOnly
        {
            get => Model.IsIconOnly;
            set => SetModelValue(value);
        }

        #endregion

        #region function

        #endregion

        #region command

        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
