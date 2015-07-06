namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.View.Control;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class PeHotkeyControl: HotkeyControl, ICommonData
	{
		public PeHotkeyControl()
		{ }

		#region ICommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }

		#endregion
	}
}
