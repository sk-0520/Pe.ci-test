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

	public class AppHotkeyControl: HotkeyControl, ICommonData
	{
		public AppHotkeyControl()
		{ }

		#region property

		protected object ExtensionData { get; private set; }

		#endregion

		#region ICommonData

		public void SetCommonData(CommonData commonData, object extensionData)
		{
			CommonData = commonData;
			ExtensionData = extensionData;
		}

		public CommonData CommonData { get; private set; }

		#endregion
	}
}
