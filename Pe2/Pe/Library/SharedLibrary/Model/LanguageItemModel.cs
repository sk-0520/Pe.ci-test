namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// 言語データの最小データ。
	/// </summary>
	public class LanguageItemModel: ModelBase, IName
	{
		#region property

		/// <summary>
		/// 表示用文字列。
		/// </summary>
		public string Text { get; set; }

		#endregion

		#region IName

		/// <summary>
		/// キーとして使用される、
		/// </summary>
		public string Name { get; set; }

		#endregion
	}
}
