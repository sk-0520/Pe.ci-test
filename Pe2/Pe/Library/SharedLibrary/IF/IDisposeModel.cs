namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;

	public interface IDisposeModel: IDisposable
	{
		/// <summary>
		/// 破棄されたか。
		/// </summary>
		bool IsDisposed { get; }
	}
}
