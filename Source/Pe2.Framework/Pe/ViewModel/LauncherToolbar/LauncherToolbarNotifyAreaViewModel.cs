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
using ContentTypeTextNet.Pe.Main.Model.Theme;
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
            DispatcherWapper = dispatcherWapper;
            WindowManager = windowManager;

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsVisible), new[] { nameof(MenuIsChecked), nameof(MenuIcon) });
        }

        #region property

        ILauncherToolbarTheme LauncherToolbarTheme { get; }
        IWindowManager WindowManager { get; }
        IDispatcherWapper DispatcherWapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }


        #endregion

        #region command

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

        #region INotifyArea

        public string MenuHeader
        {
            get
            {
                var screenOperator = new ScreenOperator(Logger.Factory);
                return screenOperator.GetName(Model.DockScreen);
            }
        }
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture MenuKeyGesture { get; }
        public DependencyObject MenuIcon => DispatcherWapper.Get(() => LauncherToolbarTheme.GetToolbarImage(Model.DockScreen, Screen.AllScreens, IconScale.Small, MenuIsChecked));
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked => Model.IsVisible;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 var isVisible = Model.IsVisible;
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

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
