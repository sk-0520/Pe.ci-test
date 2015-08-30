namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;

	/// <summary>
	/// 前景・背景の二つの色を保持する。
	/// </summary>
	public interface IColorPair
	{
		/// <summary>
		/// 前景色。
		/// </summary>
		Color ForeColor { get; set; }
		/// <summary>
		/// 背景色。
		/// </summary>
		Color BackColor { get; set; }
	}
}
