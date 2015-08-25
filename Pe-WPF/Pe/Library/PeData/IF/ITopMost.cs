namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// 最前面を保持する。
	/// </summary>
	public interface ITopMost
	{
		/// <summary>
		/// 最前面。
		/// </summary>
		bool IsTopmost { get; set; }
	}
}
