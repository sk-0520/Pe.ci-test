namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Flags]
	public enum ClipboardType:int
	{
		None = 0x00,
		Text = 0x01,
		Rtf = 0x02,
		Html = 0x04,
		Image = 0x08,
		File = 0x10,
	}
}
