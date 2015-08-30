namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// コマンドと条件式のどちらを使用するか。
	/// </summary>
	public enum QueryCondition
	{
		/// <summary>
		/// コマンドを使用する。
		/// </summary>
		Command,
		/// <summary>
		/// 条件式を使用する。
		/// <para>入れ子で使用する。</para>
		/// </summary>
		Expression,
	}
}
