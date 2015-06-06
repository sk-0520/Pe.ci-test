namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Runtime.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[DataContract, Serializable]
	public abstract class DisposeFinalizeModelBase: ModelBase, IIsDisposed
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
			if(IsDisposed) {
				return;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 解放。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
