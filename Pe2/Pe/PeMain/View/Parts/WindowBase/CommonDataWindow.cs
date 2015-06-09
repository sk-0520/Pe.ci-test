namespace ContentTypeTextNet.Pe.PeMain.View.Parts.WindowBase
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class CommonDataWindow : Window, ICommonData
	{
		#region ICommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		public CommonData CommonData { get; private set; }

		#endregion

		#region function

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);
		}

		#endregion

	}
}
