namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public interface IVisible
	{
		/// <summary>
		/// 設定値としての表示・非表示。
		/// </summary>
		bool Visible { get; set; }
	}
}
