using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.Note;
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

            NoteCollection = ApplicationManager.GetNoteCollection();
            VisibleNoteItems = NoteCollection.CreateCollectionView();
            HiddenNoteItems = NoteCollection.CreateCollectionView();
            VisibleNoteItems.Filter = o => ((NoteNotifyAreaViewModel)o).IsVisible;
            HiddenNoteItems.Filter = o => !((NoteNotifyAreaViewModel)o).IsVisible;
        }

        #region property

        ApplicationManager ApplicationManager { get; }

        ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> LauncherToolbarCollection { get; }
        public ReadOnlyObservableCollection<LauncherToolbarNotifyAreaViewModel> LauncherToolbarItems { get; }

        ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> NoteCollection { get; }
        public ICollectionView VisibleNoteItems { get; }
        public ICollectionView HiddenNoteItems { get; }
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

        public ICommand MoveFrontAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveFrontAllNotes();
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
