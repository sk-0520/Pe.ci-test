namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;

	public interface IIsDisposed: IDisposable
	{
		/// <summary>
		/// Dispose時に呼び出されるイベント。
		/// <para>本イベントが呼び出されるとき、IsDisposedはまだfalse。</para>
		/// </summary>
		event EventHandler Disposing;

		/// <summary>
		/// 破棄されたか。
		/// </summary>
		bool IsDisposed { get; }
	}
}
