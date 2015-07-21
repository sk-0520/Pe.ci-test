namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// コレクションデータ保持用モデル。
	/// <para>ObservableCollectionの単純ラッパー。</para>
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class CollectionModel<TValue> : ObservableCollection<TValue>, IIsDisposed, IModel
	{
		#region variable

		[IgnoreDataMember, XmlIgnore]
		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		public CollectionModel()
			: base()
		{
			IsDisposed = false;

			CollectionChanged += FullObservableCollectionCollectionChanged;
		}

		public CollectionModel(IEnumerable<TValue> items)
			: base(items)
		{
			IsDisposed = false;

			CollectionChanged += FullObservableCollectionCollectionChanged;
		}

		~CollectionModel()
		{
			Dispose(false);
		}

		#region IIsDisposed

		[IgnoreDataMember, XmlIgnore]
		public bool IsDisposed { get; private set; }

		protected virtual void Dispose(bool disposing)
		{
			if (IsDisposed) {
				return;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#region IModel

		[IgnoreDataMember, XmlIgnore]
		public virtual string DisplayText
		{
			get { return GetType().FullName; }
		}

		[IgnoreDataMember, XmlIgnore]
		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if (this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties();
				}

				return this._propertyInfos;
			}
		}

		public IEnumerable<string> GetNameValueList()
		{
			var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
			return ReflectionUtility.GetNameValueStrings(nameValueMap);
		}

		public virtual void Correction()
		{ }

		#endregion

		#region function

		/// <summary>
		/// Addを内部的に繰り返すだけ。
		/// <para>速度的にどうとかじゃなくて毎度毎度foreachするのだりぃ。</para>
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IEnumerable<TValue> items)
		{
			foreach (var item in items) {
				Add(item);
			}
		}

		public void SwapIndex(int indexA, int indexB)
		{
			var itemA = Items[indexA];
			var itemB = Items[indexB];

			//RemoveAt(indexA);
			//RemoveAt(indexB);

			//Insert(indexA, itemB);
			//Insert(indexB, itemA);
			var temp = itemA;
			Items[indexB] = temp;
			Items[indexA] = itemB;

			//var eventA = new NotifyCollectionChangedEventArgs(
			//	NotifyCollectionChangedAction.Reset,
			//	itemA,
			//	indexA
			//);
			//var eventB = new NotifyCollectionChangedEventArgs(
			//	NotifyCollectionChangedAction.Move,
			//	itemB,
			//	indexB
			//);
			//OnCollectionChanged(eventA);
			//OnCollectionChanged(eventB);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void SwapObject(TValue itemA, TValue itemB)
		{
			var indexA = IndexOf(itemA);
			var indexB = IndexOf(itemB);

			//RemoveAt(indexA);
			//RemoveAt(indexB);

			//Insert(indexA, itemB);
			//Insert(indexB, itemA);
			SwapIndex(indexA, indexB);
		}

		public void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.Action == NotifyCollectionChangedAction.Replace) {
				Debug.WriteLine(sender);
			}
			if(e.NewItems != null) {
				foreach(var item in e.NewItems.OfType<INotifyPropertyChanged>()) {
					((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
				}
			}
			if(e.OldItems != null) {
				foreach(var item in e.OldItems.OfType<INotifyPropertyChanged>()) {
					((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
				}
			}
		}

		public void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Replace, 
				sender, 
				sender, 
				IndexOf((TValue)sender)
			);

			OnCollectionChanged(args);
		}

		#endregion
	}
}
