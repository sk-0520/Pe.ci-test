namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Logic
{
	using System;

	public abstract class DisposeFinalizeObject
	{
		protected DisposeFinalizeObject()
		{
			IsDisposed = false;
		}

		~DisposeFinalizeObject()
		{
			Dispose(false);
		}

		/// <summary>
		/// 破棄されたか。
		/// </summary>
		public bool IsDisposed { get; protected set; }

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
