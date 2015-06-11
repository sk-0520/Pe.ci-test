namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class CommonDataUserControl : UserControl, ICommonData
	{
		#region property

		//StartupWindowStatus Startup { get; set; }

		#endregion

		#region ICommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }

		#endregion

		#region function

		//protected virtual void ApplySetting()
		//{
		//	Debug.Assert(CommonData != null);

		//	ApplyViewModel();
		//	ApplyLanguage();
		//}

		//protected virtual void ApplyLanguage()
		//{
		//	Debug.Assert(CommonData != null);

		//	LanguageUtility.SetLanguage(this, CommonData.Language);
		//}

		//protected virtual void ApplyViewModel()
		//{
		//	Debug.Assert(CommonData != null);
		//}

		#endregion
	}
}
