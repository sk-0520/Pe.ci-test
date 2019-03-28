using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Designer;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar
{
    public class LauncherToolbarNotifyAreaViewModel : SingleModelViewModelBase<LauncherToolbarElement>
    {
        public LauncherToolbarNotifyAreaViewModel(LauncherToolbarElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherToolbarTheme = launcherToolbarTheme;

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsVisible), nameof(IsVisible));
        }

        #region property

        ILauncherToolbarTheme LauncherToolbarTheme { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        public DependencyObject ToolbarIcon => LauncherToolbarTheme.CreateToolbarImage(Model.DockScreen, Screen.AllScreens, IconScale.Small);
        public string DisplayName
        {
            get
            {
                var screenOperator = new ScreenOperator(Logger.Factory);
                return screenOperator.GetName(Model.DockScreen);
            }
        }
        public bool IsVisible => Model.IsVisible;

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
