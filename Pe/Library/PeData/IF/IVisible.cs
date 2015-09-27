namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// 表示・非表示状態を保持する。
	/// </summary>
	public interface IVisible
	{
		/// <summary>
		/// 設定値としての表示・非表示。
		/// </summary>
		bool IsVisible { get; set; }
	}
}
