using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupSettingEditorElement>, ILauncherGroupId
    {
        #region variable

        LauncherItemWithIconViewModel<CommonLauncherItemViewModel>? _selectedLauncherItem;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupSettingEditorElement model, IReadOnlyList<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> allLauncherItems, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            LauncherGroupTheme = launcherGroupTheme;
            DispatcherWrapper = dispatcherWrapper;
            AllLauncherItems = allLauncherItems;

            LauncherCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<Guid>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(Model.LauncherItems) {
                RemoveViewModelToDispose = false, // 共有アイテムを使用しているので破棄させない
                ToViewModel = m => AllLauncherItems.First(i => i.LauncherItemId == m.Data),
            };
            LauncherItems = LauncherCollection.ViewModels;
        }

        #region property

        /// <summary>
        /// 共用しているランチャーアイテム一覧。
        /// <para>親元でアイコンと共通項目構築済みのランチャーアイテム。毎回作るのあれだし。</para>
        /// </summary>
        IReadOnlyList<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> AllLauncherItems { get; }

        /// <summary>
        /// 所属ランチャーアイテム。
        /// <para>注意: 設定中データ状態はモデル側に送らない。</para>
        /// </summary>
        //public ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }
        ModelViewModelObservableCollectionManagerBase<WrapModel<Guid>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherCollection { get; }
        public ReadOnlyObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }

        ILauncherGroupTheme LauncherGroupTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }


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
                ReloadIcon();
            }
        }

        public LauncherGroupImageName ImageName
        {
            get => Model.ImageName;
            set
            {
                SetModelValue(value);
                ReloadIcon();
            }
        }

        public long Sequence
        {
            get => Model.Sequence;
            set
            {
                SetModelValue(value);
                ReloadIcon();
            }
        }

        public LauncherGroupKind Kind => Model.Kind;

        public object GroupIcon => DispatcherWrapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, false));

        public LauncherItemWithIconViewModel<CommonLauncherItemViewModel>? SelectedLauncherItem
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

        void ReloadIcon()
        {
            RaisePropertyChanged(nameof(GroupIcon));
        }


        public void SaveWithoutSequence()
        {
            Model.SaveWithoutSequence();
        }

        public void InsertNewLauncherItem(int index, ILauncherItemId launcherItem)
        {
            Model.InsertLauncherItemId(index, launcherItem.LauncherItemId);
        }

        public void MoveLauncherItem(int startIndex, int insertIndex)
        {
            Model.MoveLauncherItemId(startIndex, insertIndex);
        }


        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId => Model.LauncherGroupId;

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
