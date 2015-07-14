namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class FixeSizedList<T>: ObservableCollection<T>, LimitSize
	{
		static int DefaultLimit { get { return 64; } }

		private int _limitSize;

		//public event EventHandler ListChanged;

		public FixeSizedList()
			: base()
		{
			LimitSize = DefaultLimit;
		}

		public FixeSizedList(IEnumerable<T> collection)
			: base(collection)
		{
			LimitSize = DefaultLimit;
		}

		public FixeSizedList(int limitSize)
			: base()
		{
			LimitSize = limitSize;
		}

		public int LimitSize
		{
			get { return this._limitSize; }
			set
			{
				if(this._limitSize > value && Count > value) {
					var removeCount = Count - value;
					for(var i = 0; i < removeCount;i++ ) {
						RemoveAt(0);
					}
				}
				this._limitSize = value;
			}
		}

		public new void Add(T item)
		{
			if(Count >= LimitSize) {
				RemoveAt(0);
			}

			base.Add(item);
		}

		public new void Insert(int index, T item)
		{
			if(Count >= LimitSize) {
				RemoveAt(Count - 1);
			}
			base.Insert(index, item);
		}
	}
}
