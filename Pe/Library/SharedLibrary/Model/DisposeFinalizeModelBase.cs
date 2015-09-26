namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
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

		#region IIsDisposed

		public event EventHandler Disposing = delegate { };

		[IgnoreDataMember, XmlIgnore]
		public bool IsDisposed { get; protected set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			Disposing(this, EventArgs.Empty);

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
