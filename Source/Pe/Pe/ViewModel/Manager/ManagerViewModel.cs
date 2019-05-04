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
        #region property

        bool _isOpenNoteMenu;

        #endregion

        public ManagerViewModel(ApplicationManager applicationManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApplicationManager = applicationManager;

            LauncherToolbarCollection = ApplicationManager.GetLauncherNotifyCollection();
            LauncherToolbarItems = LauncherToolbarCollection.ReadOnlyViewModels;

            NoteCollection = ApplicationManager.GetNoteCollection();
            NoteVisibleItems = NoteCollection.CreateCollectionView();
            NoteHiddenItems = NoteCollection.CreateCollectionView();
            NoteVisibleItems.Filter = o => ((NoteNotifyAreaViewModel)o).IsVisible;
            NoteHiddenItems.Filter = o => !((NoteNotifyAreaViewModel)o).IsVisible;
        }

        #region property

        ApplicationManager ApplicationManager { get; }

        ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> LauncherToolbarCollection { get; }
        public ReadOnlyObservableCollection<LauncherToolbarNotifyAreaViewModel> LauncherToolbarItems { get; }

        ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> NoteCollection { get; }
        public ICollectionView NoteVisibleItems { get; }
        public ICollectionView NoteHiddenItems { get; }


        public bool IsOpenNoteMenu
        {
            get => this._isOpenNoteMenu;
            set
            {
                if(SetProperty(ref this._isOpenNoteMenu, value)) {
                    if(IsOpenNoteMenu) {
                        NoteVisibleItems.Refresh();
                        NoteHiddenItems.Refresh();
                    }
                }
            }
        }

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

        public ICommand CompactAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.CompactAllNotes();
            }
        ));
        public ICommand MoveZorderTopAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveZorderAllNotes(true);
             }
         ));

        public ICommand MoveZorderBottomAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.MoveZorderAllNotes(false);
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
