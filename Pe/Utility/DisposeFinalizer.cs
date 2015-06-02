using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	public abstract class DisposeFinalizer: IDisposable
	{
		protected DisposeFinalizer()
		{
			IsDisposed = false;
		}

		~DisposeFinalizer()
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
			if(!IsDisposed) {
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		#endregion
	}
}
