namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ITId<T>
		where T: IComparable
	{
		/// <summary>
		/// ID。
		/// </summary>
		T Id { get; set; }

		/// <summary>
		/// IDが設定可能なものか。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool IsSafeId(T id);

		/// <summary>
		/// IDを設定可能なものに変更。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T ToSafeId(T id);
	}
}
