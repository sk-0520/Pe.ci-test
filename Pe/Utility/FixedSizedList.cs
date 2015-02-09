namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// 最大サイズ制限リスト。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class FixedSizedList<T>: EventList<T>, ILimitSize
	{
		const int defaultLimit = 64;

		private int _limitSize;

		//public event EventHandler ListChanged;

		public FixedSizedList()
			: base()
		{
			LimitSize = defaultLimit;
		}

		public FixedSizedList(IEnumerable<T> collection)
			: base(collection)
		{
			LimitSize = defaultLimit;
		}

		public FixedSizedList(int limitSize)
			: base()
		{
			LimitSize = limitSize;
		}

		public FixedSizedList(int capacity, int limitSize)
			: base(capacity)
		{
			LimitSize = limitSize;
		}

		public int LimitSize 
		{ 
			get { return this._limitSize; } 
			set
			{
				if(this._limitSize > value && Count > value) {
					RemoveRange(value, Count - value);
				}
				this._limitSize = value;
			}
		}

		public override void Add(T item)
		{
			if(Count >= LimitSize) {
				DisableEvent = true;
				RemoveAt(0);
				DisableEvent = false;
			} 

			base.Add(item);
		}

		public override void AddRange(IEnumerable<T> collection)
		{
			var collectionCount = collection.Count();
			if(collectionCount >= LimitSize) {
				Clear();
				base.AddRange(collection.Skip(collectionCount - LimitSize));
			} else {
				// TODO: ちょっと後回し
				base.AddRange(collection);
			}
		}

		public override void Insert(int index, T item)
		{
			if(Count >= LimitSize) {
				DisableEvent = true;
				RemoveAt(Count - 1);
				DisableEvent = false;
			}
			base.Insert(index, item);
		}
	}
}
