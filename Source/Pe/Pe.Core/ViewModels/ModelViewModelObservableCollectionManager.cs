using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.Base.Models;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    /// <summary>
    /// 処理種別。
    /// </summary>
    public enum ObservableCollectionKind
    {
        /// <summary>
        /// 実行前。
        /// </summary>
        Before,
        /// <summary>
        /// 実行後。
        /// </summary>
        After,
    }

    /// <summary>
    /// <typeparamref name="TModel"/> と <typeparamref name="TViewModel"/> の一元的管理。
    /// <para>対になっている部分は内部で対応するがその前後処理までは面倒見ない。</para>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class ModelViewModelObservableCollectionManagerBase<TModel, TViewModel>: ObservableCollectionManagerBase<TModel>
        where TModel : BindModelBase
        where TViewModel : ViewModelBase
    {
        #region variable

        private ReadOnlyObservableCollection<TViewModel>? _readOnlyViewModels;

        #endregion

        protected ModelViewModelObservableCollectionManagerBase(ReadOnlyObservableCollection<TModel> collection)
            : base(collection)
        {
            EditableViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelImpl(m))!);
        }

        protected ModelViewModelObservableCollectionManagerBase(ObservableCollection<TModel> collection)
            : base(collection)
        {
            EditableViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelImpl(m))!);
        }

        #region property

        /// <summary>
        /// 内部使用する<typeparamref name="TViewModel"/>のコレクション。
        /// </summary>
        protected ObservableCollection<TViewModel> EditableViewModels { get; private set; }
        /// <summary>
        /// 外部使用する<typeparamref name="TViewModel"/>のコレクション。
        /// </summary>
        public ReadOnlyObservableCollection<TViewModel> ViewModels
        {
            get => this._readOnlyViewModels ??= new ReadOnlyObservableCollection<TViewModel>(EditableViewModels);
        }

        /// <summary>
        /// アイテム削除時に対象 ViewModel の <see cref="TViewModel.Dispose"/> を呼び出すか。
        /// </summary>
        public bool ManagingResource { get; set; } = true;

        /// <inheritdoc cref="ICollection{TViewModel}.Count"/>
        public int Count => EditableViewModels.Count;

        #endregion

        #region function

        /// <summary>
        /// <typeparamref name="TModel"/>を<typeparamref name="TViewModel"/>に変換する。
        /// </summary>
        /// <param name="model"></param>
        /// <returns>初期化前の場合はnull、初期化後は生成後の<typeparamref name="TViewModel"/>。</returns>
        protected abstract TViewModel? ToViewModelImpl(TModel model);

        protected abstract void AddItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels);
        protected abstract void InsertItemsKindImpl(ObservableCollectionKind kind, int insertIndex, IReadOnlyList<TModel> newModels);
        protected abstract void RemoveItemsKindImpl(ObservableCollectionKind kind, int oldStartingIndex, IReadOnlyList<TModel> oldItems, IReadOnlyList<TViewModel> oldViewModels);
        protected abstract void ReplaceItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TModel> oldModels, IReadOnlyList<TViewModel> newViewModels, IReadOnlyList<TViewModel> oldViewModels);
        protected abstract void MoveItemsKindImpl(ObservableCollectionKind kind, int newStartingIndex, int oldStartingIndex);
        protected abstract void ResetItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TViewModel> oldViewModels);

        public ICollectionView GetDefaultView()
        {
            return CollectionViewSource.GetDefaultView(EditableViewModels);
        }

        public ICollectionView CreateView()
        {
            return new ListCollectionView(EditableViewModels);
        }

        public int IndexOf(TViewModel viewModel) => EditableViewModels.IndexOf(viewModel);

        public bool TryGetModel(TViewModel viewModel, [MaybeNullWhen(false)] out TModel result)
        {
            var index = IndexOf(viewModel);

            if(index == -1) {
                result = default;
                return false;
            }

            result = Collection[index];
            return true;
        }

        /// <summary>
        /// 対になる<typeparamref name="TModel"/>を取得。
        /// </summary>
        /// <param name="viewModel">対になっている<typeparamref name="TViewModel"/>。</param>
        /// <returns>見つからない場合は <typeparamref name="TModel"/> の初期値。</returns>
        [return: MaybeNull]
        public TModel GetModel(TViewModel viewModel)
        {
            if(TryGetModel(viewModel, out var result)) {
                return result;
            }

            return default;
        }

        public bool TryGetViewModel(TModel model, [MaybeNullWhen(false)] out TViewModel result)
        {
            var index = IndexOf(model);

            if(index == -1) {
                result = default;
                return false;
            }

            result = EditableViewModels[index];
            return true;
        }


        /// <summary>
        /// 対になる<typeparamref name="TViewModel"/>を取得。
        /// </summary>
        /// <param name="model">対になっている<typeparamref name="TModel"/>。</param>
        /// <returns>見つからない場合は <typeparamref name="TViewModel"/> の初期値。</returns>
        [return: MaybeNull]
        public TViewModel GetViewModel(TModel model)
        {
            if(TryGetViewModel(model, out var result)) {
                return result;
            }

            return default;
        }

        #endregion

        #region ObservableManager

        protected override void AddItemsImpl(IReadOnlyList<TModel> newItems)
        {
            var newViewModels = newItems
                .Select(m => ToViewModelImpl(m)!)
                .ToList()
            ;

            AddItemsKindImpl(ObservableCollectionKind.Before, newItems, newViewModels);

            EditableViewModels.AddRange(newViewModels);

            AddItemsKindImpl(ObservableCollectionKind.After, newItems, newViewModels);
        }

        protected override void InsertItemsImpl(int insertIndex, IReadOnlyList<TModel> newItems)
        {
            var newViewModels = newItems
                .Select(m => ToViewModelImpl(m)!)
                .Counting(insertIndex)
                .ToList()
            ;

            InsertItemsKindImpl(ObservableCollectionKind.Before, insertIndex, newItems);

            foreach(var item in newViewModels) {
                EditableViewModels.Insert(item.Number, item.Value);
            }

            InsertItemsKindImpl(ObservableCollectionKind.After, insertIndex, newItems);
        }


        protected override void RemoveItemsImpl(int oldStartingIndex, IReadOnlyList<TModel> oldItems)
        {
            var oldViewModels = EditableViewModels
                .Skip(oldStartingIndex)
                .Take(oldItems.Count)
                .ToList()
            ;

            RemoveItemsKindImpl(ObservableCollectionKind.Before, oldStartingIndex, oldItems, oldViewModels);

            foreach(var counter in new Counter(oldViewModels.Count)) {
                EditableViewModels.RemoveAt(oldStartingIndex);
            }
            if(ManagingResource) {
                foreach(var oldViewModel in oldViewModels) {
                    oldViewModel.Dispose();
                }
            }

            RemoveItemsKindImpl(ObservableCollectionKind.After, oldStartingIndex, oldItems, oldViewModels);
        }

        protected override void ReplaceItemsImpl(IReadOnlyList<TModel> newItems, IReadOnlyList<TModel> oldItems)
        {
            // TODO: 正直こいつがいつ呼ばれるのか分かってない
            ReplaceItemsKindImpl(ObservableCollectionKind.Before, newItems, oldItems, Array.Empty<TViewModel>(), Array.Empty<TViewModel>());

            ReplaceItemsKindImpl(ObservableCollectionKind.After, newItems, oldItems, Array.Empty<TViewModel>(), Array.Empty<TViewModel>());
        }

        protected override void MoveItemsImpl(int newStartingIndex, int oldStartingIndex)
        {
            MoveItemsKindImpl(ObservableCollectionKind.Before, newStartingIndex, oldStartingIndex);

            EditableViewModels.Move(oldStartingIndex, newStartingIndex);

            MoveItemsKindImpl(ObservableCollectionKind.After, newStartingIndex, oldStartingIndex);
        }

        protected override void ResetItemsImpl()
        {
            var oldViewModels = EditableViewModels;

            ResetItemsKindImpl(ObservableCollectionKind.Before, oldViewModels);

            EditableViewModels.Clear();
            foreach(var viewModel in oldViewModels) {
                viewModel.Dispose();
            }

            ResetItemsKindImpl(ObservableCollectionKind.After, oldViewModels);
        }

        protected override void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => base.CollectionChanged(e)));
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    var oldItems = EditableViewModels.ToArray();
                    EditableViewModels.Clear();

                    if(ManagingResource) {
                        foreach(var oldItem in oldItems) {
                            oldItem.Dispose();
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }


    public class ActionModelViewModelObservableCollectionManager<TModel, TViewModel>: ModelViewModelObservableCollectionManagerBase<TModel, TViewModel>
        where TModel : BindModelBase
        where TViewModel : ViewModelBase
    {
        #region define

        public delegate TViewModel ToViewModelDelegate(TModel model);
        public delegate void AddItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels);
        public delegate void InsertItemsKindDelegate(ObservableCollectionKind kind, int insertIndex, IReadOnlyList<TModel> newModels);
        public delegate void RemoveItemsKindDelegate(ObservableCollectionKind kind, int oldStartingIndex, IReadOnlyList<TModel> oldItems, IReadOnlyList<TViewModel> oldViewModels);
        public delegate void ReplaceItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TModel> oldModels, IReadOnlyList<TViewModel> newViewModels, IReadOnlyList<TViewModel> oldViewModels);
        public delegate void MoveItemsKindDelegate(ObservableCollectionKind kind, int newStartingIndex, int oldStartingIndex);
        public delegate void ResetItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TViewModel> oldViewModels);

        #endregion

        #region variable

        private ToViewModelDelegate? _toViewModel;

        #endregion

        public ActionModelViewModelObservableCollectionManager(ReadOnlyObservableCollection<TModel> collection)
            : base(collection)
        { }

        public ActionModelViewModelObservableCollectionManager(ObservableCollection<TModel> collection)
            : base(collection)
        { }

        #region property

        private IList<TModel>? StockModels { get; set; } = new List<TModel>();

        /// <summary>
        /// <typeparamref name="TModel"/>から<typeparamref name="TViewModel"/>への変換処理。
        /// <para>NOTE: 未設定の場合、内部的にストックされ、設定後一気に処理が走る。コンストラクタ対策。</para>
        /// </summary>
        public ToViewModelDelegate? ToViewModel
        {
            get => this._toViewModel;
            set
            {
                this._toViewModel = value;
                if(this._toViewModel != null && StockModels != null && StockModels.Count != 0) {
                    Debug.Assert(EditableViewModels.Count == StockModels.Count);
                    for(var i = 0; i < StockModels.Count; i++) {
                        EditableViewModels[i] = this._toViewModel(StockModels[i]);
                    }

                    StockModels.Clear();
                    StockModels = null;
                }
            }
        }
        public AddItemsKindDelegate? AddItems { get; set; }
        public InsertItemsKindDelegate? InsertItems { get; set; }
        public RemoveItemsKindDelegate? RemoveItems { get; set; }
        public ReplaceItemsKindDelegate? ReplaceItems { get; set; }
        public MoveItemsKindDelegate? MoveItems { get; set; }
        public ResetItemsKindDelegate? ResetItems { get; set; }

        #endregion

        #region ModelViewModelObservableCollectionManagerBase

        protected override TViewModel? ToViewModelImpl(TModel model)
        {
            if(ToViewModel != null) {
                return ToViewModel(model);
            }

            Debug.Assert(StockModels != null);
            StockModels.Add(model);
            return default;
        }

        protected override void AddItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels)
        {
            AddItems?.Invoke(kind, newModels, newViewModels);
        }

        protected override void InsertItemsKindImpl(ObservableCollectionKind kind, int insertIndex, IReadOnlyList<TModel> newModels)
        {
            InsertItems?.Invoke(kind, insertIndex, newModels);
        }


        protected override void RemoveItemsKindImpl(ObservableCollectionKind kind, int oldStartingIndex, IReadOnlyList<TModel> oldItems, IReadOnlyList<TViewModel> oldViewModels)
        {
            RemoveItems?.Invoke(kind, oldStartingIndex, oldItems, oldViewModels);
        }

        protected override void ReplaceItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TModel> oldModels, IReadOnlyList<TViewModel> newViewModels, IReadOnlyList<TViewModel> oldViewModels)
        {
            ReplaceItems?.Invoke(kind, newModels, oldModels, newViewModels, oldViewModels);
        }

        protected override void MoveItemsKindImpl(ObservableCollectionKind kind, int newStartingIndex, int oldStartingIndex)
        {
            MoveItems?.Invoke(kind, newStartingIndex, oldStartingIndex);
        }

        protected override void ResetItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TViewModel> oldViewModels)
        {
            ResetItems?.Invoke(kind, oldViewModels);
        }

        protected override void Dispose(bool disposing)
        {
            ToViewModel = null;
            AddItems = null;
            RemoveItems = null;
            ReplaceItems = null;
            MoveItems = null;
            ResetItems = null;

            StockModels?.Clear();
            StockModels = null;

            base.Dispose(disposing);
        }

        #endregion
    }
}
