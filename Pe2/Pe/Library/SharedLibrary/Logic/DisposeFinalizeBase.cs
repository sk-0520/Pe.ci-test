namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public abstract class DisposeFinalizeBase: IIsDisposed
	{
		protected DisposeFinalizeBase()
		{
			IsDisposed = false;
		}

		~DisposeFinalizeBase()
		{
			Dispose(false);
		}

		#region IIsDisposed

		public bool IsDisposed { get; protected set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		#region IDisposable

		/// <summary>
		/// 解放。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#endregion
	}
}
