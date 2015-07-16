namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;


	public class FixedSizeCollectionModel<T> : CollectionModel<T>, LimitSize
	{
		public static int DefaultLimit { get { return 64; } }

		private int _limitSize;

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
		}

		public FixedSizeCollectionModel(IEnumerable<T> collection)
			: this(collection, collection.Count())
		{ }

		public FixedSizeCollectionModel(IEnumerable<T> collection, int limitSize)
			: this(collection, limitSize, true)
		{ }

		public FixedSizeCollectionModel(IEnumerable<T> collection, int limitSize, bool isFifo)
			: base(collection)
		{
			IsFIFO = isFifo;
			LimitSize = limitSize;
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

		public new void Add(T item)
		{
			if (UsingLimit) {
				while (Count >= LimitSize) {
					if (IsFIFO) {
						RemoveAt(0);
					} else {
						RemoveAt(Count - 1);
					}
				}
			}

			base.Add(item);
		}

		public new void Insert(int index, T item)
		{
			base.Insert(index, item);

			if (UsingLimit) {
				while (Count > LimitSize) {
					if (IsFIFO) {
						RemoveAt(0);
					} else {
						RemoveAt(Count - 1);
					}
				}
			}
		}
	}
}
