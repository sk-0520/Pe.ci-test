namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;

	public interface IUnmanagedModel: IDisposable
	{
		/// <summary>
		/// 破棄されたか。
		/// </summary>
		bool IsDisposed { get; }
	}
}
