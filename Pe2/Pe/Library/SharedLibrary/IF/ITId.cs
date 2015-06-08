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
	}
}
