using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel
{
    public enum ObservableCollectionKind
    {
        Before,
        After,
    }

    /// <summary>
    /// Model と ViewModel の一元的管理。
    /// <para>対になっている部分は内部で対応するがその前後処理までは面倒見ない。</para>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class ModelViewModelObservableCollectionManagerBase<TModel, TViewModel> : ObservableCollectionManagerBase<TModel>
        where TModel : BindModelBase
        where TViewModel : ViewModelBase
    {
        public ModelViewModelObservableCollectionManagerBase(ObservableCollection<TModel> collection, ILogger logger)
            : base(collection, logger)
        {
            ViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelImpl(m)));
        }
        public ModelViewModelObservableCollectionManagerBase(ObservableCollection<TModel> collection, ILoggerFactory loggerFactory)
            : base(collection, loggerFactory)
        {
            ViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelImpl(m)));
        }

        #region property

        public ObservableCollection<TViewModel> ViewModels { get; private set; }

        #endregion

        #region function

        protected abstract TViewModel ToViewModelImpl(TModel model);

        protected abstract void AddItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels);
        protected abstract void RemoveItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> oldItems, int oldStartingIndex, IReadOnlyList<TViewModel> oldViewModels);
        protected abstract void ReplaceItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TModel> oldModels, IReadOnlyList<TViewModel> newViewModels, IReadOnlyList<TViewModel> oldViewModels);
        protected abstract void MoveItemsKindImpl(ObservableCollectionKind kind, int newStartingIndex, int oldStartingIndex);
        protected abstract void ResetItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TViewModel> oldViewModels);

        #endregion

        #region ObservableManager

        protected override void AddItemsImple(IReadOnlyList<TModel> newItems)
        {
            var newViewModels = newItems
                .Cast<TModel>()
                .Select(m => ToViewModelImpl(m))
                .ToList()
            ;

            AddItemsKindImpl(ObservableCollectionKind.Before, newItems, newViewModels);

            ViewModels.AddRange(newViewModels);

            AddItemsKindImpl(ObservableCollectionKind.After, newItems, newViewModels);
        }

        protected override void RemoveItemsImpl(IReadOnlyList<TModel> oldItems, int oldStartingIndex)
        {
            var oldViewModels = ViewModels
                .Skip(oldStartingIndex)
                .Take(oldItems.Count)
                .ToList()
            ;

            RemoveItemsKindImpl(ObservableCollectionKind.Before, oldItems, oldStartingIndex, oldViewModels);

            foreach(var counter in new Counter(oldViewModels.Count)) {
                ViewModels.RemoveAt(oldStartingIndex);
            }
            foreach(var oldViewModel in oldViewModels) {
                oldViewModel.Dispose();
            }

            RemoveItemsKindImpl(ObservableCollectionKind.After, oldItems, oldStartingIndex, oldViewModels);
        }

        protected override void ReplaceItemsImpl(IReadOnlyList<TModel> newItems, IReadOnlyList<TModel> oldItems)
        {
            // TODO: 正直こいつがいつ呼ばれるのか分かってない
            ReplaceItemsKindImpl(ObservableCollectionKind.Before, newItems, oldItems, null, null);
            ReplaceItemsKindImpl(ObservableCollectionKind.After, newItems, oldItems, null, null);
        }

        protected override void MoveItemsImpl(int newStartingIndex, int oldStartingIndex)
        {
            MoveItemsKindImpl(ObservableCollectionKind.Before, newStartingIndex, oldStartingIndex);

            ViewModels.Move(oldStartingIndex, newStartingIndex);

            MoveItemsKindImpl(ObservableCollectionKind.After, newStartingIndex, oldStartingIndex);
        }

        protected override void ResetItemsImpl()
        {
            var oldViewModels = ViewModels;

            ResetItemsKindImpl(ObservableCollectionKind.Before, oldViewModels);

            ViewModels.Clear();
            foreach(var viewModel in oldViewModels) {
                viewModel.Dispose();
            }

            ResetItemsKindImpl(ObservableCollectionKind.After, oldViewModels);
        }

        protected override void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => base.CollectionChanged(e)));
        }

        #endregion
    }


    public class ActionModelViewModelObservableCollectionManager<TModel, TViewModel> : ModelViewModelObservableCollectionManagerBase<TModel, TViewModel>
        where TModel : BindModelBase
        where TViewModel : ViewModelBase
    {
        #region define

        public delegate TViewModel ToViewModelDelegate(TModel model);
        public delegate void AddItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels);
        public delegate void RemoveItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TModel> oldItems, int oldStartingIndex, IReadOnlyList<TViewModel> oldViewModels);
        public delegate void ReplaceItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TModel> oldModels, IReadOnlyList<TViewModel> newViewModels, IReadOnlyList<TViewModel> oldViewModels);
        public delegate void MoveItemsKindDelegate(ObservableCollectionKind kind, int newStartingIndex, int oldStartingIndex);
        public delegate void ResetItemsKindDelegate(ObservableCollectionKind kind, IReadOnlyList<TViewModel> oldViewModels);

        #endregion

        public ActionModelViewModelObservableCollectionManager(ObservableCollection<TModel> collection, ILogger logger)
            : base(collection, logger)
        { }
        public ActionModelViewModelObservableCollectionManager(ObservableCollection<TModel> collection, ILoggerFactory loggerFactory)
            : base(collection, loggerFactory.CreateCurrentClass())
        { }


        #region property

        public ToViewModelDelegate ToViewModel { get; set; }
        public AddItemsKindDelegate AddItems { get; set; }
        public RemoveItemsKindDelegate RemoveItems { get; set; }
        public ReplaceItemsKindDelegate ReplaceItems { get; set; }
        public MoveItemsKindDelegate MoveItems { get; set; }
        public ResetItemsKindDelegate ResetItems { get; set; }

        #endregion

        #region ViewViewModelObservableManagerBase

        protected override TViewModel ToViewModelImpl(TModel model)
        {
            return ToViewModel(model);
        }

        protected override void AddItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> newModels, IReadOnlyList<TViewModel> newViewModels)
        {
            AddItems?.Invoke(kind, newModels, newViewModels);
        }

        protected override void RemoveItemsKindImpl(ObservableCollectionKind kind, IReadOnlyList<TModel> oldItems, int oldStartingIndex, IReadOnlyList<TViewModel> oldViewModels)
        {
            RemoveItems?.Invoke(kind, oldItems, oldStartingIndex, oldViewModels);
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

            base.Dispose(disposing);
        }

        #endregion
    }
}
