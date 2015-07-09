namespace ContentTypeTextNet.Pe.PeMain.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public enum HotKeyId: ushort
	{
		ShowCommand = 0x0001,
		HideFile,
		Extension,
		CreateNote,
		HiddenNote,
		CompactNote,
		ShowFrontNote,
		SwitchClipboardShow,
	}
}
