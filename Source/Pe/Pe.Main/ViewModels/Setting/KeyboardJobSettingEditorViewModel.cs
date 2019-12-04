using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class KeyboardJobSettingEditorViewModelBase<TJobEditor> : SingleModelViewModelBase<TJobEditor>, IKeyActionId
        where TJobEditor : KeyboardJobSettingEditorElementBase
    {
        public KeyboardJobSettingEditorViewModelBase(TJobEditor model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
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

        public Guid KeyActionId => Model.KeyActionId;

        #endregion
    }

    public sealed class KeyboardReplaceJobSettingEditorViewMode : KeyboardJobSettingEditorViewModelBase<KeyboardReplaceJobSettingEditorElement>
    {
        public KeyboardReplaceJobSettingEditorViewMode(KeyboardReplaceJobSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Key ReplaceKey
        {
            get
            {
                try {
                    var keyReplaceContentConverter = new KeyReplaceContentConverter();
                    return keyReplaceContentConverter.ToReplaceKey(Model.Content);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    return Key.None;
                }
            }
            set
            {
                var keyReplaceContentConverter = new KeyReplaceContentConverter();
                var content = keyReplaceContentConverter.ToContent(value);
                SetModelValue(content, nameof(Model.Content));
            }
        }

        public Key SourceKey
        {
            get
            {
                return Model.Mappings[0].Data.Key;
            }
            set
            {
                Model.Mappings[0].Data.Key = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

    }

    public sealed class KeyboardDisableJobSettingEditorViewModel : KeyboardJobSettingEditorViewModelBase<KeyboardDisableJobSettingEditorElement>
    {
        public KeyboardDisableJobSettingEditorViewModel(KeyboardDisableJobSettingEditorElement model, ILoggerFactory loggerFactory) : base(model, loggerFactory)
        {
        }

        #region property

        public Key Key
        {
            get
            {
                return Model.Mappings[0].Data.Key;
            }
            set
            {
                Model.Mappings[0].Data.Key = value;
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

    public sealed class KeyMappingEditorViewModel : ViewModelBase
    {
        public KeyMappingEditorViewModel(KeyMappingData mapping, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Mapping = mapping;
        }

        #region property

        KeyMappingData Mapping { get; }

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

    public abstract class KeyboardPressedJobSettingEditorViewModelBase : KeyboardJobSettingEditorViewModelBase<KeyboardPressedJobSettingEditorElement>
    {
        public KeyboardPressedJobSettingEditorViewModelBase(KeyboardPressedJobSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            MappingCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<KeyMappingData>, KeyMappingEditorViewModel>(Model.Mappings) {
                ToViewModel = m => new KeyMappingEditorViewModel(m.Data, LoggerFactory),
            };
            MappingItems = MappingCollection.GetDefaultView();
        }


        #region property

        public bool ConveySystem
        {
            get
            {
                var poc = new PressedOptionConverter();
                if(poc.TryGetConveySystem(Model.Options, out var result)) {
                    return result;
                }

                return false;
            }
            set
            {
                var poc = new PressedOptionConverter();
                poc.SetConveySystem(Model.Options, value);
                RaisePropertyChanged();
            }
        }

        ModelViewModelObservableCollectionManagerBase<WrapModel<KeyMappingData>, KeyMappingEditorViewModel> MappingCollection { get; }
        public ICollectionView MappingItems { get; }

        #endregion

        #region command

        public ICommand AddMappingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.AddMapping();
            }
        ));

        #endregion
    }

    public sealed class KeyboardCommandJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase
    {
        public KeyboardCommandJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }
    }

    public sealed class KeyboardLauncherItemJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase
    {
        public KeyboardLauncherItemJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            AllLauncherItemCollection = allLauncherItemCollection;
            AllLauncherItems = AllLauncherItemCollection.CreateView();
        }

        #region property

        [IgnoreValidation]
        ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ICollectionView AllLauncherItems { get; }

        public KeyActionContentLauncherItem Content
        {
            get
            {
                var keyLauncherItemContentConverter = new KeyLauncherItemContentConverter();
                try {
                    return keyLauncherItemContentConverter.ToKeyActionContentLauncherItem(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
                return KeyActionContentLauncherItem.Execute;
            }
            set
            {
                var keyLauncherItemContentConverter = new KeyLauncherItemContentConverter();
                var content = keyLauncherItemContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

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
                    lioc.WriteLauncherItemId( Model.Options, value.LauncherItemId);
                } else {
                    lioc.WriteLauncherItemId(Model.Options, Guid.Empty);
                }
            }
        }

        #endregion

    }
}
