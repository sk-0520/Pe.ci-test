namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// ModelとViewModelの対となるコレクションを保持する。
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <typeparam name="TViewModel"></typeparam>
	public abstract class MVMPairCollectionBase<TModel, TViewModel> : ICollection<MVMPair<TModel, TViewModel>>
		where TModel : ModelBase
		where TViewModel : ViewModelBase
	{
		protected MVMPairCollectionBase()
		{ }

		public MVMPairCollectionBase(CollectionModel<TModel> modelList, CollectionModel<TViewModel> viewModelList)
		{
			ModelList = modelList;
			ViewModelList = viewModelList;
		}

		public MVMPairCollectionBase(CollectionModel<TModel> modelList, IEnumerable<object> dataList)
		{
			ModelList = modelList;
			ViewModelList = new CollectionModel<TViewModel>(CreateViewModelList(modelList, dataList));
		}

		public MVMPairCollectionBase(CollectionModel<TModel> modelList, object data)
		{
			ModelList = modelList;
			ViewModelList = new CollectionModel<TViewModel>(CreateViewModelList(modelList, Enumerable.Repeat(data, ModelList.Count)));
		}

		#region property

		public CollectionModel<TModel> ModelList { get; protected set; }
		public CollectionModel<TViewModel> ViewModelList { get; protected set; } 

		#endregion

		#region function

		/// <summary>
		/// ViewModelを作成。
		/// </summary>
		/// <param name="model">元になるModel。</param>
		/// <param name="data">ViewModel生成に必要なModel以外のデータ。</param>
		/// <returns></returns>
		public abstract TViewModel CreateViewModel(TModel model, object data);

		protected IEnumerable<TViewModel> CreateViewModelList(IEnumerable<TModel> model, IEnumerable<object> data)
		{
			foreach (var pair in model.Zip(data, (f, s) => new { Model = f, Data = s })) {
				yield return CreateViewModel(pair.Model, pair.Data);
			}
		}

		public int IndexOf(MVMPair<TModel, TViewModel> item)
		{
			var index = ModelList.IndexOf(item.Model);
			if (index == -1) {
				return -1;
			}
			
			if (ViewModelList[index] != item.ViewModel) {
				return -1;
			}

			return index;
		}

		public MVMPair<TModel, TViewModel> Add(TModel model, object data)
		{
			var viewModel = CreateViewModel(model, data);

			var item = MVMPair.Create(model, viewModel);
			Add(item);

			return item;
		}

		public void RemoveAt(int index)
		{
			ModelList.RemoveAt(index);
			ViewModelList.RemoveAt(index);
		}

		public bool Remove(TModel model)
		{
			var index = ModelList.IndexOf(model);
			if (index != -1) {
				RemoveAt(index);
				return true;
			} else {
				return false;
			}

		}
		public bool Remove(TViewModel viewModel)
		{
			var index = ViewModelList.IndexOf(viewModel);
			if (index != -1) {
				RemoveAt(index);
				return true;
			} else {
				return false;
			}

		}

		public MVMPair<TModel, TViewModel> Insert(int index, TModel model, object data)
		{
			var viewModel = CreateViewModel(model, data);

			return Insert(index, MVMPair.Create(model, viewModel));
		}

		public MVMPair<TModel, TViewModel> Insert(int index, MVMPair<TModel, TViewModel> pair)
		{
			ModelList.Insert(index, pair.Model);
			ViewModelList.Insert(index, pair.ViewModel);

			return pair;
		}

		public void SwapIndex(int indexA, int indexB)
		{
			ModelList.SwapIndex(indexA, indexB);
			ViewModelList.SwapIndex(indexA, indexB);
		}

		#endregion

		#region ICollection

		public int Count { get { return ModelList.Count; } }
		public bool IsReadOnly { get; set; }

		public void Add(MVMPair<TModel, TViewModel> item)
		{
			ModelList.Add(item.Model);
			ViewModelList.Add(item.ViewModel);
		}

		public void Clear()
		{
			ModelList.Clear();
			ViewModelList.Clear();
		}

		public bool Contains(MVMPair<TModel, TViewModel> item)
		{
			var index = IndexOf(item);
			return index != -1;
		}


		public void CopyTo(MVMPair<TModel, TViewModel>[] array, int index)
		{
			var i = index;
			foreach (var item in array) {
				array[i++] = item;
			}
		}

		public IEnumerator<MVMPair<TModel, TViewModel>> GetEnumerator()
		{
			foreach (var pair in ModelList.Zip(ViewModelList, (m, v) => MVMPair.Create(m, v))) {
				yield return pair;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

		public bool Remove(MVMPair<TModel, TViewModel> item)
		{
			var index = IndexOf(item);
			if (index == -1) {
				return false;
			}

			RemoveAt(index);

			return true;
		}

		#endregion
	}
}
