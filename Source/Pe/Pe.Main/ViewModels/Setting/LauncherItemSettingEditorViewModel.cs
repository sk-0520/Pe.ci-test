using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public sealed class LauncherItemSettingEditorViewModel : LauncherItemCustomizeEditorViewModel
    {
        private LauncherItemSettingEditorViewModel(LauncherItemSettingEditorElement model, LauncherIconViewModel icon, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            Editor = model;
            Icon = icon;
        }

        public LauncherItemSettingEditorViewModel(LauncherItemSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            Editor = model;
            Icon = new LauncherIconViewModel(Editor.Icon, dispatcherWrapper, LoggerFactory);
        }

        #region property

        LauncherItemSettingEditorElement Editor { get; }
        public LauncherIconViewModel Icon { get; }
        #endregion

        #region function

        public LauncherItemSettingEditorViewModel Clone()
        {
            return new LauncherItemSettingEditorViewModel(Editor, Icon, DispatcherWrapper, LoggerFactory);
        }

        #endregion
    }
}
