using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// <see cref="http://stackoverflow.com/questions/5852863"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConcurrentFixedSizedQueue<T>: ConcurrentQueue<T>
	{
		const int defaultLimit = 64;

		public ConcurrentFixedSizedQueue()
			: base()
		{
			Limit = defaultLimit;
		}

		public ConcurrentFixedSizedQueue(IEnumerable<T> collection)
			: base(collection)
		{
			Limit = defaultLimit;
		}

		public ConcurrentFixedSizedQueue(int limit)
			: base()
		{
			Limit = limit;
		}

		public int Limit { get; set; }

		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			lock(this) {
				T overflow;
				while(Count > Limit && TryDequeue(out overflow)) ;
			}
		}
	}
}
