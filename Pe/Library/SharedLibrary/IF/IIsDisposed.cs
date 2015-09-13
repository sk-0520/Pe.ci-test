namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;

	public interface IIsDisposed: IDisposable
	{
		/// <summary>
		/// 破棄されたか。
		/// </summary>
		bool IsDisposed { get; }
	}
}
