namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// 名前を持つ。
	/// </summary>
	public interface IName
	{
		/// <summary>
		/// 名前。
		/// </summary>
		string Name { get; set; }
	}
}
