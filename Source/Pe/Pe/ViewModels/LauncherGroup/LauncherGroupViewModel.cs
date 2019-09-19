using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup
{
    public class LauncherGroupViewModel : SingleModelViewModelBase<LauncherGroupElement>
    {
        public LauncherGroupViewModel(LauncherGroupElement model, IDispatcherWapper dispatcherWapper, ILauncherGroupTheme launcherGroupTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            LauncherGroupTheme = launcherGroupTheme;
        }

        #region property

        public int RowIndex { get; set; }
        IDispatcherWapper DispatcherWapper { get; }
        ILauncherGroupTheme LauncherGroupTheme { get; }
        public string? Name => Model.Name;
        public LauncherGroupImageName ImageName => Model.ImageName;
        public Color ImageColor => Model.ImageColor;

        public DependencyObject NormalGroupIcon => DispatcherWapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, new IconSize(IconSize.Kind.Small), false));
        public DependencyObject StrongGroupIcon => DispatcherWapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, new IconSize(IconSize.Kind.Small), true));

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase


        #endregion
    }
}
