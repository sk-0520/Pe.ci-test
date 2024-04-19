using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel: SingleModelViewModelBase<LauncherGroupSettingEditorElement>, ILauncherGroupId
    {
        #region variable

        private LauncherItemSettingEditorViewModel? _selectedLauncherItem;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupSettingEditorElement model, ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            DispatcherWrapper = dispatcherWrapper;
            AllLauncherItemCollection = allLauncherItemCollection;

            LauncherCollection = new ModelViewModelObservableCollectionManager<WrapModel<LauncherItemId>, LauncherItemSettingEditorViewModel>(Model.LauncherItems, new ModelViewModelObservableCollectionOptions<WrapModel<LauncherItemId>, LauncherItemSettingEditorViewModel>() {
                AutoDisposeViewModel = false, // 共有アイテムを使用しているので破棄させない
                ToViewModel = (m) => {
                    var itemVm = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == m.Data);
                    return itemVm.Clone();
                }
            });
            LauncherItems = LauncherCollection.ViewModels;
        }

        #region property

        /// <summary>
        /// 共用しているランチャーアイテム一覧。
        /// </summary>
        /// <remarks>
        /// <para>親元でアイコンと共通項目構築済みのランチャーアイテム。毎回作るのあれだし。</para>
        /// </remarks>
        [IgnoreValidation]
        private ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }

        /// <summary>
        /// 所属ランチャーアイテム。
        /// </summary>
        /// <remarks>
        /// <para>注意: 設定中データ状態はモデル側に送らない。</para>
        /// </remarks>
        //public ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }
        [IgnoreValidation]
        private ModelViewModelObservableCollectionManager<WrapModel<LauncherItemId>, LauncherItemSettingEditorViewModel> LauncherCollection { get; }
        [IgnoreValidation]
        public ReadOnlyObservableCollection<LauncherItemSettingEditorViewModel> LauncherItems { get; }

        private IDispatcherWrapper DispatcherWrapper { get; }


        [Required]
        public string Name
        {
            get => Model.Name;
            set
            {
                SetModelValue(value);
                ValidateProperty(value);
            }
        }

        public Color ImageColor
        {
            get => Model.ImageColor;
            set
            {
                SetModelValue(value);
                ReloadGroupIcon();
            }
        }

        public LauncherGroupImageName ImageName
        {
            get => Model.ImageName;
            set
            {
                SetModelValue(value);
                ReloadGroupIcon();
            }
        }

        //public long Sequence
        //{
        //    get => Model.Sequence;
        //    set
        //    {
        //        SetModelValue(value);
        //        ReloadGroupIcon();
        //    }
        //}

        public LauncherGroupKind Kind => Model.Kind;

        private LauncherGroupIconMaker IconMaker { get; } = new LauncherGroupIconMaker();

        public object GroupIcon => IconMaker.GetGroupImage(ImageName, ImageColor, IconBox.Small, IconSize.DefaultScale, false);

        [IgnoreValidation]
        public LauncherItemSettingEditorViewModel? SelectedLauncherItem
        {
            get => this._selectedLauncherItem;
            set
            {
                SetProperty(ref this._selectedLauncherItem, value);
            }
        }

        #endregion

        #region command


        #endregion

        #region function

        private void ReloadGroupIcon()
        {
            RaisePropertyChanged(nameof(GroupIcon));
        }

        /*
        public void SaveWithoutSequence()
        {
            Model.SaveWithoutSequence();
        }
        */

        public void InsertNewLauncherItem(int index, ILauncherItemId launcherItem)
        {
            Model.InsertLauncherItemId(index, launcherItem.LauncherItemId);
        }

        public void MoveLauncherItem(int startIndex, int insertIndex)
        {
            Model.MoveLauncherItemId(startIndex, insertIndex);
        }

        public void RemoveLauncherItem(LauncherItemSettingEditorViewModel launcherItem)
        {
            var index = LauncherItems.IndexOf(launcherItem);
            Model.RemoveLauncherItemAt(index);
        }

        #endregion

        #region ILauncherGroupId

        public LauncherGroupId LauncherGroupId => Model.LauncherGroupId;

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    LauncherCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
