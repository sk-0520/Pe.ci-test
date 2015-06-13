namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	[Serializable]
	public class TIdCollection<TKey, TValue>: DisposeFinalizeModelBase, ICollection<TValue>
		where TValue: ITId<TKey>
		where TKey: IComparable
	{
		#region variable

		protected Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();
		protected bool _isReadOnly = false;

		#endregion

		public TIdCollection()
			: base()
		{
			Items = new List<TValue>();
		}

		#region property

		public List<TValue> Items { get; set; }

		#endregion

		#region ICollection

		public int Count { get { return Items.Count; } }
		public bool IsReadOnly { get { return this._isReadOnly; } }

		/// <summary>
		/// 要素を追加する。
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException">valueがnull</exception>
		/// <exception cref="ArgumentException">value.Idがすでに存在する</exception>
		public void Add(TValue value)
		{
			CheckReadOnly();

			Add(value, true);
		}

		public void Clear()
		{
			CheckReadOnly();

			Items.Clear();
			this._map.Clear();
		}

		public bool Contains(TValue item)
		{
			return Items.Contains(item);
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
			Items.CopyTo(array, arrayIndex);
		}

		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		public bool Remove(TValue item)
		{
			if(Items.Remove(item)) {
				this._map.Remove(item.Id);
				return true;
			}

			return false;
		}

		#endregion

		#region indexer

		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (this._map.TryGetValue(key, out result)) {
					return result;
				}

				result = Items.FirstOrDefault(i => key.CompareTo(i.Id) == 0);
				if (result != null) {
					this._map[result.Id] = result;
					return result;
				}

				throw new IndexOutOfRangeException(GetIdString(key));
			}
		}

		#endregion

		#region function

		static string GetIdString(TKey id)
		{
			return string.Format("Id = {0}", id);
		}
		static string GetIdString(ITId<TKey> id)
		{
			return GetIdString(id);
		}
		static bool IsEqual(TKey a, TKey b)
		{
			return a.CompareTo(b) == 0;
		}
		static bool IsEqual(TValue a, TValue b)
		{
			return IsEqual(a.Id, b.Id);
		}
		
		void CheckReadOnly()
		{
			if(IsReadOnly) {
				throw new NotSupportedException();
			}
		}

		void Add(TValue value, bool check)
		{
			if (check) {
				if (value == null) {
					throw new ArgumentNullException("value");
				}

				if(Items.Any(i => IsEqual(value, i))) {
					throw new ArgumentException(GetIdString(value));
				}
			}

			Items.Add(value);
			this._map[value.Id] = value;
		}

		/// <summary>
		/// 要素を設定する。
		/// <para>既に存在する場合は上書きされる。</para>
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException">valueがnull</exception>
		public void Set(TValue value)
		{
			if (value == null) {
				throw new ArgumentNullException("value");
			}

			var index = Items.FindIndex(i => value.Id.CompareTo(i) == 0);
			if(index != -1) {
				Items[index] = value;
				this._map[value.Id] = value;
			} else {
				Add(value, false);
			}
		}

		void Swap(TKey keyA, TKey keyB)
		{
			var valueA = this[keyA];
			var valueB = this[keyB];
			valueA.Id = keyB;
			valueB.Id = keyA;
			this._map[keyA] = valueB;
			this._map[keyB] = valueA;
		}

		public bool TryGetValue(TKey key, out TValue result)
		{
			if (this._map.TryGetValue(key, out result)) {
				return true;
			}

			var item = Items.FirstOrDefault(i => IsEqual(key, i.Id));
			if (item != null) {
				result = item;
				return true;
			}
			result = default(TValue);
			return false;
		}

		public bool Contains(TKey key)
		{
			TValue temp;
			if (this._map.TryGetValue(key, out temp)) {
				return true;
			}

			return Items.Any(i => IsEqual(key, i.Id));
		}

		void ChangeId(TKey src, TKey dst)
		{
			TValue tempValue;
			if (TryGetValue(dst, out tempValue)) {
				throw new ArgumentException(string.Format("exists key({0})", dst));
			}

			var srcValue = this[src];
			srcValue.Id = dst;
			this._map.Remove(src);
			this._map[dst] = srcValue;
		}

		public bool ChangeReadOnly()
		{
			if(!IsReadOnly) {
				return true;
			}

			return false;
		}

		#endregion
	}
}
