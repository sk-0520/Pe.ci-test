namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public abstract class DisposeFinalizeModelBase: ModelBase, IDisposeModel
	{
		protected DisposeFinalizeModelBase()
		{
			IsDisposed = false;
		}

		~DisposeFinalizeModelBase()
		{
			Dispose(false);
		}

		#region IUnmanagedModel

		public bool IsDisposed { get; protected set; }

		#endregion

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}

		/// <summary>
		/// 解放。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			if(!IsDisposed) {
				GC.SuppressFinalize(this);
			}
		}

		#endregion
	}
}
