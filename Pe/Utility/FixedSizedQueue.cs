namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// <see cref="http://stackoverflow.com/questions/5852863"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConcurrentFixedSizedQueue<T>: ConcurrentQueue<T>, ILimitSize
	{
		const int defaultLimit = 64;

		public ConcurrentFixedSizedQueue()
			: base()
		{
			LimitSize = defaultLimit;
		}

		public ConcurrentFixedSizedQueue(IEnumerable<T> collection)
			: base(collection)
		{
			LimitSize = defaultLimit;
		}

		public ConcurrentFixedSizedQueue(int limitSize)
			: base()
		{
			LimitSize = limitSize;
		}

		public int LimitSize { get; set; }

		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			lock(this) {
				T overflow;
				while(Count > LimitSize && TryDequeue(out overflow)) ;
			}
		}
	}
}
