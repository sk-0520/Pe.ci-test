using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using NLog.Filters;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class KeyboardJobSettingEditorViewModelBase<TJobEditor>: SettingItemViewModelBase<TJobEditor>, IKeyActionId
        where TJobEditor : KeyboardJobSettingEditorElementBase
    {
        protected KeyboardJobSettingEditorViewModelBase(TJobEditor model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public string Comment
        {
            get => Model.Comment;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IKeyActionId

        public KeyActionId KeyActionId => Model.KeyActionId;

        #endregion
    }

    public sealed class KeyboardReplaceJobSettingEditorViewMode: KeyboardJobSettingEditorViewModelBase<KeyboardReplaceJobSettingEditorElement>
    {
        public KeyboardReplaceJobSettingEditorViewMode(KeyboardReplaceJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public Key ReplaceKey
        {
            get
            {
                try {
                    var replaceContentConverter = new ReplaceContentConverter();
                    return replaceContentConverter.ToReplaceKey(Model.Content);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    return Key.None;
                }
            }
            set
            {
                var keyReplaceContentConverter = new ReplaceContentConverter();
                var content = keyReplaceContentConverter.ToContent(value);
                SetModelValue(content, nameof(Model.Content));
            }
        }

        public Key SourceKey
        {
            get
            {
                return Model.Mappings[0].Key;
            }
            set
            {
                Model.Mappings[0].Key = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public sealed class KeyboardDisableJobSettingEditorViewModel: KeyboardJobSettingEditorViewModelBase<KeyboardDisableJobSettingEditorElement>
    {
        public KeyboardDisableJobSettingEditorViewModel(KeyboardDisableJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public Key Key
        {
            get
            {
                return Model.Mappings[0].Key;
            }
            set
            {
                Model.Mappings[0].Key = value;
                RaisePropertyChanged();
            }
        }

        public bool Forever
        {
            get
            {
                var doc = new DisableOptionConverter();
                if(doc.TryGetForever(Model.Options, out var result)) {
                    return result;
                }
                return false;
            }
            set
            {
                var doc = new DisableOptionConverter();
                doc.SetForever(Model.Options, value);
                RaisePropertyChanged();
            }
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public sealed class KeyMappingEditorViewModel: ViewModelBase
    {
        public KeyMappingEditorViewModel(KeyMappingData mapping, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Mapping = mapping;
        }

        #region property

        private KeyMappingData Mapping { get; }

        public Key Key
        {
            get => Mapping.Key;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Shift
        {
            get => Mapping.Shift;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Control
        {
            get => Mapping.Control;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Alt
        {
            get => Mapping.Alt;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Super
        {
            get => Mapping.Super;
            set => SetPropertyValue(Mapping, value);
        }

        #endregion
    }

    public abstract class KeyboardPressedJobSettingEditorViewModelBase: KeyboardJobSettingEditorViewModelBase<KeyboardPressedJobSettingEditorElement>
    {
        protected KeyboardPressedJobSettingEditorViewModelBase(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            MappingCollection = new ModelViewModelObservableCollectionManager<KeyMappingData, KeyMappingEditorViewModel>(Model.Mappings, new ModelViewModelObservableCollectionOptions<KeyMappingData, KeyMappingEditorViewModel>() {
                ToViewModel = m => new KeyMappingEditorViewModel(m, LoggerFactory),
            });
            MappingItems = MappingCollection.GetDefaultView();
        }


        #region property

        public bool ThroughSystem
        {
            get
            {
                var poc = new PressedOptionConverter();
                if(poc.TryGetThroughSystem(Model.Options, out var result)) {
                    return result;
                }

                return false;
            }
            set
            {
                var poc = new PressedOptionConverter();
                poc.SetThroughSystem(Model.Options, value);
                RaisePropertyChanged();
            }
        }

        private ModelViewModelObservableCollectionManager<KeyMappingData, KeyMappingEditorViewModel> MappingCollection { get; }
        public ICollectionView MappingItems { get; }

        #endregion

        #region command

        private ICommand? _AddMappingCommand;
        public ICommand AddMappingCommand => this._AddMappingCommand ??= new DelegateCommand(
            () => {
                Model.AddMapping();
            }
        );

        private ICommand? _RemoveMappingCommand;
        public ICommand RemoveMappingCommand => this._RemoveMappingCommand ??= new DelegateCommand<KeyMappingEditorViewModel>(
            o => {
                var index = MappingCollection.ViewModels.IndexOf(o);
                Model.RemoveMappingAt(index);
            },
            o => 1 < MappingCollection.ViewModels.Count
        ).ObservesProperty(() => MappingCollection.ViewModels.Count);

        private ICommand? _UpMappingCommand;
        public ICommand UpMappingCommand => this._UpMappingCommand ??= new DelegateCommand<KeyMappingEditorViewModel>(
             o => {
                 var index = MappingCollection.ViewModels.IndexOf(o);
                 if(index == 0) {
                     return;
                 }
                 var next = index - 1;
                 Model.MoveMapping(index, next);
             }
        );

        private ICommand? _DownMappingCommand;
        public ICommand DownMappingCommand => this._DownMappingCommand ??= new DelegateCommand<KeyMappingEditorViewModel>(
             o => {
                 var index = MappingCollection.ViewModels.IndexOf(o);
                 if(index == MappingCollection.ViewModels.Count - 1) {
                     return;
                 }
                 var next = index + 1;
                 Model.MoveMapping(index, next);
             }
        );

        #endregion
    }

    public abstract class KeyboardPressedJobSettingEditorViewModelBase<TContent>: KeyboardPressedJobSettingEditorViewModelBase
    {
        protected KeyboardPressedJobSettingEditorViewModelBase(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public abstract TContent Content { get; set; }

        #endregion
    }

    public sealed class KeyboardCommandJobSettingEditorViewModel: KeyboardPressedJobSettingEditorViewModelBase
    {
        public KeyboardCommandJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }
    }

    public sealed class KeyboardLauncherItemJobSettingEditorViewModel: KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentLauncherItem>
    {
        public KeyboardLauncherItemJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AllLauncherItemCollection = allLauncherItemCollection;
            AllLauncherItems = AllLauncherItemCollection.CreateView();
            AllLauncherItems.Filter = FilterLauncherItems;
        }

        #region property

        [IgnoreValidation]
        private ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ICollectionView AllLauncherItems { get; }

        public LauncherItemSettingEditorViewModel? LauncherItem
        {
            get
            {
                var lioc = new LauncherItemOptionConverter();
                if(lioc.TryGetLauncherItemId(Model.Options, out var launcherItemId)) {
                    return AllLauncherItemCollection.ViewModels.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
                }
                return null;
            }
            set
            {
                var lioc = new LauncherItemOptionConverter();
                if(value != null) {
                    lioc.WriteLauncherItemId(Model.Options, value.LauncherItemId);
                } else {
                    lioc.WriteLauncherItemId(Model.Options, LauncherItemId.Empty);
                }
            }
        }

        #endregion

        #region function

        private bool FilterLauncherItems(object obj)
        {
            if(obj is LauncherItemSettingEditorViewModel item) {
                if(item.Common.Kind == LauncherItemKind.Separator) {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentLauncherItem Content
        {
            get
            {
                var launcherItemContentConverter = new LauncherItemContentConverter();
                try {
                    return launcherItemContentConverter.ToKeyActionContentLauncherItem(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    // 泥臭い
                    Model.Content = launcherItemContentConverter.ToContent(KeyActionContentLauncherItem.Execute);
                }
                return KeyActionContentLauncherItem.Execute;
            }
            set
            {
                var launcherItemContentConverter = new LauncherItemContentConverter();
                var content = launcherItemContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion
    }

    public sealed class KeyboardLauncherToolbarJobSettingEditorViewModel: KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentLauncherToolbar>
    {
        public KeyboardLauncherToolbarJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            // 将来的に何か増えてもOKにしたい思いとこのタイミング以外で何もできないのでしゃあなし
            Content = KeyActionContentLauncherToolbar.AutoHiddenToHide;
        }

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentLauncherToolbar Content
        {
            get
            {
                var launcherToolbarContentConverter = new LauncherToolbarContentConverter();
                try {
                    return launcherToolbarContentConverter.ToKeyActionContentLauncherToolbar(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
                return KeyActionContentLauncherToolbar.AutoHiddenToHide;
            }
            set
            {
                var launcherToolbarContentConverter = new LauncherToolbarContentConverter();
                var content = launcherToolbarContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion
    }

    public sealed class KeyboardNoteJobSettingEditorViewModel: KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentNote>
    {
        public KeyboardNoteJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            var items = Enum.GetValues<KeyActionContentNote>();
            ContentItems = new List<KeyActionContentNote>(items);
        }

        #region property

        public IReadOnlyList<KeyActionContentNote> ContentItems { get; }

        #endregion

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentNote Content
        {
            get
            {
                var noteContentConverter = new NoteContentConverter();
                try {
                    return noteContentConverter.ToKeyActionContentNote(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    // 泥臭い
                    Model.Content = noteContentConverter.ToContent(KeyActionContentNote.Create);
                }
                return KeyActionContentNote.Create;
            }
            set
            {
                var noteContentConverter = new NoteContentConverter();
                var content = noteContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion
    }
}
