namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.Model;

	/// <summary>
	/// 何かしらのCOMを管理。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ComModel<T>: ComModelBase
	{
		public ComModel(T com)
			: base(com)
		{
			Com = com;
		}

		public T Com { get; private set; }
	}

}
