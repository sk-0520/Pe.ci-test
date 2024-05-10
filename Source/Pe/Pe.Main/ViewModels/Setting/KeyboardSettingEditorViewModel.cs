using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class KeyboardSettingEditorViewModel: SettingEditorViewModelBase<KeyboardSettingEditorElement>
    {
        #region variable

        private bool _isPopupCreateJobMenu;

        #endregion

        public KeyboardSettingEditorViewModel(KeyboardSettingEditorElement model, ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ReplaceJobEditorCollection = new ModelViewModelObservableCollectionManager<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode>(model.ReplaceJobEditors, new ModelViewModelObservableCollectionOptions<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode>() {
                ToViewModel = m => new KeyboardReplaceJobSettingEditorViewMode(m, DispatcherWrapper, LoggerFactory),
            });
            ReplaceJobEditors = ReplaceJobEditorCollection.GetDefaultView();

            DisableJobEditorCollection = new ModelViewModelObservableCollectionManager<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel>(Model.DisableJobEditors, new ModelViewModelObservableCollectionOptions<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel>() {
                ToViewModel = m => new KeyboardDisableJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            });
            DisableJobEditors = DisableJobEditorCollection.GetDefaultView();

            AllLauncherItemCollection = allLauncherItemCollection;

            PressedJobEditorCollection = new ModelViewModelObservableCollectionManager<KeyboardPressedJobSettingEditorElement, KeyboardPressedJobSettingEditorViewModelBase>(Model.PressedJobEditors, new ModelViewModelObservableCollectionOptions<KeyboardPressedJobSettingEditorElement, KeyboardPressedJobSettingEditorViewModelBase>() {
                ToViewModel = m => m.Kind switch {
                    KeyActionKind.Command => new KeyboardCommandJobSettingEditorViewModel(m, DispatcherWrapper, loggerFactory),
                    KeyActionKind.LauncherItem => new KeyboardLauncherItemJobSettingEditorViewModel(m, AllLauncherItemCollection, DispatcherWrapper, loggerFactory),
                    KeyActionKind.LauncherToolbar => new KeyboardLauncherToolbarJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
                    KeyActionKind.Note => new KeyboardNoteJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
                    _ => throw new NotImplementedException(),
                }
            });
            PressedJobEditors = PressedJobEditorCollection.GetDefaultView();


            var replaceKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
            ;
            ReplaceKeyItems = new ObservableCollection<Key>(replaceKeyItems);

            var disableKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
            ;
            DisableKeyItems = new ObservableCollection<Key>(disableKeyItems);

            var pressedIgnoreKeys = new[] {
                Key.LeftShift,
                Key.RightShift,
                Key.LeftCtrl,
                Key.RightCtrl,
                Key.LeftAlt,
                Key.RightAlt,
                Key.LWin,
                Key.RWin,
            };
            var pressedKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
                .Where(i => !pressedIgnoreKeys.Any(ii => ii == i))
            ;
            PressedKeyItems = new ObservableCollection<Key>(pressedKeyItems);

        }

        #region property

        private ModelViewModelObservableCollectionManager<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode> ReplaceJobEditorCollection { get; }
        public ICollectionView ReplaceJobEditors { get; }

        private ModelViewModelObservableCollectionManager<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel> DisableJobEditorCollection { get; }
        public ICollectionView DisableJobEditors { get; }

        private ModelViewModelObservableCollectionManager<KeyboardPressedJobSettingEditorElement, KeyboardPressedJobSettingEditorViewModelBase> PressedJobEditorCollection { get; }
        public ICollectionView PressedJobEditors { get; }


        [IgnoreValidation]
        private ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }

        public bool IsPopupCreateJobMenu
        {
            get => this._isPopupCreateJobMenu;
            set => SetProperty(ref this._isPopupCreateJobMenu, value);
        }

        [IgnoreValidation]
        public ObservableCollection<Key> ReplaceKeyItems { get; }
        [IgnoreValidation]
        public ObservableCollection<Key> DisableKeyItems { get; }
        [IgnoreValidation]
        public ObservableCollection<Key> PressedKeyItems { get; }

        #endregion

        #region command

        private ICommand? _AddReplaceJobCommand;
        public ICommand AddReplaceJobCommand => this._AddReplaceJobCommand ??= new DelegateCommand(
             async () => {
                 await Model.AddReplaceJobAsync();
             }
         );

        private ICommand? _RemoveReplaceJobCommand;
        public ICommand RemoveReplaceJobCommand => this._RemoveReplaceJobCommand ??= new DelegateCommand<KeyboardReplaceJobSettingEditorViewMode>(
             o => {
                 Model.RemoveReplaceJob(o.KeyActionId);
             }
         );

        private ICommand? _AddDisableJobCommand;
        public ICommand AddDisableJobCommand => this._AddDisableJobCommand ??= new DelegateCommand(
             async () => {
                 await Model.AddDisableJobAsync();
             }
         );

        private ICommand? _RemoveDisableJobCommand;
        public ICommand RemoveDisableJobCommand => this._RemoveDisableJobCommand ??= new DelegateCommand<KeyboardDisableJobSettingEditorViewModel>(
             o => {
                 Model.RemoveDisableJob(o.KeyActionId);
             }
         );

        private ICommand? _RemovePressedJobCommand;
        public ICommand RemovePressedJobCommand => this._RemovePressedJobCommand ??= new DelegateCommand<KeyboardPressedJobSettingEditorViewModelBase>(
            o => {
                Model.RemovePressedJob(o.KeyActionId);
            }
        );

        private ICommand? _AddCommandJobCommand;
        public ICommand AddCommandJobCommand => this._AddCommandJobCommand ??= new DelegateCommand(
            async () => {
                await AddPressedJobAsync(KeyActionKind.Command);
            }
        );

        private ICommand? _AddLauncherItemJobCommand;
        public ICommand AddLauncherItemJobCommand => this._AddLauncherItemJobCommand ??= new DelegateCommand(
            async () => {
                await AddPressedJobAsync(KeyActionKind.LauncherItem);
            }
        );

        private ICommand? _AddLauncherToolbarJobCommand;
        public ICommand AddLauncherToolbarJobCommand => this._AddLauncherToolbarJobCommand ??= new DelegateCommand(
            async () => {
                await AddPressedJobAsync(KeyActionKind.LauncherToolbar);
            }
        );

        private ICommand? _AddNoteJobCommand;
        public ICommand AddNoteJobCommand => this._AddNoteJobCommand ??= new DelegateCommand(
            async () => {
                await AddPressedJobAsync(KeyActionKind.Note);
            }
        );

        #endregion

        #region function

        private async Task AddPressedJobAsync(KeyActionKind kind)
        {
            await Model.AddPressedJobAsync(kind);
            IsPopupCreateJobMenu = false;
        }

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Keyboard_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    ReplaceJobEditorCollection.Dispose();
                    DisableJobEditorCollection.Dispose();
                    PressedJobEditorCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
