namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// テンプレート置き換え。
	/// </summary>
	public enum TemplateReplaceMode
	{
		/// <summary>
		/// 置き換えない。
		/// </summary>
		None,
		/// <summary>
		/// テキスト置き換え。
		/// </summary>
		Text,
		/// <summary>
		/// T4置き換え。
		/// </summary>
		Program,
	}
}
