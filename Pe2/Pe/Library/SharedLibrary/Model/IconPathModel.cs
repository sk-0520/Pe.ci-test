namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// アイコンのパスを保持。
	/// </summary>
	[DataContract, Serializable]
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
