namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ITId<T>
	{
		/// <summary>
		/// ID。
		/// </summary>
		T Id { get; set; }
	}
}
