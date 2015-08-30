namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Marshal.AllocHGlobalのラッパー。
	/// </summary>
	public class GlobalAllocModel: UnmanagedModelBase
	{
		#region static

		public static GlobalAllocModel Create<T>()
		{
			return new GlobalAllocModel(Marshal.SizeOf<T>());
		}

		public static GlobalAllocModel Create<T>(T structure)
		{
			var result = new GlobalAllocModel(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, result.Buffer, false);

			return result;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cb">確保するサイズ。</param>
		public GlobalAllocModel(int cb)
		{
			Buffer = Marshal.AllocHGlobal(cb);
			Size = cb;
		}

		#region property

		/// <summary>
		/// 確保領域のポインタ。
		/// </summary>
		public IntPtr Buffer { get; private set; }
		/// <summary>
		/// 確保サイズ。
		/// </summary>
		public int Size { get; private set; }

		#endregion

		#region UnmanagedModelBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				Marshal.FreeHGlobal(Buffer);
				Buffer = IntPtr.Zero;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
