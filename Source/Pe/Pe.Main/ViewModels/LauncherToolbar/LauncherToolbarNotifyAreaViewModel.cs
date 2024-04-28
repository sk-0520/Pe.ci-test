using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherToolbarNotifyAreaViewModel: SingleModelViewModelBase<LauncherToolbarElement>
    {
        public LauncherToolbarNotifyAreaViewModel(LauncherToolbarElement model, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, IWindowManager windowManager, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherToolbarTheme = launcherToolbarTheme;
            DispatcherWrapper = dispatcherWrapper;
            WindowManager = windowManager;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsVisible), new[] { nameof(MenuIsChecked), nameof(MenuIcon) });
        }

        #region property

        private ILauncherToolbarTheme LauncherToolbarTheme { get; }
        private IWindowManager WindowManager { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private PropertyChangedHooker PropertyChangedHooker { get; }

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
                return ScreenUtility.GetName(Model.DockScreen, LoggerFactory);
            }
        }
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture? MenuKeyGesture { get; }
        LauncherToolbarIconMaker IconMaker { get; } = new LauncherToolbarIconMaker();
        public DependencyObject MenuIcon => IconMaker.GetToolbarImage(Model.DockScreen, Screen.AllScreens, IconBox.Small, MenuIsChecked);
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked => Model.IsVisible;

        private ICommand? _MenuCommand;
        public ICommand MenuCommand => this._MenuCommand ??= new DelegateCommand(
             () => {
                 var isVisible = Model.IsVisible;
                 Model.ChangeVisibleDelaySave(!isVisible);
                 if(!isVisible) {
                     Model.StartView();
                 } else {
                     var target = WindowManager.GetWindowItems(WindowKind.LauncherToolbar)
                        .First(i => ((LauncherToolbarViewModel)i.ViewModel).DockScreen.DeviceName == Model.DockScreen.DeviceName)
                     ;
                     target.Window.Close();
                 }
             }
         );

        #endregion

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
