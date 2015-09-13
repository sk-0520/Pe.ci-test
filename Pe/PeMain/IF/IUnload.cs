namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// 読込データの解放。
	/// <para>IDisposableと違って殺さない。</para>
	/// </summary>
	public interface IUnload
	{
		/// <summary>
		/// 読込データが解放されているか。
		/// </summary>
		bool IsUnloaded { get; }
		/// <summary>
		/// 解放処理。
		/// </summary>
		void Unload();
	}
}
