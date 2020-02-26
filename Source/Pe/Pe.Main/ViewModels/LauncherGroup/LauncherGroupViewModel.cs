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
        public LauncherGroupViewModel(LauncherGroupElement model, IDispatcherWrapper dispatcherWrapper, ILauncherGroupTheme launcherGroupTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            LauncherGroupTheme = launcherGroupTheme;
        }

        #region property

        public Guid LauncherGroupId => Model.LauncherGroupId;

        public int RowIndex { get; set; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ILauncherGroupTheme LauncherGroupTheme { get; }
        public string? Name => Model.Name;
        public LauncherGroupImageName ImageName => Model.ImageName;
        public Color ImageColor => Model.ImageColor;

        public DependencyObject NormalGroupIcon => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, false);
        public DependencyObject StrongGroupIcon => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, true);

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase


        #endregion
    }
}
