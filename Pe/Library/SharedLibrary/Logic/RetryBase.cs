namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// 定型の再試行処理を行う。
	/// </summary>
	public abstract class RetryBase<T>
	{
		#region variable
		
		T _result;
		
		#endregion
		
		#region

		/// <summary>
		/// 待った回数。
		/// </summary>
		public int WaitCurrentCount { get; protected set; }
		/// <summary>
		/// 最大待ち回数。
		/// </summary>
		public int WaitMaxCount { get; private set; }
		/// <summary>
		/// 待ち過ぎた。
		/// </summary>
		public bool WaitOver { get { return WaitCurrentCount < WaitMaxCount;  } }

		public T Result { get { return this._result; } }

		#endregion

		#region function

		protected abstract bool Execute(int waitCurrentCount, ref T result);
		protected abstract void Wait(int waitCurrentCount);

		public void Run()
		{
			WaitCurrentCount = 0;
			while(WaitCurrentCount <= WaitMaxCount) {
				var result = Execute(WaitCurrentCount++, ref this._result);
				if(result) {
					return;
				} else {
					Wait(WaitCurrentCount);
				}
			}
		}

		#endregion
	}
}
