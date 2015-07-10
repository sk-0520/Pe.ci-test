namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Flags]
	public enum ClipboardType
	{
		None,
		Text,
		Rtf,
		Html,
		Image,
		File,
	}
}
