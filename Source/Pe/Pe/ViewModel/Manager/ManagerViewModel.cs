using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.CompatibleWindows;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Manager
{
    public class ManagerViewModel : ViewModelBase, IBuildStatus
    {
        public ManagerViewModel(ApplicationManager applicationManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApplicationManager = applicationManager;

            LauncherToolbarCollection = ApplicationManager.GetLauncherNotifyCollection();
            LauncherToolbarItems = LauncherToolbarCollection.ReadOnlyViewModels;
        }

        #region property

        ApplicationManager ApplicationManager { get; }

        ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> LauncherToolbarCollection { get; }
        public ReadOnlyObservableCollection<LauncherToolbarNotifyAreaViewModel> LauncherToolbarItems { get; }

        #endregion

        #region command


        public ICommand CreateNoteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                NativeMethods.GetCursorPos(out var rawCursorPosition);
                var deviceCursorPosition = PodStructUtility.Convert(rawCursorPosition);
                var currentScreen = Screen.FromDevicePoint(deviceCursorPosition);

                var noteElement = ApplicationManager.CreateNote(currentScreen);
                noteElement.StartView();
            }
        ));

        public ICommand MoveFrontAllNoteCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveFrontAllNote();
             }
         ));

        public ICommand ExitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.Exit();
            }
        ));

        #endregion

        #region function
        #endregion

        #region IBuildStatus

        public BuildType BuildType => BuildStatus.BuildType;

        public Version Version => BuildStatus.Version;
        public string Revision => BuildStatus.Revision;

        #endregion
    }
}
