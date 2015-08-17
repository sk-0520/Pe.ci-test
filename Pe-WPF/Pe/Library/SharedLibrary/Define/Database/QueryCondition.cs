using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Define.Database
{
	/// <summary>
	/// 条件が偽の場合にコマンドと条件式のどちらを使用するか。
	/// </summary>
	public enum QueryCondition
	{
		/// <summary>
		/// コマンドを使用する。
		/// </summary>
		Command,
		/// <summary>
		/// 条件式を使用する。
		/// </summary>
		Expression,
	}
}
