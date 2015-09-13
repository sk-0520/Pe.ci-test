namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ILimitSize
	{
		/// <summary>
		/// データ長。
		/// </summary>
		int LimitSize { get; set; }
	}
}
