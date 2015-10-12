namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public class DelegateRetry<T>: RetryBase<T>
	{
		#region property

		public DelegateRetryExecute<T> ExecuteFunc { get; set; }
		public Action<int> WaitAction { get; set; }

		#endregion

		#region function

		protected override bool Execute(int waitCurrentCount, ref T result)
		{
			return ExecuteFunc(waitCurrentCount, ref result);
		}

		protected override void Wait(int waitCurrentCount)
		{
			WaitAction(waitCurrentCount);
		}

		#endregion
	}
}
