namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// 上限付きコレクション。
	/// <para>上限はほとんどおまけ。</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class FixedSizeCollectionModel<T> : CollectionModel<T>, ILimitSize
	{
		#region define

		/// <summary>
		/// 初期値は上限なしでいいや。
		/// </summary>
		public static int DefaultLimit { get { return 0; } }

		#endregion

		#region variable

		[IgnoreDataMember, XmlIgnore]
		int _limitSize;

		#endregion

		public FixedSizeCollectionModel()
			: this(DefaultLimit, true)
		{ }

		public FixedSizeCollectionModel(int limitSize)
			: this(limitSize, true)
		{ }

		public FixedSizeCollectionModel(int limitSize, bool isFifo)
			: base()
		{
			IsFIFO = isFifo;
			LimitSize = limitSize;

			StockItems = new List<T>();
		}

		public FixedSizeCollectionModel(IEnumerable<T> collection)
			: this(collection, DefaultLimit)
		{ }

		public FixedSizeCollectionModel(IEnumerable<T> collection, int limitSize)
			: this(collection, limitSize, true)
		{ }

		public FixedSizeCollectionModel(IEnumerable<T> collection, int limitSize, bool isFifo)
			: base(collection)
		{
			IsFIFO = isFifo;
			LimitSize = limitSize;

			StockItems = new List<T>();
		}

		#region property

		/// <summary>
		/// 上限サイズ。
		/// <para>0の場合は上限指定なし。</para>
		/// </summary>
		public int LimitSize
		{
			get { return this._limitSize; }
			set
			{
				if (value != 0) {
					if (Count > value) {
						var removeCount = Count - value;
						for (var i = 0; i < removeCount; i++) {
							if (IsFIFO) {
								RemoveAt(0);
							} else {
								RemoveAt(value);
							}
						}
					}
				}
				this._limitSize = value;
			}
		}

		/// <summary>
		/// 上限を超えて押し出されるデータを保持リストへ回すか。
		/// <para>保持リストのデータは本クラスの管理外となる。</para>
		/// </summary>
		[IgnoreDataMember, XmlIgnore]
		public bool StockRemovedItem { get; set; }

		[IgnoreDataMember, XmlIgnore]
		public IList<T> StockItems { get; private set; }

		/// <summary>
		/// 上限サイズが有効か。
		/// </summary>
		public virtual bool UsingLimit 
		{
			get { return LimitSize != 0; } 
		}

		/// <summary>
		/// 追加時に先頭要素から消えていくか。
		/// </summary>
		public bool IsFIFO { get; set; }

		#endregion

		#region function

		void RemoveLimit()
		{
			int removeIndex;
			if (IsFIFO) {
				removeIndex = 0;
			} else {
				removeIndex = Count - 1;
			}
			if (StockRemovedItem) {
				StockItems.Add(this[removeIndex]);
			}
			RemoveAt(removeIndex);
		}

		#endregion

		#region CollectionModel

		public new void Add(T item)
		{
			if(UsingLimit) {
				while(Count >= LimitSize) {
					RemoveLimit();
				}
			}

			base.Add(item);
		}

		public new void Insert(int index, T item)
		{
			base.Insert(index, item);

			if(UsingLimit) {
				while(Count > LimitSize) {
					RemoveLimit();
				}
			}
		}

		#endregion
	}
}
