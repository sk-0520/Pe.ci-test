namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Model.Unmanaged.Com
{
	using System;
	using System.Runtime.InteropServices;

	/// <summary>
	/// 生のCOMを管理。
	/// </summary>
	public abstract class ComModelBase: UnmanagedModelBase
	{
		public ComModelBase(object rawCom)
			: base()
		{
			if(rawCom == null) {
				throw new ArgumentNullException("rawComObject");
			}

			RawCom = rawCom;
		}

		/// <summary>
		/// COMオブジェクト。
		/// </summary>
		public object RawCom { get; private set; }

		#region UnmanagedModelBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				Marshal.ReleaseComObject(RawCom);
				RawCom = null;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
