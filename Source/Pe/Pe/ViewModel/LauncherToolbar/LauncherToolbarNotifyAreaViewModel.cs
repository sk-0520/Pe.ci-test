using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Designer;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar
{
    public class LauncherToolbarNotifyAreaViewModel : SingleModelViewModelBase<LauncherToolbarElement>
    {
        public LauncherToolbarNotifyAreaViewModel(LauncherToolbarElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, IWindowManager windowManager, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherToolbarTheme = launcherToolbarTheme;
            WindowManager = windowManager;

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsVisible), nameof(IsVisible));
        }

        #region property

        ILauncherToolbarTheme LauncherToolbarTheme { get; }
        IWindowManager WindowManager { get; }
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

        #region command

        public ICommand SwitchToolbarCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 var isVisible = IsVisible;
                 Model.ChangeVisible(!isVisible);
                 if(!isVisible) {
                     Model.StartView();
                 } else {
                     var target = WindowManager.GetWindowItems(WindowKind.LauncherToolbar)
                        .First(i => ((LauncherToolbarViewModel)i.ViewModel).DockScreen.DeviceName == Model.DockScreen.DeviceName)
                     ;
                     target.Window.Close();
                 }
             }
         ));
        #endregion

        #region function
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
