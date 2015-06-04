using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	/// <summary>
	/// アイコンのパスを保持。
	/// </summary>
	public class IconPathModel: ModelBase
	{
		/// <summary>
		/// パス。
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// アイコンインデックス。
		/// </summary>
		public int Index { get; set; }
	}
}
