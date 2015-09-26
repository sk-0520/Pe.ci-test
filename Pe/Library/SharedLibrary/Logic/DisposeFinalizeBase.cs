namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
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

		[field: NonSerialized]
		public event EventHandler Disposing;

		[IgnoreDataMember, XmlIgnore]
		public bool IsDisposed { get; protected set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			if(Disposing != null) {
				Disposing(this, EventArgs.Empty);
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
