namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Model
{
	using System;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.IF;

	public abstract class DisposeFinalizeModel: ModelBase, IUnmanagedModel
	{
		protected DisposeFinalizeModel()
		{
			IsDisposed = false;
		}

		~DisposeFinalizeModel()
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
