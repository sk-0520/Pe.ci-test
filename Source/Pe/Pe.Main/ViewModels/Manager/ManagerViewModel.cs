using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Manager
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
            LauncherToolbarItems = LauncherToolbarCollection.ViewModels;

            NoteCollection = ApplicationManager.GetNoteCollection();
            NoteVisibleItems = NoteCollection.CreateView();
            NoteHiddenItems = NoteCollection.CreateView();
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

                var noteElement = ApplicationManager.CreateNote(currentScreen, Models.Data.NoteStartupPosition.CenterScreen);
                noteElement.StartView();
            }
        ));

        public ICommand CompactAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.CompactAllNotes();
            }
        ));
        public ICommand MoveZOrderTopAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveZOrderAllNotes(true);
             }
         ));

        public ICommand MoveZOrderBottomAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.MoveZOrderAllNotes(false);
            }
        ));

        public ICommand ShowCommandViewCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => {
               ApplicationManager.ShowCommandView();
           }
       ));

        public ICommand OpenSettingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowSettingView();
            }
        ));
        public ICommand OpenStartupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowStartupView();
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
