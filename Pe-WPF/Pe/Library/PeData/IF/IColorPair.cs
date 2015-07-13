namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;

	public interface IColorPair
	{
		Color ForeColor { get; set; }
		Color BackColor { get; set; }
	}
}
