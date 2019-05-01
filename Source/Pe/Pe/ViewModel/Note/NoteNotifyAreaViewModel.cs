using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.ViewModel.Manager;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteNotifyAreaViewModel : SingleModelViewModelBase<NoteElement>, INotifyArea
    {
        public NoteNotifyAreaViewModel(NoteElement model, IWindowManager windowManager, IDispatcherWapper dispatcherWapper, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            WindowManager = windowManager;
            DispatcherWapper = dispatcherWapper;
            NoteTheme = noteTheme;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
        }

        #region property

        IWindowManager WindowManager { get; }
        IDispatcherWapper DispatcherWapper { get; }
        INoteTheme NoteTheme { get; }

        PropertyChangedHooker PropertyChangedHooker { get; }

        public bool IsVisible => Model.IsVisible;

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

        public string MenuHeader => Model.Title;
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture MenuKeyGesture { get; }
        public DependencyObject MenuIcon => DispatcherWapper.Get(() => NoteTheme.GetIconImage(IconScale.Small, Model.IsCompact, Model.IsLocked, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor)));
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked { get; } = false;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(IsVisible) {
                    var target = WindowManager.GetWindowItems(WindowKind.Note)
                       .First(i => ((NoteViewModel)i.ViewModel).NoteId == Model.NoteId)
                    ;
                    var hWnd = HandleUtility.GetWindowHandle(target.Window);
                    WindowsUtility.ShowActive(hWnd);
                    //target.Window.Activate();
                } else {
                    Model.ChangeVisible(true);
                    Model.StartView();
                }
            },
            () => MenuIsEnabled
        ));

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }


    }
}
