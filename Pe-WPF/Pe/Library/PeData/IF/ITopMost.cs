using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	public interface ITopMost
	{
		/// <summary>
		/// 最前面。
		/// </summary>
		bool IsTopmost { get; set; }
	}
}
