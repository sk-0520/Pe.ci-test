namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ツールバーボタンの位置。
	/// <para>各値の意味はSystem.Drawing.StringAlignmentを踏襲。</para>
	/// </summary>
	public enum ToolbarButtonPosition
	{
		/// <summary>
		/// 最上位。
		/// </summary>
		Near,
		/// <summary>
		/// 中央。
		/// </summary>
		Center,
		/// <summary>
		/// 最下位。
		/// </summary>
		Far
	}
}
