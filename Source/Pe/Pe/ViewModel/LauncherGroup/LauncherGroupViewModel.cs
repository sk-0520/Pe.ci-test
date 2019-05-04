using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherGroup
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
        public string Name => Model.Name;
        public LauncherGroupImageName ImageName => Model.ImageName;
        public Color ImageColor => Model.ImageColor;

        public DependencyObject NormalGroupIcon => DispatcherWapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconScale.Small, false));
        public DependencyObject StrongGroupIcon => DispatcherWapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconScale.Small, true));

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase


        #endregion
    }
}
