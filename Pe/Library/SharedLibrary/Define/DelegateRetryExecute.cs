namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public delegate bool DelegateRetryExecute<T>(int waitCurrentCount, ref T result);
}
